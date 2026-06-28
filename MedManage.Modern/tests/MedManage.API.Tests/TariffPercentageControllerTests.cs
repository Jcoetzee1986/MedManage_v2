using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using MedManage.API.Controllers;
using MedManage.Core.DTOs.Common;
using MedManage.Core.DTOs.TariffPercentage;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Tests;

/// <summary>
/// Unit tests for TariffPercentageController endpoint responses.
/// Requirements: 2.3, 3.4, 5.2, 5.3, 7.2, 7.4
/// </summary>
public class TariffPercentageControllerTests
{
    private readonly Mock<ITariffPercentageService> _serviceMock;
    private readonly Mock<ILogger<TariffPercentageController>> _loggerMock;
    private readonly TariffPercentageController _controller;

    public TariffPercentageControllerTests()
    {
        _serviceMock = new Mock<ITariffPercentageService>();
        _loggerMock = new Mock<ILogger<TariffPercentageController>>();
        _controller = new TariffPercentageController(_serviceMock.Object, _loggerMock.Object);
    }

    #region Apply Endpoint - 202 Accepted (Requirement 5.2)

    [Fact]
    public async Task Apply_WithValidPendingRecord_Returns202Accepted()
    {
        // Arrange
        var jobId = Guid.NewGuid().ToString();
        var jobStatus = new TariffUpdateJobStatus
        {
            JobId = jobId,
            Status = "Queued"
        };
        _serviceMock.Setup(s => s.ApplyPercentageAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(jobStatus);

        // Act
        var result = await _controller.Apply(1, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(202);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeTrue();
        response.Data!.JobId.Should().Be(jobId);
        response.Data.Status.Should().Be("Queued");
    }

    [Fact]
    public async Task Apply_WithValidPendingRecord_ReturnsJobId()
    {
        // Arrange
        var expectedJobId = Guid.NewGuid().ToString();
        var jobStatus = new TariffUpdateJobStatus
        {
            JobId = expectedJobId,
            Status = "Queued",
            StartedAt = null,
            CompletedAt = null
        };
        _serviceMock.Setup(s => s.ApplyPercentageAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(jobStatus);

        // Act
        var result = await _controller.Apply(5, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(202);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Data!.JobId.Should().Be(expectedJobId);
    }

    #endregion

    #region Apply Endpoint - 409 Conflict (Requirements 5.2, 5.3)

    [Fact]
    public async Task Apply_WhenAlreadyProcessing_Returns409Conflict()
    {
        // Arrange - service throws for duplicate/concurrent apply
        _serviceMock.Setup(s => s.ApplyPercentageAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("A propagation job is already in progress for this period"));

        // Act
        var result = await _controller.Apply(1, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(409);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Apply_WhenAnotherJobProcessingForSamePeriod_Returns409Conflict()
    {
        // Arrange
        _serviceMock.Setup(s => s.ApplyPercentageAsync(2, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("A propagation job is already in progress for this period"));

        // Act
        var result = await _controller.Apply(2, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(409);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
        response.Message.Should().Contain("already in progress");
    }

    #endregion

    #region GetById - 404 Not Found (Requirement 2.3)

    [Fact]
    public async Task GetById_WhenRecordDoesNotExist_Returns404NotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TariffPercentageDto?)null);

        // Act
        var result = await _controller.GetById(999, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(404);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffPercentageDto>>().Subject;
        response.Success.Should().BeFalse();
    }

    #endregion

    #region GetJobStatus - 404 Not Found (Requirement 7.2)

    [Fact]
    public async Task GetJobStatus_WhenJobDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var jobId = Guid.NewGuid().ToString();
        _serviceMock.Setup(s => s.GetJobStatusAsync(jobId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException($"Job with ID {jobId} not found"));

        // Act
        var result = await _controller.GetJobStatus(jobId, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(404);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
    }

    #endregion

    #region GetJobStatus - 400 Bad Request for invalid GUID (Requirement 7.4)

    [Fact]
    public async Task GetJobStatus_WithInvalidGuidFormat_Returns400BadRequest()
    {
        // Act
        var result = await _controller.GetJobStatus("not-a-guid", CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(400);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
        response.Message.Should().Contain("invalid");
    }

    [Fact]
    public async Task GetJobStatus_WithEmptyString_Returns400BadRequest()
    {
        // Act
        var result = await _controller.GetJobStatus("", CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetJobStatus_WithRandomText_Returns400BadRequest()
    {
        // Act
        var result = await _controller.GetJobStatus("abc123xyz", CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(400);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
    }

    #endregion

    #region CRUD Operations - Correct Status Codes (Requirements 2.3, 3.4)

    [Fact]
    public async Task GetAll_ReturnsOk200WithData()
    {
        // Arrange
        var percentages = new List<TariffPercentageDto>
        {
            new() { TariffPercentageId = 1, PercentageAdded = 250.60m, TariffPeriodName = 2026, Status = "Pending" }
        };
        _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(percentages);

        // Act
        var result = await _controller.GetAll(CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(200);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<IEnumerable<TariffPercentageDto>>>().Subject;
        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetById_WhenRecordExists_ReturnsOk200()
    {
        // Arrange
        var dto = new TariffPercentageDto
        {
            TariffPercentageId = 1,
            PercentageAdded = 250.60m,
            TariffPeriodName = 2026,
            Status = "Pending"
        };
        _serviceMock.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await _controller.GetById(1, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(200);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffPercentageDto>>().Subject;
        response.Success.Should().BeTrue();
        response.Data!.TariffPercentageId.Should().Be(1);
    }

    [Fact]
    public async Task Create_WithValidDto_Returns201Created()
    {
        // Arrange
        var createDto = new CreateTariffPercentageDto
        {
            PercentageAdded = 250.60m,
            TariffPeriodName = 2026,
            StartActiveDate = new DateOnly(2026, 1, 1)
        };
        var resultDto = new TariffPercentageDto
        {
            TariffPercentageId = 1,
            PercentageAdded = 250.60m,
            TariffPeriodName = 2026,
            StartActiveDate = new DateOnly(2026, 1, 1),
            Status = "Pending"
        };
        _serviceMock.Setup(s => s.CreateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultDto);

        // Act
        var result = await _controller.Create(createDto, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(201);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffPercentageDto>>().Subject;
        response.Success.Should().BeTrue();
        response.Data!.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task Create_WithOverlappingPeriod_Returns409Conflict()
    {
        // Arrange
        var createDto = new CreateTariffPercentageDto
        {
            PercentageAdded = 250.60m,
            TariffPeriodName = 2026,
            StartActiveDate = new DateOnly(2026, 1, 1)
        };
        _serviceMock.Setup(s => s.CreateAsync(createDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("A tariff percentage already exists for this period and date range."));

        // Act
        var result = await _controller.Create(createDto, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(409);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffPercentageDto>>().Subject;
        response.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Update_WhenRecordExists_ReturnsOk200()
    {
        // Arrange
        var updateDto = new UpdateTariffPercentageDto { PercentageAdded = 260.00m };
        var resultDto = new TariffPercentageDto
        {
            TariffPercentageId = 1,
            PercentageAdded = 260.00m,
            TariffPeriodName = 2026,
            Status = "Pending"
        };
        _serviceMock.Setup(s => s.UpdateAsync(1, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultDto);

        // Act
        var result = await _controller.Update(1, updateDto, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(200);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffPercentageDto>>().Subject;
        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Update_WhenRecordNotFound_Returns404NotFound()
    {
        // Arrange
        var updateDto = new UpdateTariffPercentageDto { PercentageAdded = 260.00m };
        _serviceMock.Setup(s => s.UpdateAsync(999, updateDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException("Tariff percentage with ID 999 not found"));

        // Act
        var result = await _controller.Update(999, updateDto, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Delete_WhenRecordExists_Returns204NoContent()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WhenRecordNotFound_Returns404NotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetJobStatus_WithValidGuid_ReturnsOk200()
    {
        // Arrange
        var jobId = Guid.NewGuid().ToString();
        var jobStatus = new TariffUpdateJobStatus
        {
            JobId = jobId,
            Status = "Completed",
            RecordsAffected = 15000
        };
        _serviceMock.Setup(s => s.GetJobStatusAsync(jobId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(jobStatus);

        // Act
        var result = await _controller.GetJobStatus(jobId, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(200);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeTrue();
        response.Data!.Status.Should().Be("Completed");
        response.Data.RecordsAffected.Should().Be(15000);
    }

    [Fact]
    public async Task GetActiveForLetter_ReturnsOk200()
    {
        // Arrange
        var percentages = new List<TariffPercentageDto>
        {
            new() { TariffPercentageId = 1, TariffPeriodName = 2026, PercentageAdded = 250.60m, Status = "Completed" },
            new() { TariffPercentageId = 2, TariffPeriodName = 2025, PercentageAdded = 233.90m, Status = "Completed" }
        };
        _serviceMock.Setup(s => s.GetActivePercentagesForLetterAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(percentages);

        // Act
        var result = await _controller.GetActiveForLetter(CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(200);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<IEnumerable<TariffPercentageDto>>>().Subject;
        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(2);
    }

    #endregion

    #region Apply Endpoint - Error Cases

    [Fact]
    public async Task Apply_WhenRecordNotFound_Returns404NotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.ApplyPercentageAsync(999, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException("Tariff percentage with ID 999 not found"));

        // Act
        var result = await _controller.Apply(999, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(404);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Apply_WhenRecordAlreadyCompleted_Returns400BadRequest()
    {
        // Arrange - service throws for completed record (not "already in progress")
        _serviceMock.Setup(s => s.ApplyPercentageAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("The tariff percentage has already been successfully applied"));

        // Act
        var result = await _controller.Apply(1, CancellationToken.None);

        // Assert
        var objectResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(400);
        var response = objectResult.Value.Should().BeOfType<ApiResponse<TariffUpdateJobStatus>>().Subject;
        response.Success.Should().BeFalse();
    }

    #endregion
}
