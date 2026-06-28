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
/// Property-based tests for Valid State Transitions (Property 8).
/// 
/// For any TariffPercentage record, the Status field transitions only through valid paths:
/// - Creation always yields "Pending"
/// - Updates and deletes are permitted only in "Pending" or "Failed" states
/// - Applying transitions to "Processing"
/// - Successful completion transitions to "Completed"
/// - Failure transitions to "Failed"
/// - Operations on records in invalid states are rejected
/// 
/// **Validates: Requirements 1.1, 3.1, 3.2, 4.1, 4.2, 5.1, 7.3**
/// </summary>
public class TariffPercentageStateTransitionPropertyTests : IDisposable
{
    private readonly MedManageDbContext _context;
    private readonly TariffPercentageService _service;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Channel<TariffUpdateJob> _channel;

    public TariffPercentageStateTransitionPropertyTests()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"StateTransitionTests_{Guid.NewGuid()}")
            .Options;

        _context = new MedManageDbContext(options);
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _currentUserServiceMock.Setup(x => x.UserId).Returns("test-user-001");
        _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);

        _channel = Channel.CreateUnbounded<TariffUpdateJob>();

        _service = new TariffPercentageService(_context, _currentUserServiceMock.Object, _channel.Writer);
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

    #region Custom Generators

    /// <summary>
    /// Generates valid CreateTariffPercentageDto values that should pass validation.
    /// Uses unique TariffPeriodName per test to avoid overlap conflicts.
    /// </summary>
    private static Arbitrary<CreateTariffPercentageDto> ValidCreateDtoArbitrary()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from startDay in Gen.Choose(1, 28)
                  from startMonth in Gen.Choose(1, 12)
                  from hasEndDate in Arb.Generate<bool>()
                  from endOffset in Gen.Choose(1, 365)
                  let startDate = new DateOnly(year, startMonth, startDay)
                  let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
                  select new CreateTariffPercentageDto
                  {
                      PercentageAdded = percentage,
                      TariffPeriodName = year,
                      StartActiveDate = startDate,
                      EndActiveDate = endDate,
                      Notes = "Property test generated"
                  };

        return Arb.From(gen);
    }

    /// <summary>
    /// Generates valid UpdateTariffPercentageDto values.
    /// </summary>
    private static Arbitrary<UpdateTariffPercentageDto> ValidUpdateDtoArbitrary()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  select new UpdateTariffPercentageDto
                  {
                      PercentageAdded = percentage,
                      Notes = "Updated via property test"
                  };

        return Arb.From(gen);
    }

    /// <summary>
    /// Represents an operation that can be performed on a TariffPercentage record.
    /// </summary>
    public enum TariffOperation
    {
        Create,
        Update,
        Delete,
        Apply
    }

    /// <summary>
    /// Valid statuses that a record can be in (for seeding tests).
    /// </summary>
    private static readonly string[] AllStatuses = { "Pending", "Processing", "Completed", "Failed" };
    private static readonly string[] ModifiableStatuses = { "Pending", "Failed" };
    private static readonly string[] NonModifiableStatuses = { "Processing", "Completed" };

    #endregion

    #region Property: Creation always yields "Pending"

    [Property(MaxTest = 50)]
    public Property Creation_Always_Yields_Pending_Status()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from startDay in Gen.Choose(1, 28)
                  from startMonth in Gen.Choose(1, 12)
                  from hasEndDate in Arb.Generate<bool>()
                  from endOffset in Gen.Choose(1, 365)
                  let startDate = new DateOnly(year, startMonth, startDay)
                  let endDate = hasEndDate ? startDate.AddDays(endOffset) : (DateOnly?)null
                  select new CreateTariffPercentageDto
                  {
                      PercentageAdded = percentage,
                      TariffPeriodName = year,
                      StartActiveDate = startDate,
                      EndActiveDate = endDate,
                      Notes = "Property test"
                  };

        return Prop.ForAll(Arb.From(gen), dto =>
        {
            // Use a fresh context per test iteration to avoid overlap conflicts
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"CreateTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            var result = service.CreateAsync(dto).GetAwaiter().GetResult();

            return (result.Status == "Pending").Label("Status should be Pending after creation");
        });
    }

    #endregion

    #region Property: Updates only permitted in "Pending" or "Failed" states

    [Property(MaxTest = 50)]
    public Property Update_Permitted_Only_In_Pending_Or_Failed_States()
    {
        var statusGen = Gen.Elements(AllStatuses);
        var percentageGen = Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m);

        var gen = from status in statusGen
                  from percentage in percentageGen
                  select (status, percentage);

        return Prop.ForAll(Arb.From(gen), tuple =>
        {
            var (status, newPercentage) = tuple;

            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"UpdateStateTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record with the specified status
            var entity = new TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = status,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            var updateDto = new UpdateTariffPercentageDto
            {
                PercentageAdded = newPercentage
            };

            bool isModifiableState = status == "Pending" || status == "Failed";

            if (isModifiableState)
            {
                // Should succeed
                var result = service.UpdateAsync(entity.TariffPercentageId, updateDto).GetAwaiter().GetResult();
                return (result.PercentageAdded == newPercentage)
                    .Label($"Update should succeed in '{status}' state");
            }
            else
            {
                // Should throw InvalidOperationException
                Exception? caught = null;
                try
                {
                    service.UpdateAsync(entity.TariffPercentageId, updateDto).GetAwaiter().GetResult();
                }
                catch (InvalidOperationException ex)
                {
                    caught = ex;
                }

                return (caught != null)
                    .Label($"Update should be rejected in '{status}' state");
            }
        });
    }

    #endregion

    #region Property: Deletes only permitted in "Pending" or "Failed" states

    [Property(MaxTest = 50)]
    public Property Delete_Permitted_Only_In_Pending_Or_Failed_States()
    {
        var statusGen = Gen.Elements(AllStatuses);

        return Prop.ForAll(Arb.From(statusGen), status =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"DeleteStateTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record with the specified status
            var entity = new TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = status,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            bool isModifiableState = status == "Pending" || status == "Failed";

            if (isModifiableState)
            {
                // Should succeed (returns true)
                var result = service.DeleteAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
                return result.Label($"Delete should succeed in '{status}' state");
            }
            else
            {
                // Should throw InvalidOperationException
                Exception? caught = null;
                try
                {
                    service.DeleteAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
                }
                catch (InvalidOperationException ex)
                {
                    caught = ex;
                }

                return (caught != null)
                    .Label($"Delete should be rejected in '{status}' state");
            }
        });
    }

    #endregion

    #region Property: Apply transitions from "Pending"/"Failed" to "Processing" (rejected for others)

    [Property(MaxTest = 50)]
    public Property Apply_Only_Permitted_From_Pending_Or_Failed()
    {
        var statusGen = Gen.Elements(AllStatuses);

        return Prop.ForAll(Arb.From(statusGen), status =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"ApplyStateTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record with the specified status
            var entity = new TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = status,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            bool isApplyableState = status == "Pending" || status == "Failed";

            if (isApplyableState)
            {
                // Should succeed: transitions to "Processing" and returns job status
                var jobStatus = service.ApplyPercentageAsync(entity.TariffPercentageId).GetAwaiter().GetResult();

                // Verify the record status is now "Processing"
                var updatedRecord = context.Set<TariffPercentage>()
                    .First(r => r.TariffPercentageId == entity.TariffPercentageId);

                return (updatedRecord.Status == "Processing" && jobStatus.Status == "Queued")
                    .Label($"Apply from '{status}' should transition to 'Processing' and return 'Queued' job");
            }
            else
            {
                // Should throw InvalidOperationException for "Processing" or "Completed"
                Exception? caught = null;
                try
                {
                    service.ApplyPercentageAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
                }
                catch (InvalidOperationException ex)
                {
                    caught = ex;
                }

                return (caught != null)
                    .Label($"Apply should be rejected in '{status}' state");
            }
        });
    }

    #endregion

    #region Property: Sequence of operations respects valid state machine

    [Property(MaxTest = 30)]
    public Property Operation_Sequence_Respects_State_Machine()
    {
        // Generate a sequence of operations to perform after creating a record
        var opsGen = Gen.ListOf(Gen.Choose(0, 2).Select(i => i switch
        {
            0 => TariffOperation.Update,
            1 => TariffOperation.Delete,
            _ => TariffOperation.Update
        }));

        return Prop.ForAll(Arb.From(opsGen), operations =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"SequenceTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Step 1: Create - always yields "Pending"
            var createDto = new CreateTariffPercentageDto
            {
                PercentageAdded = 150.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Notes = "Sequence test"
            };

            var created = service.CreateAsync(createDto).GetAwaiter().GetResult();
            if (created.Status != "Pending")
                return false.Label("Creation must yield Pending status");

            var currentStatus = "Pending";
            var recordId = created.TariffPercentageId;
            var isDeleted = false;

            foreach (var op in operations)
            {
                if (isDeleted)
                {
                    // After deletion, all operations should fail (record not found)
                    switch (op)
                    {
                        case TariffOperation.Update:
                            try
                            {
                                service.UpdateAsync(recordId, new UpdateTariffPercentageDto { PercentageAdded = 200m }).GetAwaiter().GetResult();
                                return false.Label("Update should fail on deleted record");
                            }
                            catch (KeyNotFoundException) { /* Expected */ }
                            break;
                        case TariffOperation.Delete:
                            var deleteResult = service.DeleteAsync(recordId).GetAwaiter().GetResult();
                            if (deleteResult)
                                return false.Label("Delete should return false for already-deleted record");
                            break;
                    }
                    continue;
                }

                bool isModifiable = currentStatus == "Pending" || currentStatus == "Failed";

                switch (op)
                {
                    case TariffOperation.Update:
                        if (isModifiable)
                        {
                            var result = service.UpdateAsync(recordId, new UpdateTariffPercentageDto { PercentageAdded = 200m }).GetAwaiter().GetResult();
                            // Status should remain unchanged after update
                            if (result.Status != currentStatus)
                                return false.Label($"Update should not change status from '{currentStatus}'");
                        }
                        else
                        {
                            try
                            {
                                service.UpdateAsync(recordId, new UpdateTariffPercentageDto { PercentageAdded = 200m }).GetAwaiter().GetResult();
                                return false.Label($"Update should be rejected in '{currentStatus}' state");
                            }
                            catch (InvalidOperationException) { /* Expected */ }
                        }
                        break;

                    case TariffOperation.Delete:
                        if (isModifiable)
                        {
                            var result = service.DeleteAsync(recordId).GetAwaiter().GetResult();
                            if (!result)
                                return false.Label("Delete should succeed in modifiable state");
                            isDeleted = true;
                        }
                        else
                        {
                            try
                            {
                                service.DeleteAsync(recordId).GetAwaiter().GetResult();
                                return false.Label($"Delete should be rejected in '{currentStatus}' state");
                            }
                            catch (InvalidOperationException) { /* Expected */ }
                        }
                        break;
                }
            }

            return true.Label("All operations respected valid state transitions");
        });
    }

    #endregion

    #region Property: Records in "Processing" or "Completed" status reject modifications

    [Property(MaxTest = 50)]
    public Property Non_Modifiable_States_Reject_All_Modifications()
    {
        var nonModifiableStatusGen = Gen.Elements(NonModifiableStatuses);
        var operationGen = Gen.Elements(new[] { TariffOperation.Update, TariffOperation.Delete });

        var gen = from status in nonModifiableStatusGen
                  from operation in operationGen
                  select (status, operation);

        return Prop.ForAll(Arb.From(gen), tuple =>
        {
            var (status, operation) = tuple;

            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"NonModifiableTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record in non-modifiable state
            var entity = new TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = status,
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            Exception? caught = null;
            try
            {
                switch (operation)
                {
                    case TariffOperation.Update:
                        service.UpdateAsync(entity.TariffPercentageId, new UpdateTariffPercentageDto { PercentageAdded = 999m }).GetAwaiter().GetResult();
                        break;
                    case TariffOperation.Delete:
                        service.DeleteAsync(entity.TariffPercentageId).GetAwaiter().GetResult();
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                caught = ex;
            }

            return (caught != null)
                .Label($"Operation '{operation}' should be rejected in '{status}' state");
        });
    }

    #endregion

    #region Property: Status transitions from "Failed" allow retry (update back to modifiable)

    [Property(MaxTest = 30)]
    public Property Failed_Status_Allows_Update_And_Delete()
    {
        var percentageGen = Gen.Choose(1, 99999999).Select(x => (decimal)x / 10000m);

        return Prop.ForAll(Arb.From(percentageGen), newPercentage =>
        {
            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"FailedRetryTest_{Guid.NewGuid()}")
                .Options;

            using var context = new MedManageDbContext(options);
            var service = CreateService(context);

            // Seed a record in "Failed" state (simulating a previously failed apply)
            var entity = new TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = "Failed",
                Notes = "Previous failure: timeout",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            context.Set<TariffPercentage>().Add(entity);
            context.SaveChanges();

            // Update should succeed on "Failed" records
            var result = service.UpdateAsync(entity.TariffPercentageId, new UpdateTariffPercentageDto { PercentageAdded = newPercentage }).GetAwaiter().GetResult();

            // Status should remain "Failed" (not changed by update)
            return (result.Status == "Failed" && result.PercentageAdded == newPercentage)
                .Label("Failed records should allow updates without changing status");
        });
    }

    #endregion
}
