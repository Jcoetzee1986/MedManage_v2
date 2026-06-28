using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using MedManage.Core.Entities;
using Xunit;

namespace MedManage.Infrastructure.Tests.Services;

/// <summary>
/// Property-based tests for Percentage Propagation Completeness (Property 1).
///
/// For any set of ServiceProvider_Tariff records in the prior period, after a successful
/// propagation job, the count of newly inserted ServiceProvider_Tariff records equals the
/// count of distinct (ServiceProviderId, TariffNameId, MainClientId) combinations that
/// existed for that prior period.
///
/// Since the TariffPercentageProcessor uses raw SQL (ExecuteSqlRawAsync) which doesn't work
/// with InMemoryDatabase, these tests simulate the propagation SQL logic (INSERT...SELECT
/// pattern with NOT EXISTS deduplication) to validate the completeness invariant.
///
/// **Validates: Requirements 6.2, 6.3**
/// </summary>
public class TariffPercentagePropagationCompletenessPropertyTests
{
    #region Propagation Logic Simulation

    /// <summary>
    /// Represents a ServiceProvider_Tariff record for testing purposes.
    /// </summary>
    public record ServiceProviderTariffRecord(
        int ServiceProviderId,
        int TariffNameId,
        int MainClientId,
        DateOnly? StartActiveDate,
        DateOnly? EndActiveDate,
        int TariffPeriodName,
        decimal PercentageAdded);

    /// <summary>
    /// Represents the result of a simulated propagation operation.
    /// </summary>
    public record PropagationResult(
        int ClosedCount,
        int InsertedCount,
        List<ServiceProviderTariffRecord> NewRecords);

    /// <summary>
    /// Simulates the SQL propagation logic from TariffPercentageProcessor.ProcessTariffUpdateJob:
    /// 
    /// Step 1: Close prior period records (SET EndActiveDate WHERE TariffPeriodName = priorYear AND EndActiveDate IS NULL)
    /// Step 2: INSERT INTO ... SELECT ... FROM WHERE TariffPeriodName = priorYear AND EndActiveDate = closedDate
    ///         AND NOT EXISTS (matching record for the new period)
    /// 
    /// This mirrors the actual SQL statements in the processor.
    /// </summary>
    public static PropagationResult SimulatePropagation(
        List<ServiceProviderTariffRecord> existingRecords,
        int jobTariffPeriodName,
        DateOnly jobStartActiveDate,
        DateOnly? jobEndActiveDate,
        decimal jobPercentageAdded)
    {
        int priorYear = jobTariffPeriodName - 1;
        var closedEndDate = jobStartActiveDate.AddDays(-1);

        // Step 1: Close prior period records (simulate UPDATE SET EndActiveDate)
        var recordsToClose = existingRecords
            .Where(r => r.EndActiveDate == null && r.TariffPeriodName == priorYear)
            .ToList();

        int closedCount = recordsToClose.Count;

        // Apply closure: create updated versions with EndActiveDate set
        var updatedRecords = existingRecords.Select(r =>
        {
            if (r.EndActiveDate == null && r.TariffPeriodName == priorYear)
            {
                return r with { EndActiveDate = closedEndDate };
            }
            return r;
        }).ToList();

        // Step 2: Select from prior period WHERE TariffPeriodName = priorYear AND EndActiveDate = closedEndDate
        var sourceRecords = updatedRecords
            .Where(r => r.TariffPeriodName == priorYear && r.EndActiveDate == closedEndDate)
            .ToList();

        // Determine existing records for the new period (for NOT EXISTS check)
        var existingNewPeriodCombinations = existingRecords
            .Where(r => r.TariffPeriodName == jobTariffPeriodName)
            .Select(r => (r.ServiceProviderId, r.TariffNameId, r.MainClientId))
            .ToHashSet();

        // Insert new records, skipping duplicates (NOT EXISTS clause)
        var newRecords = sourceRecords
            .Where(r => !existingNewPeriodCombinations.Contains(
                (r.ServiceProviderId, r.TariffNameId, r.MainClientId)))
            .Select(r => new ServiceProviderTariffRecord(
                r.ServiceProviderId,
                r.TariffNameId,
                r.MainClientId,
                jobStartActiveDate,
                jobEndActiveDate,
                jobTariffPeriodName,
                jobPercentageAdded))
            .ToList();

        return new PropagationResult(closedCount, newRecords.Count, newRecords);
    }

    #endregion

    #region Custom Generators

    /// <summary>
    /// Represents a prior period record with a unique (ServiceProviderId, TariffNameId, MainClientId) combination.
    /// EndActiveDate is null (active) so it can be closed by the propagation logic.
    /// </summary>
    public record PriorPeriodInput(
        int ServiceProviderId,
        int TariffNameId,
        int MainClientId);

    /// <summary>
    /// Generates a set of prior period records with distinct combinations for the simple case
    /// (no pre-existing new period records, all records have null EndActiveDate).
    /// </summary>
    private static Arbitrary<PriorPeriodInput[]> DistinctPriorPeriodRecordsArbitrary()
    {
        var inputGen = from serviceProviderId in Gen.Choose(1, 1000)
                       from tariffNameId in Gen.Choose(1, 50)
                       from mainClientId in Gen.Choose(1, 20)
                       select new PriorPeriodInput(serviceProviderId, tariffNameId, mainClientId);

        var listGen = Gen.NonEmptyListOf(inputGen)
            .Select(list => list
                .GroupBy(r => (r.ServiceProviderId, r.TariffNameId, r.MainClientId))
                .Select(g => g.First())
                .ToArray());

        return Arb.From(listGen);
    }

    /// <summary>
    /// Generates a set of prior period records that may contain multiple records for the same
    /// combination (simulating realistic data where each combination has at most one active record
    /// with null EndActiveDate, and possibly older closed records).
    /// 
    /// This reflects real database state: a (ServiceProviderId, TariffNameId, MainClientId) combination
    /// has at most one active (null EndActiveDate) record per period, but may have historical closed records.
    /// </summary>
    private static Arbitrary<ServiceProviderTariffRecord[]> MixedPriorPeriodRecordsArbitrary()
    {
        var recordGen = from serviceProviderId in Gen.Choose(1, 100)
                        from tariffNameId in Gen.Choose(1, 20)
                        from mainClientId in Gen.Choose(1, 10)
                        from hasEndDate in Gen.Frequency(
                            Tuple.Create(2, Gen.Constant(false)),  // Most records are active (null EndActiveDate)
                            Tuple.Create(1, Gen.Constant(true)))   // Some already closed
                        from endOffset in Gen.Choose(1, 365)
                        from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                        let startDate = new DateOnly(2024, 1, 1)
                        let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
                        select new ServiceProviderTariffRecord(
                            serviceProviderId, tariffNameId, mainClientId,
                            startDate, endDate, 2025, percentage);

        var listGen = Gen.NonEmptyListOf(recordGen)
            .Where(list => list.Any(r => r.EndActiveDate == null)) // Ensure at least one active record
            .Select(list =>
            {
                // Ensure at most one active (null EndActiveDate) record per combination,
                // reflecting real DB constraint. Keep the first active record per combination
                // and allow multiple closed records (different EndActiveDates).
                var activeByCombo = new HashSet<(int, int, int)>();
                var result = new List<ServiceProviderTariffRecord>();
                foreach (var r in list)
                {
                    var key = (r.ServiceProviderId, r.TariffNameId, r.MainClientId);
                    if (r.EndActiveDate == null)
                    {
                        if (activeByCombo.Add(key))
                            result.Add(r);
                        // Skip duplicates for the same combo with null EndActiveDate
                    }
                    else
                    {
                        result.Add(r);
                    }
                }
                return result.ToArray();
            })
            .Where(arr => arr.Any(r => r.EndActiveDate == null)); // Re-validate after dedup

        return Arb.From(listGen);
    }

    #endregion

    #region Property: Inserted count equals distinct combination count from prior period (simple case)

    /// <summary>
    /// Property: For any set of distinct (ServiceProviderId, TariffNameId, MainClientId) combinations
    /// in the prior period where all records have null EndActiveDate and no existing records for the
    /// new period, after propagation, the inserted count equals the number of distinct combinations.
    ///
    /// This is the core completeness invariant: every unique provider/tariff/client combination
    /// from the prior period gets exactly one new record in the new period.
    ///
    /// **Validates: Requirements 6.2, 6.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Inserted_Count_Equals_Distinct_Combinations_From_Prior_Period()
    {
        var gen = from records in DistinctPriorPeriodRecordsArbitrary().Generator
                  from jobYear in Gen.Choose(2001, 2100)
                  from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from startMonth in Gen.Choose(1, 12)
                  from startDay in Gen.Choose(1, 28)
                  let startDate = new DateOnly(jobYear, startMonth, startDay)
                  select new
                  {
                      Records = records,
                      JobYear = jobYear,
                      Percentage = percentage,
                      StartDate = startDate
                  };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            int priorYear = data.JobYear - 1;

            // Build prior period records with null EndActiveDate (active records)
            var existingRecords = data.Records.Select(r => new ServiceProviderTariffRecord(
                r.ServiceProviderId, r.TariffNameId, r.MainClientId,
                new DateOnly(priorYear, 1, 1), null, priorYear, 200.0000m
            )).ToList();

            // Run propagation simulation
            var result = SimulatePropagation(
                existingRecords,
                data.JobYear,
                data.StartDate,
                null,
                data.Percentage);

            // Expected: inserted count = number of distinct combinations in prior period
            var expectedCount = data.Records.Length;

            return (result.InsertedCount == expectedCount)
                .Label($"Expected {expectedCount} inserted records, got {result.InsertedCount}");
        });
    }

    #endregion

    #region Property: Inserted count equals distinct active (null EndActiveDate) combinations

    /// <summary>
    /// Property: For a mixed set of prior period records (some with EndActiveDate null, some already closed),
    /// after propagation, the inserted count equals the distinct (ServiceProviderId, TariffNameId, MainClientId)
    /// combinations from records that had null EndActiveDate (i.e., were active and got closed by Step 1).
    ///
    /// Records that already had an EndActiveDate set are NOT included in the propagation source
    /// because Step 2 filters on EndActiveDate = closedEndDate (which only matches freshly closed records).
    ///
    /// **Validates: Requirements 6.2, 6.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Inserted_Count_Equals_Distinct_Active_Combinations_When_Mixed_Records()
    {
        var gen = from records in MixedPriorPeriodRecordsArbitrary().Generator
                  from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from startDay in Gen.Choose(1, 28)
                  select new { Records = records, Percentage = percentage, StartDay = startDay };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            int priorYear = 2025;
            int jobYear = 2026;
            var jobStartDate = new DateOnly(jobYear, 1, data.StartDay);

            // Run propagation simulation
            var result = SimulatePropagation(
                data.Records.ToList(),
                jobYear,
                jobStartDate,
                null,
                data.Percentage);

            // Expected: distinct combinations from records that had null EndActiveDate
            var expectedDistinctCombinations = data.Records
                .Where(r => r.EndActiveDate == null && r.TariffPeriodName == priorYear)
                .Select(r => (r.ServiceProviderId, r.TariffNameId, r.MainClientId))
                .Distinct()
                .Count();

            return (result.InsertedCount == expectedDistinctCombinations)
                .Label($"Expected {expectedDistinctCombinations} inserted records (distinct active combos), got {result.InsertedCount}");
        });
    }

    #endregion

    #region Property: No records inserted when no active prior period records exist

    /// <summary>
    /// Property: When no ServiceProvider_Tariff records exist for the prior period with null EndActiveDate,
    /// propagation inserts zero records and completes successfully.
    /// 
    /// This covers Requirement 6.7: "WHEN no ServiceProvider_Tariff_Records exist for the prior period...
    /// THE Background_Processor SHALL complete the job with RecordsAffected set to zero"
    ///
    /// **Validates: Requirements 6.2, 6.3**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Zero_Records_Inserted_When_No_Active_Prior_Period_Records()
    {
        var gen = from recordCount in Gen.Choose(0, 20)
                  from jobYear in Gen.Choose(2001, 2100)
                  from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from records in Gen.ListOf(recordCount,
                      from spId in Gen.Choose(1, 100)
                      from tnId in Gen.Choose(1, 50)
                      from mcId in Gen.Choose(1, 20)
                      from endOffset in Gen.Choose(1, 300) // Limit to 300 to avoid hitting closedEndDate (Dec 31)
                      select new ServiceProviderTariffRecord(
                          spId, tnId, mcId,
                          new DateOnly(jobYear - 1, 1, 1),
                          new DateOnly(jobYear - 1, 1, 1).AddDays(endOffset), // All have EndActiveDate set (already closed)
                          jobYear - 1,
                          200.0000m))
                  // Filter out any records whose EndActiveDate coincidentally equals the closedEndDate
                  let jobStartDate = new DateOnly(jobYear, 1, 1)
                  let closedEndDate = jobStartDate.AddDays(-1)
                  where records.All(r => r.EndActiveDate != closedEndDate)
                  select new { Records = records, JobYear = jobYear, Percentage = percentage };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            var jobStartDate = new DateOnly(data.JobYear, 1, 1);

            // Run propagation with records that all already have EndActiveDate set
            var result = SimulatePropagation(
                data.Records.ToList(),
                data.JobYear,
                jobStartDate,
                null,
                data.Percentage);

            // No active (null EndActiveDate) records means nothing to close or propagate.
            // Also, no records have EndActiveDate matching the closedEndDate, so Step 2 finds no sources.
            return (result.InsertedCount == 0 && result.ClosedCount == 0)
                .Label($"Expected 0 inserted and 0 closed, got {result.InsertedCount} inserted and {result.ClosedCount} closed");
        });
    }

    #endregion

    #region Property: Existing new period records are not duplicated (NOT EXISTS guard)

    /// <summary>
    /// Property: When some (ServiceProviderId, TariffNameId, MainClientId) combinations already
    /// have records in the new period, propagation skips those combinations and only inserts
    /// records for combinations that don't yet exist in the new period.
    /// 
    /// Inserted count = distinct active prior combinations - already-existing new period combinations.
    ///
    /// **Validates: Requirements 6.2, 6.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Existing_New_Period_Records_Are_Skipped()
    {
        var gen = from combinationCount in Gen.Choose(2, 20)
                  from preExistingRatio in Gen.Choose(1, combinationCount - 1)
                  from jobYear in Gen.Choose(2001, 2100)
                  from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from combinations in Gen.ListOf(combinationCount,
                      from spId in Gen.Choose(1, 500)
                      from tnId in Gen.Choose(1, 50)
                      from mcId in Gen.Choose(1, 20)
                      select new PriorPeriodInput(spId, tnId, mcId))
                  let distinctCombinations = combinations
                      .GroupBy(c => (c.ServiceProviderId, c.TariffNameId, c.MainClientId))
                      .Select(g => g.First())
                      .ToList()
                  where distinctCombinations.Count >= 2
                  let preExistCount = Math.Min(preExistingRatio, distinctCombinations.Count - 1)
                  select new
                  {
                      DistinctCombinations = distinctCombinations,
                      PreExistCount = preExistCount,
                      JobYear = jobYear,
                      Percentage = percentage
                  };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            int priorYear = data.JobYear - 1;
            var jobStartDate = new DateOnly(data.JobYear, 1, 1);

            // Build prior period records (all active - null EndActiveDate)
            var existingRecords = data.DistinctCombinations.Select(c => new ServiceProviderTariffRecord(
                c.ServiceProviderId, c.TariffNameId, c.MainClientId,
                new DateOnly(priorYear, 1, 1), null, priorYear, 200.0000m
            )).ToList();

            // Add some pre-existing records for the new period (these should be skipped)
            var preExistingCombos = data.DistinctCombinations.Take(data.PreExistCount).ToList();
            foreach (var combo in preExistingCombos)
            {
                existingRecords.Add(new ServiceProviderTariffRecord(
                    combo.ServiceProviderId, combo.TariffNameId, combo.MainClientId,
                    jobStartDate, null, data.JobYear, 100.0000m));
            }

            // Run propagation
            var result = SimulatePropagation(
                existingRecords,
                data.JobYear,
                jobStartDate,
                null,
                data.Percentage);

            // Expected: total distinct combos minus pre-existing ones
            var expectedInserted = data.DistinctCombinations.Count - data.PreExistCount;

            return (result.InsertedCount == expectedInserted)
                .Label($"Expected {expectedInserted} inserted (total {data.DistinctCombinations.Count} - preExisting {data.PreExistCount}), got {result.InsertedCount}");
        });
    }

    #endregion

    #region Property: Each inserted record has correct job properties

    /// <summary>
    /// Property: All newly inserted records carry the job's StartActiveDate, EndActiveDate,
    /// TariffPeriodName, and PercentageAdded values, and each record corresponds to a unique
    /// (ServiceProviderId, TariffNameId, MainClientId) combination from the prior period.
    ///
    /// **Validates: Requirements 6.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Inserted_Records_Have_Correct_Job_Properties()
    {
        var gen = from records in DistinctPriorPeriodRecordsArbitrary().Generator
                  from jobYear in Gen.Choose(2001, 2100)
                  from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from startMonth in Gen.Choose(1, 12)
                  from startDay in Gen.Choose(1, 28)
                  from hasEndDate in Arb.Generate<bool>()
                  from endOffset in Gen.Choose(1, 365)
                  let startDate = new DateOnly(jobYear, startMonth, startDay)
                  let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
                  select new
                  {
                      Records = records,
                      JobYear = jobYear,
                      Percentage = percentage,
                      StartDate = startDate,
                      EndDate = endDate
                  };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            int priorYear = data.JobYear - 1;

            var existingRecords = data.Records.Select(r => new ServiceProviderTariffRecord(
                r.ServiceProviderId, r.TariffNameId, r.MainClientId,
                new DateOnly(priorYear, 1, 1), null, priorYear, 200.0000m
            )).ToList();

            var result = SimulatePropagation(
                existingRecords,
                data.JobYear,
                data.StartDate,
                data.EndDate,
                data.Percentage);

            // Verify all inserted records have correct job properties
            var allCorrect = result.NewRecords.All(r =>
                r.StartActiveDate == data.StartDate &&
                r.EndActiveDate == data.EndDate &&
                r.TariffPeriodName == data.JobYear &&
                r.PercentageAdded == data.Percentage);

            // Verify each inserted record maps to a prior period combination
            var priorCombinations = data.Records
                .Select(r => (r.ServiceProviderId, r.TariffNameId, r.MainClientId))
                .ToHashSet();

            var allMapped = result.NewRecords.All(r =>
                priorCombinations.Contains((r.ServiceProviderId, r.TariffNameId, r.MainClientId)));

            return (allCorrect && allMapped)
                .Label($"All inserted records must have correct job properties and map to prior period combinations. " +
                       $"Properties correct: {allCorrect}, All mapped: {allMapped}");
        });
    }

    #endregion
}
