using MedManage.Core.DTOs.Case;

namespace MedManage.Core.Interfaces.Services;

/// <summary>
/// Validates business rules for case create/update operations.
/// Goes beyond field-level validation to check cross-entity constraints:
/// member eligibility, date consistency, required fields by status, and auth number management.
/// </summary>
public interface ICaseBusinessRuleService
{
    /// <summary>
    /// Validates all business rules for case creation
    /// </summary>
    Task<BusinessRuleValidationResult> ValidateCreateAsync(CreateCaseRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates all business rules for case update
    /// </summary>
    Task<BusinessRuleValidationResult> ValidateUpdateAsync(UpdateCaseRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a member's medical aid product allows services.
    /// Returns full member status information including warnings.
    /// </summary>
    Task<MemberStatusCheckResult> CheckMemberAllowServicesAsync(int memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates the correct auth number with prefix from the member's medical aid.
    /// If the auth number already has the correct prefix, returns it unchanged.
    /// </summary>
    Task<AuthNumberPrefixResult> GenerateAuthNumberWithPrefixAsync(int memberId, string? baseAuthNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates that an auth number has the correct prefix for the member's medical aid.
    /// </summary>
    Task<AuthNumberPrefixResult> ValidateAuthNumberPrefixAsync(int memberId, string? authNumber, CancellationToken cancellationToken = default);
}
