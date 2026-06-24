using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Validates business rules for case operations.
/// Checks cross-entity constraints that go beyond simple field validation.
/// </summary>
public class CaseBusinessRuleService : ICaseBusinessRuleService
{
    private readonly MedManageDbContext _context;

    public CaseBusinessRuleService(MedManageDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessRuleValidationResult> ValidateCreateAsync(CreateCaseRequest request, CancellationToken cancellationToken = default)
    {
        var result = new BusinessRuleValidationResult();

        // 1. Member eligibility check
        if (request.MemberId.HasValue)
        {
            await ValidateMemberEligibility(request.MemberId.Value, result, cancellationToken);
        }

        // 2. Date consistency
        ValidateDateConsistency(request.AdmissionDate, request.DischargeDate, request.StatusId, result);

        // 3. Required fields for non-booking cases
        ValidateRequiredFieldsForStatus(request.StatusId, request.MemberId, request.ReferToId, request.ReferFromId, result);

        return result;
    }

    public async Task<BusinessRuleValidationResult> ValidateUpdateAsync(UpdateCaseRequest request, CancellationToken cancellationToken = default)
    {
        var result = new BusinessRuleValidationResult();

        // 1. Member eligibility check (if member changed)
        if (request.MemberId.HasValue)
        {
            await ValidateMemberEligibility(request.MemberId.Value, result, cancellationToken);
        }

        // 2. Date consistency
        ValidateDateConsistency(request.AdmissionDate, request.DischargeDate, request.StatusId, result);

        // 3. Required fields for the case status
        ValidateRequiredFieldsForStatus(request.StatusId, request.MemberId, request.ReferToId, request.ReferFromId, result);

        // 4. For closed cases, ensure dates are not in the future
        await ValidateClosedCaseDates(request.CaseId, request.StatusId, request.AdmissionDate, request.DischargeDate, result, cancellationToken);

        return result;
    }

    public async Task<MemberStatusCheckResult> CheckMemberAllowServicesAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var member = await _context.Members
            .Include(m => m.MedicalAid)
            .Include(m => m.MemberStatus)
            .FirstOrDefaultAsync(m => m.MemberId == memberId && m.DateDeleted == null, cancellationToken);

        if (member == null)
        {
            return new MemberStatusCheckResult
            {
                MemberId = memberId,
                AllowServices = false,
                IsActive = false,
                Warnings = { "Member not found" }
            };
        }

        var checkResult = new MemberStatusCheckResult
        {
            MemberId = memberId,
            IsSuspended = member.Suspended == true,
            IsDeceased = member.Deceased == true,
            IsMedicalAidExhausted = member.MedicalAidExhausted == true,
            IsActive = true,
            MemberName = $"{member.Name} {member.Surname}".Trim(),
            MedicalAidName = member.MedicalAid?.MedicalAidName
        };

        // Check the product's AllowServices flag
        if (member.MedAidProductId.HasValue)
        {
            var product = await _context.MedicalAidProducts
                .FirstOrDefaultAsync(p => p.MedAidProductId == member.MedAidProductId.Value && p.DateDeleted == null, cancellationToken);

            if (product != null)
            {
                checkResult.AllowServices = product.AllowServices == true;
                checkResult.ProductName = product.MedAidProductName;
            }
            else
            {
                checkResult.AllowServices = false;
                checkResult.Warnings.Add("Member's medical aid product not found");
            }
        }
        else
        {
            // No product assigned — default to allowing services but warn
            checkResult.AllowServices = true;
            checkResult.Warnings.Add("Member has no medical aid product assigned");
        }

        // Build warnings
        if (checkResult.IsSuspended)
            checkResult.Warnings.Add("Member is suspended");
        if (checkResult.IsDeceased)
            checkResult.Warnings.Add("Member is deceased");
        if (checkResult.IsMedicalAidExhausted)
            checkResult.Warnings.Add("Member's medical aid benefits are exhausted");
        if (!checkResult.AllowServices)
            checkResult.Warnings.Add($"Medical aid product '{checkResult.ProductName}' does not allow services");

        return checkResult;
    }

    public async Task<AuthNumberPrefixResult> GenerateAuthNumberWithPrefixAsync(int memberId, string? baseAuthNumber, CancellationToken cancellationToken = default)
    {
        var prefix = await GetMemberMedicalAidPrefix(memberId, cancellationToken);

        var result = new AuthNumberPrefixResult
        {
            BaseNumber = baseAuthNumber,
            Prefix = prefix.Prefix,
            MedicalAidName = prefix.MedicalAidName
        };

        if (string.IsNullOrWhiteSpace(baseAuthNumber))
        {
            result.GeneratedAuthNumber = null;
            result.HasCorrectPrefix = false;
            return result;
        }

        if (string.IsNullOrWhiteSpace(prefix.Prefix))
        {
            // No prefix for this medical aid — return as-is
            result.GeneratedAuthNumber = baseAuthNumber;
            result.HasCorrectPrefix = true;
            return result;
        }

        // Check if already prefixed
        if (baseAuthNumber.StartsWith(prefix.Prefix, StringComparison.OrdinalIgnoreCase))
        {
            result.GeneratedAuthNumber = baseAuthNumber;
            result.HasCorrectPrefix = true;
        }
        else
        {
            result.GeneratedAuthNumber = prefix.Prefix + baseAuthNumber;
            result.HasCorrectPrefix = false;
        }

        return result;
    }

    public async Task<AuthNumberPrefixResult> ValidateAuthNumberPrefixAsync(int memberId, string? authNumber, CancellationToken cancellationToken = default)
    {
        var prefix = await GetMemberMedicalAidPrefix(memberId, cancellationToken);

        var result = new AuthNumberPrefixResult
        {
            Prefix = prefix.Prefix,
            MedicalAidName = prefix.MedicalAidName,
            GeneratedAuthNumber = authNumber
        };

        if (string.IsNullOrWhiteSpace(authNumber) || string.IsNullOrWhiteSpace(prefix.Prefix))
        {
            result.HasCorrectPrefix = true;
            result.BaseNumber = authNumber;
            return result;
        }

        if (authNumber.StartsWith(prefix.Prefix, StringComparison.OrdinalIgnoreCase))
        {
            result.HasCorrectPrefix = true;
            result.BaseNumber = authNumber[prefix.Prefix.Length..];
        }
        else
        {
            result.HasCorrectPrefix = false;
            result.BaseNumber = authNumber;
        }

        return result;
    }

    #region Private Helpers

    private async Task ValidateMemberEligibility(int memberId, BusinessRuleValidationResult result, CancellationToken cancellationToken)
    {
        var memberCheck = await CheckMemberAllowServicesAsync(memberId, cancellationToken);

        if (!memberCheck.IsActive)
        {
            result.AddError("MemberNotFound", $"Member with ID {memberId} not found");
            return;
        }

        if (memberCheck.IsDeceased)
        {
            result.AddError("MemberDeceased", "Cannot create/update case for a deceased member");
            return;
        }

        if (memberCheck.IsSuspended)
        {
            // Suspension is a warning, not a hard block (legacy behaviour was to warn)
            result.AddWarning("MemberSuspended", "Member is suspended — proceed with caution");
        }

        if (!memberCheck.AllowServices)
        {
            result.AddError("AllowServicesDenied",
                $"Member's medical aid product '{memberCheck.ProductName}' does not allow services");
        }

        if (memberCheck.IsMedicalAidExhausted)
        {
            result.AddWarning("MedicalAidExhausted", "Member's medical aid benefits are exhausted");
        }
    }

    private static void ValidateDateConsistency(DateOnly? admissionDate, DateOnly? dischargeDate, int? statusId, BusinessRuleValidationResult result)
    {
        if (admissionDate.HasValue && dischargeDate.HasValue)
        {
            if (dischargeDate.Value < admissionDate.Value)
            {
                result.AddError("DateConsistency", "Discharge date must be on or after admission date");
            }
        }
    }

    private static void ValidateRequiredFieldsForStatus(int? statusId, int? memberId, int? referToId, int? referFromId, BusinessRuleValidationResult result)
    {
        // For active cases (non-booking), member and at least one provider are required.
        // StatusId conventions: typically 1 = Booking, other values = Active/Closed.
        // We use a simple heuristic: if statusId is provided and > 1, enforce required fields.
        if (statusId.HasValue && statusId.Value > 1)
        {
            if (!memberId.HasValue || memberId.Value <= 0)
            {
                result.AddError("MemberRequired", "A member is required for active cases");
            }

            if ((!referToId.HasValue || referToId.Value <= 0) && (!referFromId.HasValue || referFromId.Value <= 0))
            {
                result.AddError("ProviderRequired", "At least one service provider (Refer To or Refer From) is required for active cases");
            }
        }
    }

    private async Task ValidateClosedCaseDates(int caseId, int? statusId, DateOnly? admissionDate, DateOnly? dischargeDate, BusinessRuleValidationResult result, CancellationToken cancellationToken)
    {
        // Determine if the case is being closed by checking status name
        if (!statusId.HasValue) return;

        var status = await _context.CaseStatuses
            .FirstOrDefaultAsync(s => s.CaseStatusId == statusId.Value && s.DateDeleted == null, cancellationToken);

        if (status == null) return;

        var statusName = status.CaseStatus1?.ToLower() ?? "";
        if (statusName.Contains("closed") || statusName.Contains("complete"))
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (admissionDate.HasValue && admissionDate.Value > today)
            {
                result.AddError("FutureAdmissionDate", "Admission date cannot be in the future for closed cases");
            }

            if (dischargeDate.HasValue && dischargeDate.Value > today)
            {
                result.AddError("FutureDischargeDate", "Discharge date cannot be in the future for closed cases");
            }

            // Validate against facility type dates if any exist
            var facilityTypes = await _context.CaseFacilityTypes
                .Where(f => f.CaseId == caseId && f.DateDeleted == null)
                .ToListAsync(cancellationToken);

            if (facilityTypes.Any())
            {
                var earliestFacilityAdmission = facilityTypes
                    .Min(f => DateOnly.FromDateTime(f.DateAdmitted));

                var latestFacilityDischarge = facilityTypes
                    .Where(f => f.DateDischarged.HasValue)
                    .Select(f => DateOnly.FromDateTime(f.DateDischarged!.Value))
                    .DefaultIfEmpty()
                    .Max();

                if (admissionDate.HasValue && admissionDate.Value > earliestFacilityAdmission)
                {
                    result.AddError("AdmissionAfterFacility",
                        "Case admission date must be on or before the earliest facility admission date");
                }

                if (latestFacilityDischarge != default && dischargeDate.HasValue
                    && dischargeDate.Value < latestFacilityDischarge)
                {
                    result.AddError("DischargeBeforeFacility",
                        "Case discharge date must be on or after the latest facility discharge date");
                }
            }
        }
    }

    private async Task<(string? Prefix, string? MedicalAidName)> GetMemberMedicalAidPrefix(int memberId, CancellationToken cancellationToken)
    {
        var member = await _context.Members
            .Include(m => m.MedicalAid)
            .FirstOrDefaultAsync(m => m.MemberId == memberId && m.DateDeleted == null, cancellationToken);

        if (member?.MedicalAid == null)
        {
            return (null, null);
        }

        return (member.MedicalAid.CasePrefix, member.MedicalAid.MedicalAidName);
    }

    #endregion
}
