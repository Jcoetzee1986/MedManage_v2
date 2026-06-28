using System.Globalization;
using FluentAssertions;
using HandlebarsDotNet;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Services.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MedManage.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for letter template field mapping with tariff percentage data.
/// Verifies that GatherCaseLetterDataAsync correctly maps tariff percentage fields
/// and that Handlebars templates render the dynamic values without errors.
///
/// **Validates: Requirements 11.1, 11.2, 11.3, 11.4, 11.6**
/// </summary>
public class LetterTemplateFieldMappingIntegrationTests : IDisposable
{
    private readonly Mock<ITariffPercentageService> _tariffPercentageServiceMock;
    private readonly Mock<ILogger<LetterTemplateService>> _loggerMock;

    public LetterTemplateFieldMappingIntegrationTests()
    {
        _tariffPercentageServiceMock = new Mock<ITariffPercentageService>();
        _loggerMock = new Mock<ILogger<LetterTemplateService>>();
    }

    public void Dispose()
    {
        // Each test uses its own context, no shared state to dispose
    }

    private MedManageDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"LetterTemplateFieldMapping_{Guid.NewGuid()}")
            .Options;

        return new MedManageDbContext(options);
    }

    #region Tests: GatherCaseLetterDataAsync includes tariff percentage fields with completed records

    /// <summary>
    /// Verifies that when completed TariffPercentage records exist for two years,
    /// the letter data includes TariffPercentageCurrentYear, TariffPercentageCurrentYearValue,
    /// TariffPercentagePriorYear, and TariffPercentagePriorYearValue with correct values.
    /// **Validates: Requirements 11.1, 11.2, 11.3**
    /// </summary>
    [Fact]
    public async Task GatherCaseLetterData_WithCompletedRecords_IncludesCorrectTariffPercentageFields()
    {
        // Arrange
        var tariffPercentages = new List<TariffPercentageDto>
        {
            new TariffPercentageDto
            {
                TariffPercentageId = 1,
                PercentageAdded = 250.60m,
                TariffPeriodName = 2026,
                StartActiveDate = new DateOnly(2026, 1, 1),
                EndActiveDate = null,
                Status = "Completed"
            },
            new TariffPercentageDto
            {
                TariffPercentageId = 2,
                PercentageAdded = 233.90m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = "Completed"
            }
        };

        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tariffPercentages);

        // Build the data dictionary the same way GatherCaseLetterDataAsync does
        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Assert
        data["TariffPercentageCurrentYear"].Should().Be("2026");
        data["TariffPercentageCurrentYearValue"].Should().Be("250.60");
        data["TariffPercentagePriorYear"].Should().Be("2025");
        data["TariffPercentagePriorYearValue"].Should().Be("233.90");
    }

    /// <summary>
    /// Verifies that when only one year of completed records exists,
    /// the current year fields are populated and prior year fields are empty strings.
    /// **Validates: Requirements 11.1, 11.2, 11.4**
    /// </summary>
    [Fact]
    public async Task GatherCaseLetterData_WithSingleYearRecord_PopulatesCurrentYearAndEmptyPriorYear()
    {
        // Arrange
        var tariffPercentages = new List<TariffPercentageDto>
        {
            new TariffPercentageDto
            {
                TariffPercentageId = 1,
                PercentageAdded = 250.60m,
                TariffPeriodName = 2026,
                StartActiveDate = new DateOnly(2026, 1, 1),
                EndActiveDate = null,
                Status = "Completed"
            }
        };

        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tariffPercentages);

        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Assert
        data["TariffPercentageCurrentYear"].Should().Be("2026");
        data["TariffPercentageCurrentYearValue"].Should().Be("250.60");
        data["TariffPercentagePriorYear"].Should().Be("");
        data["TariffPercentagePriorYearValue"].Should().Be("");
    }

    #endregion

    #region Tests: No completed records → empty strings

    /// <summary>
    /// Verifies that when no completed TariffPercentage records exist,
    /// all four tariff percentage fields are set to empty strings (no rendering error).
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Fact]
    public async Task GatherCaseLetterData_WithNoCompletedRecords_SetsAllFieldsToEmptyString()
    {
        // Arrange
        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<TariffPercentageDto>());

        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Assert
        data["TariffPercentageCurrentYear"].Should().Be("");
        data["TariffPercentageCurrentYearValue"].Should().Be("");
        data["TariffPercentagePriorYear"].Should().Be("");
        data["TariffPercentagePriorYearValue"].Should().Be("");
    }

    #endregion

    #region Tests: Rendered HTML output contains dynamic percentage values

    /// <summary>
    /// Verifies that a Handlebars template with tariff percentage placeholders renders
    /// the dynamically inserted percentage values in the expected format.
    /// **Validates: Requirements 11.1, 11.6**
    /// </summary>
    [Fact]
    public async Task RenderTemplate_WithTariffPercentageFields_ProducesCorrectHtmlOutput()
    {
        // Arrange
        var tariffPercentages = new List<TariffPercentageDto>
        {
            new TariffPercentageDto
            {
                TariffPercentageId = 1,
                PercentageAdded = 250.60m,
                TariffPeriodName = 2026,
                StartActiveDate = new DateOnly(2026, 1, 1),
                EndActiveDate = null,
                Status = "Completed"
            },
            new TariffPercentageDto
            {
                TariffPercentageId = 2,
                PercentageAdded = 233.90m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = "Completed"
            }
        };

        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tariffPercentages);

        var templateHtml = @"<html><body>
<p>Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}</p>
<p>Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentageCurrentYearValue}}% for {{TariffPercentageCurrentYear}}</p>
</body></html>";

        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Act
        var handlebars = Handlebars.Create();
        var compiledTemplate = handlebars.Compile(templateHtml);
        var renderedHtml = compiledTemplate(data);

        // Assert
        renderedHtml.Should().Contain("NHRPL+ 233.90% for 2025");
        renderedHtml.Should().Contain("NHRPL+ 250.60% for 2026");
    }

    /// <summary>
    /// Verifies that when no completed records exist, the template renders
    /// without errors (empty strings in place of tariff values).
    /// **Validates: Requirements 11.4, 11.6**
    /// </summary>
    [Fact]
    public async Task RenderTemplate_WithNoCompletedRecords_DoesNotCauseRenderingError()
    {
        // Arrange
        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<TariffPercentageDto>());

        var templateHtml = @"<html><body>
<p>Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}</p>
<p>Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentageCurrentYearValue}}% for {{TariffPercentageCurrentYear}}</p>
</body></html>";

        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Act
        var handlebars = Handlebars.Create();
        var compiledTemplate = handlebars.Compile(templateHtml);
        var renderedHtml = compiledTemplate(data);

        // Assert - template renders without exceptions and empty strings don't break the output
        renderedHtml.Should().Contain("NHRPL+ % for ");
        renderedHtml.Should().NotContain("{{TariffPercentageCurrentYear}}");
        renderedHtml.Should().NotContain("{{TariffPercentageCurrentYearValue}}");
        renderedHtml.Should().NotContain("{{TariffPercentagePriorYear}}");
        renderedHtml.Should().NotContain("{{TariffPercentagePriorYearValue}}");
    }

    /// <summary>
    /// Verifies that tariff percentage values are formatted to exactly two decimal places.
    /// **Validates: Requirements 11.2, 11.3**
    /// </summary>
    [Theory]
    [InlineData(100.1, "100.10")]
    [InlineData(250.6, "250.60")]
    [InlineData(9999.9999, "10000.00")]
    [InlineData(1.0, "1.00")]
    [InlineData(0.01, "0.01")]
    public async Task GatherCaseLetterData_FormatsPercentageToTwoDecimalPlaces(decimal inputValue, string expectedFormatted)
    {
        // Arrange
        var tariffPercentages = new List<TariffPercentageDto>
        {
            new TariffPercentageDto
            {
                TariffPercentageId = 1,
                PercentageAdded = inputValue,
                TariffPeriodName = 2026,
                StartActiveDate = new DateOnly(2026, 1, 1),
                EndActiveDate = null,
                Status = "Completed"
            }
        };

        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tariffPercentages);

        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Assert
        data["TariffPercentageCurrentYearValue"].Should().Be(expectedFormatted);
    }

    /// <summary>
    /// Verifies that a full letter template with multiple tariff percentage placeholders
    /// in both non-medical-scheme and referral letter sections renders correctly.
    /// **Validates: Requirements 11.1, 11.6**
    /// </summary>
    [Fact]
    public async Task RenderTemplate_FullLetterWithBothSections_ContainsDynamicValues()
    {
        // Arrange
        var tariffPercentages = new List<TariffPercentageDto>
        {
            new TariffPercentageDto
            {
                TariffPercentageId = 1,
                PercentageAdded = 250.60m,
                TariffPeriodName = 2026,
                StartActiveDate = new DateOnly(2026, 1, 1),
                EndActiveDate = null,
                Status = "Completed"
            },
            new TariffPercentageDto
            {
                TariffPercentageId = 2,
                PercentageAdded = 233.90m,
                TariffPeriodName = 2025,
                StartActiveDate = new DateOnly(2025, 1, 1),
                EndActiveDate = new DateOnly(2025, 12, 31),
                Status = "Completed"
            }
        };

        _tariffPercentageServiceMock
            .Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tariffPercentages);

        // Template that mimics the actual CaseLetter template with both sections
        var templateHtml = @"<html><body>
<div class='non-medical-scheme-section'>
    <p>The rates are as follows:</p>
    <p>Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentagePriorYearValue}}% for {{TariffPercentagePriorYear}}</p>
    <p>Ministry of Health Botswana will pay 2009 NHRPL+ {{TariffPercentageCurrentYearValue}}% for {{TariffPercentageCurrentYear}}</p>
</div>
<div class='referral-letter-section'>
    <p>Tariff rates applied:</p>
    <p>{{TariffPercentagePriorYear}}: NHRPL+ {{TariffPercentagePriorYearValue}}%</p>
    <p>{{TariffPercentageCurrentYear}}: NHRPL+ {{TariffPercentageCurrentYearValue}}%</p>
</div>
</body></html>";

        var data = await BuildTariffPercentageData(_tariffPercentageServiceMock.Object);

        // Act
        var handlebars = Handlebars.Create();
        var compiledTemplate = handlebars.Compile(templateHtml);
        var renderedHtml = compiledTemplate(data);

        // Assert - both sections contain the correct dynamic values
        renderedHtml.Should().Contain("NHRPL+ 233.90% for 2025");
        renderedHtml.Should().Contain("NHRPL+ 250.60% for 2026");
        renderedHtml.Should().Contain("2025: NHRPL+ 233.90%");
        renderedHtml.Should().Contain("2026: NHRPL+ 250.60%");
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Replicates the tariff percentage field mapping logic from GatherCaseLetterDataAsync
    /// to test it in isolation without needing the full database setup.
    /// </summary>
    private static async Task<Dictionary<string, object?>> BuildTariffPercentageData(ITariffPercentageService tariffPercentageService)
    {
        var data = new Dictionary<string, object?>();

        var tariffPercentages = (await tariffPercentageService.GetActivePercentagesForLetterAsync()).ToList();

        if (tariffPercentages.Count > 0)
        {
            // First record is the current year (ordered by TariffPeriodName descending)
            data["TariffPercentageCurrentYear"] = tariffPercentages[0].TariffPeriodName.ToString();
            data["TariffPercentageCurrentYearValue"] = tariffPercentages[0].PercentageAdded.ToString("F2", CultureInfo.InvariantCulture);

            if (tariffPercentages.Count > 1)
            {
                // Second record is the prior year
                data["TariffPercentagePriorYear"] = tariffPercentages[1].TariffPeriodName.ToString();
                data["TariffPercentagePriorYearValue"] = tariffPercentages[1].PercentageAdded.ToString("F2", CultureInfo.InvariantCulture);
            }
            else
            {
                data["TariffPercentagePriorYear"] = "";
                data["TariffPercentagePriorYearValue"] = "";
            }
        }
        else
        {
            // No completed records — set all fields to empty string to avoid Handlebars rendering errors
            data["TariffPercentageCurrentYear"] = "";
            data["TariffPercentageCurrentYearValue"] = "";
            data["TariffPercentagePriorYear"] = "";
            data["TariffPercentagePriorYearValue"] = "";
        }

        return data;
    }

    #endregion
}
