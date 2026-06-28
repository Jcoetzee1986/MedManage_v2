using MedManage.Core.DTOs.Case;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for managing case status workflow transitions (e.g., Booking → Case)
/// </summary>
public interface ICaseWorkflowService
{
    /// <summary>
    /// Transitions a case to a new status if the transition is valid.
    /// For Booking → Case transitions, sets ChangeToCaseDate.
    /// </summary>
    /// <param name="caseId">The case to transition</param>
    /// <param name="request">The transition request containing target status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated case DTO</returns>
    Task<CaseDto> TransitionStatusAsync(int caseId, CaseStatusTransitionRequest request, CancellationToken cancellationToken = default);
}
