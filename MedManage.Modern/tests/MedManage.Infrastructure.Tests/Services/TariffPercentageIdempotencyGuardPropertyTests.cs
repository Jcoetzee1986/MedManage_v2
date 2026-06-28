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
/// Property-based tests for Idempotency Guard (Property 4).
/// 
/// For any TariffPercentage record, attempting to apply it when it has Status "Completed"
/// or when another job is already "Processing" for the same TariffPeriodName is rejected,
/// preventing duplicate ServiceProvider_Tariff records from being created.
/// 
/// **Validates: Requirements 5.2, 5.3, 5.5, 11.2, 11.3**
/// </summary>
public class TariffPercentageIdempotencyGuardPropertyTests : IDisposable
{
    private readonly MedManageDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Channel<TariffUpdateJob> _channel;

    public TariffPercentageIdempotencyGuardPropertyTests()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"IdempotencyGuardTests_{Guid.NewGuid()}")
            .Options;

        _context = new MedManageDbContext(options);
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _currentUserServiceMock.Setup(x => x.UserId).Returns("test-user-001");
        _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);

        _channel = Channel.CreateUnbounded<TariffUpdateJob>();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    /// <summary>
    /// Creates a TariffPercentageService with a fresh channel for isolated testing.
    /// </summary>
    private TariffPercentageService CreateService(MedManageDbContext context)
    {
        var channel = Channel.CreateUnbounded<TariffUpdateJob>();
        return new TariffPercentageService(context, _currentUserServiceMock.Object, channel.Writer);
    }

    #region Property: Applying a "Completed" record is always rejected

    /// <summary>
    /// Property: For any TariffPercentage record with Status "Completed", ApplyPercentageAsync
    /// always throws InvalidOperationException regardless of the record's other properties.
    /// 
    /// **Validates: Requirements 5.3**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Applying_Completed_Record_Is_Always_Rejected()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from startDay in Gen.Choose(1, 28)
                  from startMonth in Gen.Choose(1, 12)
                  from hasEndDate in Arb.Generate<bool>()
                  from endOffset in Gen.Choose(1, 365)
                  let startDate = new DateOnly(year, startMonth, startDay)
                  let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
                  select new { Percentage = percentage, Year = year, StartDate = startDate, EndDate = endDate };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"CompletedRejectTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record with "Completed" status
            var entity = new TariffPercentage
            {
                PercentageAdded = data.Percentage,
                TariffPeriodName = data.Year,
                StartActiveDate = data.StartDate,
                EndActiveDate = data.EndDate,
                Status = "Completed",
                RecordsAffected = 100,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            // Act: attempt to apply the completed record
            InvalidOperationException? caught = null;
            try
            {
                service.ApplyPercentageAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
            }
            catch (InvalidOperationException ex)
            {
                caught = ex;
            }

            return (caught != null)
                .Label("Applying a 'Completed' record must always be rejected with InvalidOperationException");
        });
    }

    #endregion

    #region Property: Applying when another job is "Processing" for same period is always rejected

    /// <summary>
    /// Property: For any TariffPercentage record in "Pending" or "Failed" state, if another
    /// record with the same TariffPeriodName is already "Processing", ApplyPercentageAsync
    /// always throws InvalidOperationException.
    /// 
    /// **Validates: Requirements 5.2**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Applying_When_Same_Period_Is_Processing_Is_Always_Rejected()
    {
        var statusGen = Gen.Elements(new[] { "Pending", "Failed" });

        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from startDay in Gen.Choose(1, 28)
                  from startMonth in Gen.Choose(1, 12)
                  from status in statusGen
                  let startDate = new DateOnly(year, startMonth, startDay)
                  select new { Percentage = percentage, Year = year, StartDate = startDate, Status = status };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"SamePeriodProcessingTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed an existing "Processing" record for the same TariffPeriodName
            var processingEntity = new TariffPercentage
            {
                PercentageAdded = 200.0000m,
                TariffPeriodName = data.Year,
                StartActiveDate = new DateOnly(data.Year, 1, 1),
                EndActiveDate = new DateOnly(data.Year, 6, 30),
                Status = "Processing",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(processingEntity);

            // Seed the record we want to apply (different date range to avoid overlap conflicts)
            var targetEntity = new TariffPercentage
            {
                PercentageAdded = data.Percentage,
                TariffPeriodName = data.Year,
                StartActiveDate = new DateOnly(data.Year, 7, 1),
                EndActiveDate = new DateOnly(data.Year, 12, 31),
                Status = data.Status,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(targetEntity);
            context.SaveChanges();

            // Act: attempt to apply while same period is processing
            InvalidOperationException? caught = null;
            try
            {
                service.ApplyPercentageAsync(targetEntity.TariffPercentageId).GetAwaiter().GetResult();
            }
            catch (InvalidOperationException ex)
            {
                caught = ex;
            }

            return (caught != null)
                .Label($"Applying when another job is 'Processing' for same period must be rejected (target status: '{data.Status}')");
        });
    }

    #endregion

    #region Property: Applying a "Processing" record is always rejected

    /// <summary>
    /// Property: For any TariffPercentage record with Status "Processing", ApplyPercentageAsync
    /// always throws InvalidOperationException (cannot re-apply while already processing).
    /// 
    /// **Validates: Requirements 5.2**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Applying_Processing_Record_Is_Always_Rejected()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from startDay in Gen.Choose(1, 28)
                  from startMonth in Gen.Choose(1, 12)
                  let startDate = new DateOnly(year, startMonth, startDay)
                  select new { Percentage = percentage, Year = year, StartDate = startDate };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"ProcessingRejectTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record with "Processing" status
            var entity = new TariffPercentage
            {
                PercentageAdded = data.Percentage,
                TariffPeriodName = data.Year,
                StartActiveDate = data.StartDate,
                EndActiveDate = null,
                Status = "Processing",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            // Act: attempt to apply the processing record
            InvalidOperationException? caught = null;
            try
            {
                service.ApplyPercentageAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
            }
            catch (InvalidOperationException ex)
            {
                caught = ex;
            }

            return (caught != null)
                .Label("Applying a 'Processing' record must always be rejected with InvalidOperationException");
        });
    }

    #endregion

    #region Property: Repeated apply attempts on the same record do not create duplicates

    /// <summary>
    /// Property: For any TariffPercentage record, repeated sequential apply attempts result in
    /// at most one successful transition to "Processing" - subsequent attempts are rejected,
    /// ensuring no duplicate propagation jobs can be queued.
    /// 
    /// **Validates: Requirements 5.5, 11.2, 11.3**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Repeated_Apply_Attempts_Only_Succeed_Once()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from startDay in Gen.Choose(1, 28)
                  from startMonth in Gen.Choose(1, 12)
                  from attemptCount in Gen.Choose(2, 10)
                  let startDate = new DateOnly(year, startMonth, startDay)
                  select new { Percentage = percentage, Year = year, StartDate = startDate, AttemptCount = attemptCount };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"RepeatedApplyTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var channel = Channel.CreateUnbounded<TariffUpdateJob>();
            var service = new TariffPercentageService(context, _currentUserServiceMock.Object, channel.Writer);

            // Seed a "Pending" record
            var entity = new TariffPercentage
            {
                PercentageAdded = data.Percentage,
                TariffPeriodName = data.Year,
                StartActiveDate = data.StartDate,
                EndActiveDate = null,
                Status = "Pending",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            // Apply multiple times
            int successCount = 0;
            int rejectionCount = 0;

            for (int i = 0; i < data.AttemptCount; i++)
            {
                try
                {
                    service.ApplyPercentageAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
                    successCount++;
                }
                catch (InvalidOperationException)
                {
                    rejectionCount++;
                }
            }

            // Count jobs written to channel
            int jobsQueued = 0;
            while (channel.Reader.TryRead(out _))
            {
                jobsQueued++;
            }

            // Exactly one apply should succeed, all others rejected
            return (successCount == 1 && rejectionCount == data.AttemptCount - 1 && jobsQueued == 1)
                .Label($"Expected 1 success and {data.AttemptCount - 1} rejections, got {successCount} successes and {rejectionCount} rejections, {jobsQueued} jobs queued");
        });
    }

    #endregion

    #region Property: Non-existent record apply is always rejected

    /// <summary>
    /// Property: For any non-existent TariffPercentageId, ApplyPercentageAsync
    /// always throws KeyNotFoundException.
    /// 
    /// **Validates: Requirements 5.5**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Applying_NonExistent_Record_Is_Always_Rejected()
    {
        var gen = Gen.Choose(10000, 99999);

        return Prop.ForAll(Arb.From(gen), nonExistentId =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"NonExistentApplyTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Act: attempt to apply a non-existent record
            KeyNotFoundException? caught = null;
            try
            {
                service.ApplyPercentageAsync(nonExistentId).GetAwaiter().GetResult();
            }
            catch (KeyNotFoundException ex)
            {
                caught = ex;
            }

            return (caught != null)
                .Label("Applying a non-existent record must always throw KeyNotFoundException");
        });
    }

    #endregion

    #region Property: Multiple records for same period cannot both be "Processing"

    /// <summary>
    /// Property: For any sequence of apply requests on multiple records sharing the same
    /// TariffPeriodName, at most one record can be in "Processing" state at any time.
    /// This ensures the period-level concurrency guard prevents duplicate propagation.
    /// 
    /// **Validates: Requirements 5.2, 5.6, 11.2**
    /// </summary>
    [Property(MaxTest = 30)]
    public Property At_Most_One_Record_Processing_Per_Period()
    {
        var gen = from year in Gen.Choose(2000, 2100)
                  from recordCount in Gen.Choose(2, 5)
                  from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  select new { Year = year, RecordCount = recordCount, Percentage = percentage };

        return Prop.ForAll(Arb.From(gen), data =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"OneProcessingPerPeriodTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed multiple "Pending" records for the same TariffPeriodName with non-overlapping dates
            var entities = new List<TariffPercentage>();
            for (int i = 0; i < data.RecordCount; i++)
            {
                var startDate = new DateOnly(data.Year, 1, 1).AddDays(i * 60);
                var endDate = startDate.AddDays(59);
                var entity = new TariffPercentage
                {
                    PercentageAdded = data.Percentage + i,
                    TariffPeriodName = data.Year,
                    StartActiveDate = startDate,
                    EndActiveDate = endDate,
                    Status = "Pending",
                    DateInserted = DateTime.UtcNow,
                    UserID = "test-user"
                };
                entities.Add(entity);
                context.Set<TariffPercentage>().Add(entity);
            }
            context.SaveChanges();

            // Try to apply all records sequentially
            int successCount = 0;
            int rejectionCount = 0;

            foreach (var entity in entities)
            {
                try
                {
                    service.ApplyPercentageAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
                    successCount++;
                }
                catch (InvalidOperationException)
                {
                    rejectionCount++;
                }
            }

            // Only the first apply should succeed; all others should be rejected
            // because after the first one, there's a "Processing" record for the same period
            var processingCount = context.Set<TariffPercentage>()
                .Count(r => r.TariffPeriodName == data.Year && r.Status == "Processing" && r.DateDeleted == null);

            return (successCount == 1 && processingCount == 1)
                .Label($"Expected exactly 1 processing record per period, got {processingCount} processing, {successCount} successes out of {data.RecordCount} attempts");
        });
    }

    #endregion
}
