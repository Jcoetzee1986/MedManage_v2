using System.Threading.Channels;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Services.Business;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace MedManage.Infrastructure.Tests.Services;

/// <summary>
/// Property-based tests for Case Letter Two-Year Window (Property 5).
///
/// For any collection of completed TariffPercentage records, GetActivePercentagesForLetterAsync
/// returns at most 2 records representing the two most recent years, selects the record with
/// the latest EndActiveDate per year (null = highest priority), and orders results by
/// TariffPeriodName descending.
///
/// **Validates: Requirements 8.1, 8.2, 8.5**
/// </summary>
public class TariffPercentageCaseLetterPropertyTests : IDisposable
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    public TariffPercentageCaseLetterPropertyTests()
    {
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _currentUserServiceMock.Setup(x => x.UserId).Returns("test-user-letter");
        _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);
    }

    public void Dispose()
    {
        // Each test uses its own context, no shared state to dispose
    }

    private TariffPercentageService CreateService(MedManageDbContext context)
    {
        var channel = Channel.CreateUnbounded<TariffUpdateJob>();
        return new TariffPercentageService(context, _currentUserServiceMock.Object, channel.Writer);
    }

    private MedManageDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"CaseLetterTests_{Guid.NewGuid()}")
            .Options;

        return new MedManageDbContext(options);
    }

    #region Custom Generators

    /// <summary>
    /// Represents a completed TariffPercentage record for seeding in tests.
    /// </summary>
    public record CompletedRecord(int Year, decimal Percentage, DateOnly StartDate, DateOnly? EndDate);

    /// <summary>
    /// Generates a non-empty list of completed TariffPercentage records with varying years and end dates.
    /// Years are constrained to 2000-2100 range. Multiple records per year are allowed to test
    /// the "latest EndActiveDate" selection logic.
    /// </summary>
    private static Arbitrary<CompletedRecord[]> CompletedRecordsArbitrary()
    {
        var recordGen = from year in Gen.Choose(2020, 2030)
                        from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
                        from startMonth in Gen.Choose(1, 12)
                        from startDay in Gen.Choose(1, 28)
                        from hasEndDate in Gen.Frequency(
                            Tuple.Create(3, Gen.Constant(true)),
                            Tuple.Create(1, Gen.Constant(false)))
                        from endOffset in Gen.Choose(1, 365)
                        let startDate = new DateOnly(year, startMonth, startDay)
                        let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
                        select new CompletedRecord(year, percentage, startDate, endDate);

        var listGen = Gen.NonEmptyListOf(recordGen)
            .Select(list => list.ToArray());

        return Arb.From(listGen);
    }

    /// <summary>
    /// Generates a collection of records that spans multiple years (at least 2 distinct years)
    /// with multiple records per year to thoroughly test grouping and selection logic.
    /// </summary>
    private static Arbitrary<CompletedRecord[]> MultiYearRecordsArbitrary()
    {
        var gen = from yearCount in Gen.Choose(2, 5)
                  from baseYear in Gen.Choose(2020, 2028)
                  from recordsPerYear in Gen.Choose(1, 4)
                  from records in GenRecordsForYears(baseYear, yearCount, recordsPerYear)
                  select records;

        return Arb.From(gen);
    }

    private static Gen<CompletedRecord[]> GenRecordsForYears(int baseYear, int yearCount, int recordsPerYear)
    {
        var years = Enumerable.Range(baseYear, yearCount).ToArray();
        var recordGens = years.SelectMany(year =>
            Enumerable.Range(0, recordsPerYear).Select(_ => GenRecordForYear(year)));

        return Gen.Sequence(recordGens).Select(records => records.ToArray());
    }

    private static Gen<CompletedRecord> GenRecordForYear(int year)
    {
        return from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
               from startMonth in Gen.Choose(1, 12)
               from startDay in Gen.Choose(1, 28)
               from hasEndDate in Gen.Frequency(
                   Tuple.Create(3, Gen.Constant(true)),
                   Tuple.Create(1, Gen.Constant(false)))
               from endOffset in Gen.Choose(1, 365)
               let startDate = new DateOnly(year, startMonth, startDay)
               let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
               select new CompletedRecord(year, percentage, startDate, endDate);
    }

    #endregion

    #region Property: Returns at most 2 records with distinct TariffPeriodName values

    /// <summary>
    /// Property: For any collection of completed TariffPercentage records,
    /// GetActivePercentagesForLetterAsync returns at most 2 records with distinct TariffPeriodName values.
    /// **Validates: Requirements 8.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Returns_At_Most_Two_Records_With_Distinct_Years()
    {
        return Prop.ForAll(CompletedRecordsArbitrary(), records =>
        {
            using var context = CreateContext();
            var service = CreateService(context);

            // Seed completed records
            SeedRecords(context, records);

            // Act
            var result = service.GetActivePercentagesForLetterAsync().GetAwaiter().GetResult().ToList();

            // Assert: at most 2 records
            var atMostTwo = result.Count <= 2;

            // Assert: distinct years
            var distinctYears = result.Select(r => r.TariffPeriodName).Distinct().Count() == result.Count;

            return (atMostTwo && distinctYears)
                .Label($"Expected at most 2 distinct years, got {result.Count} records with years: [{string.Join(", ", result.Select(r => r.TariffPeriodName))}]");
        });
    }

    #endregion

    #region Property: Results represent the two most recent years

    /// <summary>
    /// Property: The returned records represent the two most recent years (highest TariffPeriodName values)
    /// from the completed data.
    /// **Validates: Requirements 8.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Returns_Two_Most_Recent_Years()
    {
        return Prop.ForAll(CompletedRecordsArbitrary(), records =>
        {
            using var context = CreateContext();
            var service = CreateService(context);

            SeedRecords(context, records);

            // Act
            var result = service.GetActivePercentagesForLetterAsync().GetAwaiter().GetResult().ToList();

            // Determine expected years: top 2 distinct years from input
            var expectedYears = records
                .Select(r => r.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .Take(2)
                .ToList();

            var resultYears = result.Select(r => r.TariffPeriodName).OrderByDescending(y => y).ToList();

            return resultYears.SequenceEqual(expectedYears)
                .Label($"Expected years [{string.Join(", ", expectedYears)}], got [{string.Join(", ", resultYears)}]");
        });
    }

    #endregion

    #region Property: Null EndActiveDate takes highest priority per year

    /// <summary>
    /// Property: When a year has both null and non-null EndActiveDate records,
    /// the record with null EndActiveDate (representing currently active) is selected.
    /// **Validates: Requirements 8.2**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Null_EndActiveDate_Takes_Highest_Priority()
    {
        return Prop.ForAll(MultiYearRecordsArbitrary(), records =>
        {
            using var context = CreateContext();
            var service = CreateService(context);

            SeedRecords(context, records);

            // Act
            var result = service.GetActivePercentagesForLetterAsync().GetAwaiter().GetResult().ToList();

            // For each year in the result, verify priority logic
            foreach (var dto in result)
            {
                var yearRecords = records.Where(r => r.Year == dto.TariffPeriodName).ToList();

                // If any record for this year has null EndActiveDate, the result should also have null
                var hasNullEndDate = yearRecords.Any(r => r.EndDate == null);

                if (hasNullEndDate)
                {
                    if (dto.EndActiveDate != null)
                    {
                        return false.Label($"Year {dto.TariffPeriodName} has records with null EndActiveDate but result has EndActiveDate={dto.EndActiveDate}");
                    }
                }
                else
                {
                    // No null EndDate records - the result should have the latest EndActiveDate
                    var maxEndDate = yearRecords.Max(r => r.EndDate!.Value);
                    if (dto.EndActiveDate != maxEndDate)
                    {
                        return false.Label($"Year {dto.TariffPeriodName} should have EndActiveDate={maxEndDate} but got {dto.EndActiveDate}");
                    }
                }
            }

            return true.Label("Null EndActiveDate correctly prioritized for all years");
        });
    }

    #endregion

    #region Property: Results are ordered by TariffPeriodName descending

    /// <summary>
    /// Property: The returned results are ordered by TariffPeriodName descending.
    /// **Validates: Requirements 8.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property Results_Ordered_By_TariffPeriodName_Descending()
    {
        return Prop.ForAll(CompletedRecordsArbitrary(), records =>
        {
            using var context = CreateContext();
            var service = CreateService(context);

            SeedRecords(context, records);

            // Act
            var result = service.GetActivePercentagesForLetterAsync().GetAwaiter().GetResult().ToList();

            // Assert: results are in descending order by TariffPeriodName
            if (result.Count <= 1)
                return true.Label("Single or empty result is trivially ordered");

            var isDescending = true;
            for (int i = 0; i < result.Count - 1; i++)
            {
                if (result[i].TariffPeriodName < result[i + 1].TariffPeriodName)
                {
                    isDescending = false;
                    break;
                }
            }

            return isDescending.Label($"Results should be ordered descending, got: [{string.Join(", ", result.Select(r => r.TariffPeriodName))}]");
        });
    }

    #endregion

    #region Property: Empty collection when no completed records exist

    /// <summary>
    /// Property: When no completed records exist (only non-completed or deleted),
    /// the method returns an empty collection.
    /// **Validates: Requirements 8.1**
    /// </summary>
    [Fact]
    public async Task Returns_Empty_When_No_Completed_Records()
    {
        using var context = CreateContext();
        var service = CreateService(context);

        // Seed only non-completed records
        var statuses = new[] { "Pending", "Processing", "Failed" };
        foreach (var status in statuses)
        {
            context.Set<TariffPercentage>().Add(new TariffPercentage
            {
                PercentageAdded = 100m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = null,
                Status = status,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            });
        }

        // Add a completed but soft-deleted record
        context.Set<TariffPercentage>().Add(new TariffPercentage
        {
            PercentageAdded = 200m,
            TariffPeriodName = 2025,
            StartActiveDate = new DateOnly(2025, 1, 1),
            EndActiveDate = null,
            Status = "Completed",
            DateInserted = DateTime.UtcNow,
            DateDeleted = DateTime.UtcNow,
            UserID = "test-user"
        });

        await context.SaveChangesAsync();

        // Act
        var result = await service.GetActivePercentagesForLetterAsync();

        // Assert
        result.Should().BeEmpty("no completed non-deleted records exist");
    }

    #endregion

    #region Helpers

    private static void SeedRecords(MedManageDbContext context, CompletedRecord[] records)
    {
        foreach (var record in records)
        {
            context.Set<TariffPercentage>().Add(new TariffPercentage
            {
                PercentageAdded = record.Percentage,
                TariffPeriodName = record.Year,
                StartActiveDate = record.StartDate,
                EndActiveDate = record.EndDate,
                Status = "Completed",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            });
        }

        context.SaveChanges();
    }

    #endregion
}
