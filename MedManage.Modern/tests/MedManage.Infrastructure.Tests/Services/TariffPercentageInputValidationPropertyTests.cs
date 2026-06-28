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
/// Property-based tests for input validation rejection in TariffPercentageService.
/// **Validates: Requirements 1.2, 1.3, 1.4, 3.3**
/// 
/// Property 7: Input Validation Rejection
/// For any CreateTariffPercentageDto or UpdateTariffPercentageDto with invalid values
/// (PercentageAdded less than 0.0001 or greater than 9999.9999, TariffPeriodName outside 2000-2100,
/// EndActiveDate before StartActiveDate), the service rejects the request with a validation error
/// and no record is created or modified.
/// </summary>
public class TariffPercentageInputValidationPropertyTests : IDisposable
{
    private readonly MedManageDbContext _context;
    private readonly TariffPercentageService _service;

    public TariffPercentageInputValidationPropertyTests()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"InputValidationTests_{Guid.NewGuid()}")
            .Options;

        _context = new MedManageDbContext(options);

        var mockCurrentUser = new Mock<ICurrentUserService>();
        mockCurrentUser.Setup(x => x.UserId).Returns("test-user");
        mockCurrentUser.Setup(x => x.IsAuthenticated).Returns(true);

        var channel = Channel.CreateUnbounded<TariffUpdateJob>();

        _service = new TariffPercentageService(_context, mockCurrentUser.Object, channel.Writer);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    #region Custom Generators

    /// <summary>
    /// Generates CreateTariffPercentageDto with an invalid PercentageAdded (less than 0.0001 or greater than 9999.9999).
    /// </summary>
    private static Arbitrary<CreateTariffPercentageDto> InvalidPercentageCreateDtoArbitrary()
    {
        var invalidPercentageGen = Gen.OneOf(
            // Values <= 0
            Gen.Choose(-100000, 0).Select(v => (decimal)v / 10000m),
            // Values > 9999.9999
            Gen.Choose(100000000, 200000000).Select(v => (decimal)v / 10000m)
        );

        var dtoGen = from percentage in invalidPercentageGen
                     from year in Gen.Choose(2000, 2100)
                     from dayOffset in Gen.Choose(0, 365)
                     select new CreateTariffPercentageDto
                     {
                         PercentageAdded = percentage,
                         TariffPeriodName = year,
                         StartActiveDate = new DateOnly(year, 1, 1).AddDays(dayOffset),
                         EndActiveDate = null,
                         Notes = "Test"
                     };

        return Arb.From(dtoGen);
    }

    /// <summary>
    /// Generates CreateTariffPercentageDto with an invalid TariffPeriodName (outside 2000-2100).
    /// </summary>
    private static Arbitrary<CreateTariffPercentageDto> InvalidPeriodNameCreateDtoArbitrary()
    {
        var invalidYearGen = Gen.OneOf(
            Gen.Choose(1800, 1999),
            Gen.Choose(2101, 2500)
        );

        var dtoGen = from year in invalidYearGen
                     from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
                     select new CreateTariffPercentageDto
                     {
                         PercentageAdded = percentage,
                         TariffPeriodName = year,
                         StartActiveDate = new DateOnly(2025, 1, 1),
                         EndActiveDate = null,
                         Notes = "Test"
                     };

        return Arb.From(dtoGen);
    }

    /// <summary>
    /// Generates CreateTariffPercentageDto with EndActiveDate before StartActiveDate.
    /// </summary>
    private static Arbitrary<CreateTariffPercentageDto> InvalidDateRangeCreateDtoArbitrary()
    {
        var dtoGen = from year in Gen.Choose(2000, 2100)
                     from startDayOffset in Gen.Choose(30, 365)
                     from endDaysBefore in Gen.Choose(1, 30)
                     from percentage in Gen.Choose(1, 99999999).Select(v => (decimal)v / 10000m)
                     let startDate = new DateOnly(year, 1, 1).AddDays(startDayOffset)
                     let endDate = startDate.AddDays(-endDaysBefore)
                     select new CreateTariffPercentageDto
                     {
                         PercentageAdded = percentage,
                         TariffPeriodName = year,
                         StartActiveDate = startDate,
                         EndActiveDate = endDate,
                         Notes = "Test"
                     };

        return Arb.From(dtoGen);
    }

    /// <summary>
    /// Generates UpdateTariffPercentageDto with an invalid PercentageAdded.
    /// </summary>
    private static Arbitrary<UpdateTariffPercentageDto> InvalidPercentageUpdateDtoArbitrary()
    {
        var invalidPercentageGen = Gen.OneOf(
            // Values <= 0
            Gen.Choose(-100000, 0).Select(v => (decimal)v / 10000m),
            // Values > 9999.9999
            Gen.Choose(100000000, 200000000).Select(v => (decimal)v / 10000m)
        );

        var dtoGen = from percentage in invalidPercentageGen
                     select new UpdateTariffPercentageDto
                     {
                         PercentageAdded = percentage,
                         StartActiveDate = null,
                         EndActiveDate = null,
                         Notes = null
                     };

        return Arb.From(dtoGen);
    }

    /// <summary>
    /// Generates UpdateTariffPercentageDto with EndActiveDate before StartActiveDate.
    /// </summary>
    private static Arbitrary<UpdateTariffPercentageDto> InvalidDateRangeUpdateDtoArbitrary()
    {
        var dtoGen = from startDayOffset in Gen.Choose(30, 365)
                     from endDaysBefore in Gen.Choose(1, 30)
                     let startDate = new DateOnly(2025, 1, 1).AddDays(startDayOffset)
                     let endDate = startDate.AddDays(-endDaysBefore)
                     select new UpdateTariffPercentageDto
                     {
                         PercentageAdded = null,
                         StartActiveDate = startDate,
                         EndActiveDate = endDate,
                         Notes = null
                     };

        return Arb.From(dtoGen);
    }

    #endregion

    #region CreateAsync Validation Tests

    /// <summary>
    /// Property: For any CreateTariffPercentageDto with PercentageAdded less than 0.0001 or greater than 9999.9999,
    /// CreateAsync throws ArgumentException and no record is persisted.
    /// **Validates: Requirements 1.2**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property CreateAsync_RejectsInvalidPercentage()
    {
        return Prop.ForAll(InvalidPercentageCreateDtoArbitrary(), dto =>
        {
            // Act & Assert
            var act = async () => await _service.CreateAsync(dto);
            act.Should().ThrowAsync<ArgumentException>().Wait();

            // Verify no record was persisted
            var count = _context.TariffPercentages.Count();
            count.Should().Be(0, "no record should be persisted when validation fails");
        });
    }

    /// <summary>
    /// Property: For any CreateTariffPercentageDto with TariffPeriodName outside 2000-2100,
    /// CreateAsync throws ArgumentException and no record is persisted.
    /// **Validates: Requirements 1.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property CreateAsync_RejectsInvalidPeriodName()
    {
        return Prop.ForAll(InvalidPeriodNameCreateDtoArbitrary(), dto =>
        {
            // Act & Assert
            var act = async () => await _service.CreateAsync(dto);
            act.Should().ThrowAsync<ArgumentException>().Wait();

            // Verify no record was persisted
            var count = _context.TariffPercentages.Count();
            count.Should().Be(0, "no record should be persisted when validation fails");
        });
    }

    /// <summary>
    /// Property: For any CreateTariffPercentageDto with EndActiveDate before StartActiveDate,
    /// CreateAsync throws ArgumentException and no record is persisted.
    /// **Validates: Requirements 1.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property CreateAsync_RejectsEndDateBeforeStartDate()
    {
        return Prop.ForAll(InvalidDateRangeCreateDtoArbitrary(), dto =>
        {
            // Act & Assert
            var act = async () => await _service.CreateAsync(dto);
            act.Should().ThrowAsync<ArgumentException>().Wait();

            // Verify no record was persisted
            var count = _context.TariffPercentages.Count();
            count.Should().Be(0, "no record should be persisted when validation fails");
        });
    }

    #endregion

    #region UpdateAsync Validation Tests

    /// <summary>
    /// Property: For any UpdateTariffPercentageDto with PercentageAdded less than 0.0001 or greater than 9999.9999,
    /// UpdateAsync throws ArgumentException and the existing record is not modified.
    /// **Validates: Requirements 3.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property UpdateAsync_RejectsInvalidPercentage()
    {
        return Prop.ForAll(InvalidPercentageUpdateDtoArbitrary(), dto =>
        {
            // Arrange - create a valid record to update
            var entity = new Core.Entities.TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = null,
                Status = "Pending",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            _context.TariffPercentages.Add(entity);
            _context.SaveChanges();
            var originalPercentage = entity.PercentageAdded;

            try
            {
                // Act & Assert
                var act = async () => await _service.UpdateAsync(entity.TariffPercentageId, dto);
                act.Should().ThrowAsync<ArgumentException>().Wait();

                // Verify record was not modified
                _context.Entry(entity).Reload();
                entity.PercentageAdded.Should().Be(originalPercentage, "record should not be modified when validation fails");
            }
            finally
            {
                // Cleanup
                _context.TariffPercentages.Remove(entity);
                _context.SaveChanges();
            }
        });
    }

    /// <summary>
    /// Property: For any UpdateTariffPercentageDto with EndActiveDate before StartActiveDate,
    /// UpdateAsync throws ArgumentException and the existing record is not modified.
    /// **Validates: Requirements 3.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public Property UpdateAsync_RejectsEndDateBeforeStartDate()
    {
        return Prop.ForAll(InvalidDateRangeUpdateDtoArbitrary(), dto =>
        {
            // Arrange - create a valid record to update
            var entity = new Core.Entities.TariffPercentage
            {
                PercentageAdded = 100.0000m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = null,
                Status = "Pending",
                DateInserted = DateTime.UtcNow,
                UserID = "test-user"
            };
            _context.TariffPercentages.Add(entity);
            _context.SaveChanges();
            var originalStartDate = entity.StartActiveDate;

            try
            {
                // Act & Assert
                var act = async () => await _service.UpdateAsync(entity.TariffPercentageId, dto);
                act.Should().ThrowAsync<ArgumentException>().Wait();

                // Verify record was not modified
                _context.Entry(entity).Reload();
                entity.StartActiveDate.Should().Be(originalStartDate, "record should not be modified when validation fails");
            }
            finally
            {
                // Cleanup
                _context.TariffPercentages.Remove(entity);
                _context.SaveChanges();
            }
        });
    }

    #endregion
}
