using System.Threading.Channels;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Interfaces;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Services.Business;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace MedManage.Infrastructure.Tests.Services;

/// <summary>
/// Property-based tests for audit trail completeness in TariffPercentageService.
/// **Validates: Requirements 9.5**
///
/// Property 9: Audit Trail Completeness
/// For any create or update operation on a TariffPercentage record,
/// the authenticated user ID is recorded on the resulting record.
/// </summary>
public class TariffPercentageAuditTrailPropertyTests : IDisposable
{
    private readonly MedManageDbContext _context;

    public TariffPercentageAuditTrailPropertyTests()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"AuditTrailTests_{Guid.NewGuid()}")
            .Options;

        _context = new MedManageDbContext(options);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    #region Custom Generators

    /// <summary>
    /// Generates arbitrary authenticated user IDs (non-null, non-empty strings).
    /// </summary>
    private static Gen<string> AuthenticatedUserIdGen()
    {
        return Gen.OneOf(
            Gen.Elements(
                "user-001", "admin-42", "sys-admin-xyz",
                "john.doe@company.com", "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                "USER_123", "service-account-1", "auth0|abc123"
            ),
            Arb.Generate<NonEmptyString>().Select(s => s.Get)
        );
    }

    /// <summary>
    /// Generates valid CreateTariffPercentageDto instances for testing audit trail on creation.
    /// Each generated DTO has a unique TariffPeriodName + date range to avoid overlap conflicts.
    /// </summary>
    private static Gen<CreateTariffPercentageDto> ValidCreateDtoGen(int index)
    {
        return from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
               from year in Gen.Choose(2000, 2100)
               from dayOffset in Gen.Choose(0, 300)
               select new CreateTariffPercentageDto
               {
                   PercentageAdded = percentage,
                   TariffPeriodName = year,
                   StartActiveDate = new DateOnly(year, 1, 1).AddDays(dayOffset),
                   EndActiveDate = new DateOnly(year, 1, 1).AddDays(dayOffset + 30),
                   Notes = $"Audit trail test {index}"
               };
    }

    /// <summary>
    /// Generates valid UpdateTariffPercentageDto instances for testing audit trail on updates.
    /// </summary>
    private static Arbitrary<UpdateTariffPercentageDto> ValidUpdateDtoArbitrary()
    {
        var gen = from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
                  from hasNotes in Arb.Generate<bool>()
                  select new UpdateTariffPercentageDto
                  {
                      PercentageAdded = percentage,
                      StartActiveDate = null,
                      EndActiveDate = null,
                      Notes = hasNotes ? "Updated notes" : null
                  };

        return Arb.From(gen);
    }

    /// <summary>
    /// Generates a pair of (userId, CreateTariffPercentageDto) for create audit testing.
    /// </summary>
    private static Arbitrary<(string UserId, CreateTariffPercentageDto Dto)> CreateOperationArbitrary()
    {
        var gen = from userId in AuthenticatedUserIdGen()
                  from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
                  from year in Gen.Choose(2000, 2100)
                  from dayOffset in Gen.Choose(0, 300)
                  select (UserId: userId, Dto: new CreateTariffPercentageDto
                  {
                      PercentageAdded = percentage,
                      TariffPeriodName = year,
                      StartActiveDate = new DateOnly(year, 1, 1).AddDays(dayOffset),
                      EndActiveDate = new DateOnly(year, 1, 1).AddDays(dayOffset + 30),
                      Notes = "Audit test"
                  });

        return Arb.From(gen);
    }

    /// <summary>
    /// Generates a pair of (userId, UpdateTariffPercentageDto) for update audit testing.
    /// </summary>
    private static Arbitrary<(string UserId, UpdateTariffPercentageDto Dto)> UpdateOperationArbitrary()
    {
        var gen = from userId in AuthenticatedUserIdGen()
                  from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
                  select (UserId: userId, Dto: new UpdateTariffPercentageDto
                  {
                      PercentageAdded = percentage,
                      StartActiveDate = null,
                      EndActiveDate = null,
                      Notes = null
                  });

        return Arb.From(gen);
    }

    #endregion

    /// <summary>
    /// Property: For any create operation with an authenticated user,
    /// the authenticated user's ID is recorded on the resulting record's UserID field.
    /// **Validates: Requirements 9.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property CreateAsync_AlwaysRecordsAuthenticatedUserIdOnRecord()
    {
        return Prop.ForAll(CreateOperationArbitrary(), operation =>
        {
            // Arrange - use a unique in-memory DB per test iteration to avoid overlap conflicts
            var mockCurrentUser = new Mock<ICurrentUserService>();
            mockCurrentUser.Setup(x => x.UserId).Returns(operation.UserId);
            mockCurrentUser.Setup(x => x.IsAuthenticated).Returns(true);

            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"AuditCreate_{Guid.NewGuid()}")
                .Options;

            // Pass currentUserService to DbContext so SetAuditFields picks up the correct user
            using var context = new MedManageDbContext(options, mockCurrentUser.Object);

            var channel = Channel.CreateUnbounded<TariffUpdateJob>();
            var service = new TariffPercentageService(context, mockCurrentUser.Object, channel.Writer);

            // Act
            var result = service.CreateAsync(operation.Dto).GetAwaiter().GetResult();

            // Assert - the returned DTO has the user ID
            result.UserID.Should().Be(operation.UserId,
                "the authenticated user's ID should be recorded on created records");

            // Also verify at the entity level
            var entity = context.TariffPercentages.First(e => e.TariffPercentageId == result.TariffPercentageId);
            entity.UserID.Should().Be(operation.UserId,
                "the authenticated user's ID should be persisted on the entity");
        });
    }

    /// <summary>
    /// Property: For any update operation with an authenticated user,
    /// the authenticated user's ID is recorded on the resulting record's UpdatedUserID field.
    /// **Validates: Requirements 9.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property UpdateAsync_AlwaysRecordsAuthenticatedUserIdOnRecord()
    {
        return Prop.ForAll(UpdateOperationArbitrary(), operation =>
        {
            // Arrange - set up the mock user service first (shared by DbContext and Service)
            var mockCurrentUser = new Mock<ICurrentUserService>();
            mockCurrentUser.Setup(x => x.UserId).Returns("original-creator");
            mockCurrentUser.Setup(x => x.IsAuthenticated).Returns(true);

            var options = new DbContextOptionsBuilder<MedManageDbContext>()
                .UseInMemoryDatabase(databaseName: $"AuditUpdate_{Guid.NewGuid()}")
                .Options;

            // Pass currentUserService to DbContext so SetAuditFields picks up the correct user
            using var context = new MedManageDbContext(options, mockCurrentUser.Object);

            // Seed a record created by a different user
            var entity = new Core.Entities.TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = "Pending",
                DateInserted = DateTime.UtcNow,
                UserID = "original-creator"
            };
            context.TariffPercentages.Add(entity);
            context.SaveChanges();

            // Now switch to the updating user's ID
            mockCurrentUser.Setup(x => x.UserId).Returns(operation.UserId);

            var channel = Channel.CreateUnbounded<TariffUpdateJob>();
            var service = new TariffPercentageService(context, mockCurrentUser.Object, channel.Writer);

            // Act
            service.UpdateAsync(entity.TariffPercentageId, operation.Dto).GetAwaiter().GetResult();

            // Assert - verify at the entity level
            context.Entry(entity).Reload();
            entity.UpdatedUserID.Should().Be(operation.UserId,
                "the authenticated user's ID should be recorded as UpdatedUserID on updated records");
            entity.DateUpdated.Should().NotBeNull(
                "DateUpdated should be set when a record is updated");
        });
    }
}
