using MedManage.Infrastructure.Mapping.Manual;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Manages case status workflow transitions (e.g., Booking → Case).
/// Validates that transitions are allowed and applies side effects.
/// </summary>
public class CaseWorkflowService : ICaseWorkflowService
{
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    // Define allowed transitions: Dictionary<fromStatusName, List<toStatusName>>
    private static readonly Dictionary<string, List<string>> AllowedTransitions = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Booking", new List<string> { "Case", "Active" } },
        { "Active", new List<string> { "Closed", "On Hold" } },
        { "On Hold", new List<string> { "Active" } },
    };

    public CaseWorkflowService(
        MedManageDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CaseDto> TransitionStatusAsync(int caseId, CaseStatusTransitionRequest request, CancellationToken cancellationToken = default)
    {
        // Load the case with its current status
        var caseEntity = await _context.Cases
            .Include(c => c.Status)
            .Include(c => c.Member)
            .Include(c => c.ReferTo)
            .Include(c => c.ReferFrom)
            .FirstOrDefaultAsync(c => c.CaseId == caseId && c.DateDeleted == null, cancellationToken);

        if (caseEntity == null)
        {
            throw new KeyNotFoundException($"Case with ID {caseId} not found");
        }

        // Determine target status
        CaseStatus? targetStatus;
        if (request.TargetStatusId > 0)
        {
            targetStatus = await _context.CaseStatuses
                .FirstOrDefaultAsync(s => s.CaseStatusId == request.TargetStatusId && s.DateDeleted == null, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(request.TargetStatusName))
        {
            targetStatus = await _context.CaseStatuses
                .FirstOrDefaultAsync(s => s.CaseStatus1 != null && s.CaseStatus1.ToLower() == request.TargetStatusName.ToLower() && s.DateDeleted == null, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("Either TargetStatusId or TargetStatusName must be specified");
        }

        if (targetStatus == null)
        {
            throw new InvalidOperationException($"Target status not found");
        }

        // Get current status name for validation
        var currentStatusName = caseEntity.Status?.CaseStatus1 ?? "Unknown";
        var targetStatusName = targetStatus.CaseStatus1 ?? "Unknown";

        // Validate the transition is allowed
        if (!IsTransitionAllowed(currentStatusName, targetStatusName))
        {
            throw new InvalidOperationException(
                $"Transition from '{currentStatusName}' to '{targetStatusName}' is not allowed. " +
                $"Allowed transitions from '{currentStatusName}': {GetAllowedTargets(currentStatusName)}");
        }

        // Apply transition
        caseEntity.StatusId = targetStatus.CaseStatusId;

        // Apply side effects for specific transitions
        if (IsBookingToCaseTransition(currentStatusName, targetStatusName))
        {
            caseEntity.ChangeToCaseDate = DateTime.UtcNow;
            caseEntity.HasBooking = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Reload to get navigation properties
        await _context.Entry(caseEntity).Reference(c => c.Status).LoadAsync(cancellationToken);

        return caseEntity.ToDto();
    }

    private static bool IsTransitionAllowed(string currentStatus, string targetStatus)
    {
        // If no rules defined for current status, allow any transition (permissive by default)
        if (!AllowedTransitions.TryGetValue(currentStatus, out var allowedTargets))
        {
            return true;
        }

        return allowedTargets.Contains(targetStatus, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsBookingToCaseTransition(string currentStatus, string targetStatus)
    {
        return string.Equals(currentStatus, "Booking", StringComparison.OrdinalIgnoreCase)
            && (string.Equals(targetStatus, "Case", StringComparison.OrdinalIgnoreCase)
                || string.Equals(targetStatus, "Active", StringComparison.OrdinalIgnoreCase));
    }

    private static string GetAllowedTargets(string currentStatus)
    {
        if (AllowedTransitions.TryGetValue(currentStatus, out var allowedTargets))
        {
            return string.Join(", ", allowedTargets);
        }

        return "(any)";
    }
}
