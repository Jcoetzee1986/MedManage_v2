using MedManage.Core.DTOs.Case;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Service for deep copying cases including all sub-entities
/// </summary>
public interface ICaseCopyService
{
    /// <summary>
    /// Creates a deep copy of a case and all its linked sub-entities.
    /// The new case gets a new CaseID with reset audit dates.
    /// </summary>
    /// <param name="sourceCaseId">The ID of the case to copy</param>
    /// <param name="request">Optional configuration for which sub-entities to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The newly created case DTO</returns>
    Task<CaseDto> CopyAsync(int sourceCaseId, CaseCopyRequest? request = null, CancellationToken cancellationToken = default);
}
