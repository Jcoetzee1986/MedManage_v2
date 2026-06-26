using MedManage.Infrastructure.Mapping.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class CaseBillingService : ICaseBillingService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseBillingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseBillingDto>> GetAllAsync()
    {
        var billings = await _unitOfWork.CaseBillings
            .FindAsync(c => c.DateDeleted == null);
        return billings.OrderByDescending(c => c.DateInserted).Select(e => e.ToDto());
    }

    public async Task<CaseBillingDto?> GetByIdAsync(int id)
    {
        var billing = await _unitOfWork.CaseBillings.GetByIdAsync(id);
        if (billing == null || billing.DateDeleted != null)
            return null;

        return billing.ToDto();
    }

    public async Task<IEnumerable<CaseBillingDto>> GetByCaseIdAsync(int caseId)
    {
        var billings = await _unitOfWork.CaseBillings
            .FindAsync(c => c.CaseId == caseId && c.DateDeleted == null);
        return billings.OrderByDescending(c => c.DateInserted).Select(e => e.ToDto());
    }

    public async Task<CaseBillingDto> CreateAsync(CreateCaseBillingDto dto)
    {
        var billing = dto.ToEntity();
        billing.DateInserted = DateTime.Now;

        await _unitOfWork.CaseBillings.AddAsync(billing);
        await _unitOfWork.SaveChangesAsync();

        return billing.ToDto();
    }

    public async Task<CaseBillingDto> UpdateAsync(int id, UpdateCaseBillingDto dto)
    {
        var billing = await _unitOfWork.CaseBillings.GetByIdAsync(id);
        if (billing == null || billing.DateDeleted != null)
            throw new KeyNotFoundException($"CaseBilling with ID {id} not found");

        dto.ApplyTo(billing);
        billing.DateUpdated = DateTime.Now;

        await _unitOfWork.CaseBillings.UpdateAsync(billing);
        await _unitOfWork.SaveChangesAsync();

        return billing.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var billing = await _unitOfWork.CaseBillings.GetByIdAsync(id);
        if (billing == null || billing.DateDeleted != null)
            return false;

        billing.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseBillings.UpdateAsync(billing);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResult<CaseBillingDto>> SearchAsync(BillingSearchRequest request, CancellationToken cancellationToken = default)
    {
        // Build up predicate filters
        var billings = await _unitOfWork.CaseBillings.FindAsync(cb =>
            cb.DateDeleted == null
            && (!request.ServiceProviderId.HasValue || cb.ServiceProviderId == request.ServiceProviderId.Value)
            && (string.IsNullOrEmpty(request.AccountNumber) || (cb.AccountNumber != null && cb.AccountNumber.Contains(request.AccountNumber)))
            && (string.IsNullOrEmpty(request.InvoiceNumber) || (cb.InvoiceNumber != null && cb.InvoiceNumber.Contains(request.InvoiceNumber)))
            && (string.IsNullOrEmpty(request.Remittance) || (cb.Remittance != null && cb.Remittance.Contains(request.Remittance)))
            && (!request.CaseId.HasValue || cb.CaseId == request.CaseId.Value)
            && (!request.BillingStatusId.HasValue || cb.BillingStatusId == request.BillingStatusId.Value)
            && (!request.Paid.HasValue || cb.Paid == request.Paid.Value)
            && (!request.Submitted.HasValue || cb.Submitted == request.Submitted.Value)
            && (!request.DateReceivedFrom.HasValue || cb.DateReceived >= request.DateReceivedFrom.Value)
            && (!request.DateReceivedTo.HasValue || cb.DateReceived <= request.DateReceivedTo.Value)
            && (!request.DateSubmittedFrom.HasValue || cb.DateSubmitted >= request.DateSubmittedFrom.Value)
            && (!request.DateSubmittedTo.HasValue || cb.DateSubmitted <= request.DateSubmittedTo.Value)
            && (!request.DatePaidFrom.HasValue || cb.DatePaid >= request.DatePaidFrom.Value)
            && (!request.DatePaidTo.HasValue || cb.DatePaid <= request.DatePaidTo.Value)
        );

        var query = billings.AsQueryable();

        // Sort
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "accountnumber" => request.SortDescending ? query.OrderByDescending(b => b.AccountNumber) : query.OrderBy(b => b.AccountNumber),
            "datereceived" => request.SortDescending ? query.OrderByDescending(b => b.DateReceived) : query.OrderBy(b => b.DateReceived),
            "datepaid" => request.SortDescending ? query.OrderByDescending(b => b.DatePaid) : query.OrderBy(b => b.DatePaid),
            "amountdue" => request.SortDescending ? query.OrderByDescending(b => b.AmountDue) : query.OrderBy(b => b.AmountDue),
            _ => request.SortDescending ? query.OrderByDescending(b => b.DateInserted) : query.OrderBy(b => b.DateInserted),
        };

        var totalCount = query.Count();
        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<CaseBillingDto>
        {
            Items = items.Select(e => e.ToDto()),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<DuplicateAccountCheckResult> CheckDuplicateAccountAsync(string accountNumber, int? excludeBillingId = null)
    {
        var duplicates = await _unitOfWork.CaseBillings.FindAsync(cb =>
            cb.DateDeleted == null
            && cb.AccountNumber != null
            && cb.AccountNumber == accountNumber
            && (!excludeBillingId.HasValue || cb.CaseBillingId != excludeBillingId.Value)
        );

        var duplicateList = duplicates.ToList();

        return new DuplicateAccountCheckResult
        {
            IsDuplicate = duplicateList.Count > 0,
            ExistingBillings = duplicateList.Select(e => e.ToDto()).ToList(),
            Message = duplicateList.Count > 0
                ? $"Account number '{accountNumber}' already exists on {duplicateList.Count} billing record(s)."
                : null
        };
    }

    public async Task<BillingSummaryDto> GetBillingSummaryAsync(int caseId)
    {
        var billings = await _unitOfWork.CaseBillings
            .FindAsync(cb => cb.CaseId == caseId && cb.DateDeleted == null);

        var billingList = billings.ToList();

        return new BillingSummaryDto
        {
            CaseId = caseId,
            TotalBillings = billingList.Count,
            TotalAmountDue = billingList.Sum(b => b.AmountDue ?? 0),
            TotalDiscount = billingList.Sum(b => b.Discount ?? 0),
            TotalPenalty = billingList.Sum(b => b.Penalty ?? 0),
            TotalRejected = billingList.Sum(b => b.Rejected ?? 0),
            TotalPaid = billingList.Where(b => b.Paid == true).Sum(b => b.AmountDue ?? 0),
            TotalOutstanding = billingList.Where(b => b.Paid != true).Sum(b => (b.AmountDue ?? 0) - (b.Discount ?? 0) - (b.Rejected ?? 0)),
            PaidCount = billingList.Count(b => b.Paid == true),
            SubmittedCount = billingList.Count(b => b.Submitted == true),
            PendingCount = billingList.Count(b => b.Paid != true && b.Submitted != true),
            Billings = billingList.Select(e => e.ToDto()).ToList()
        };
    }

    public async Task<BulkPaymentResult> BulkMarkAsPaidAsync(BulkPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var result = new BulkPaymentResult();

        foreach (var billingId in request.BillingIds)
        {
            var billing = await _unitOfWork.CaseBillings.GetByIdAsync(billingId);
            if (billing == null || billing.DateDeleted != null)
            {
                result.FailedIds.Add(billingId);
                continue;
            }

            billing.Paid = true;
            billing.DatePaid = request.DatePaid;
            billing.DateUpdated = DateTime.Now;

            if (request.PaymentAmount.HasValue)
                billing.FinalInvoiceAmountDue = request.PaymentAmount.Value;

            if (!string.IsNullOrWhiteSpace(request.Comments))
                billing.Comment = request.Comments;

            if (!string.IsNullOrWhiteSpace(request.Remittance))
                billing.Remittance = request.Remittance;

            await _unitOfWork.CaseBillings.UpdateAsync(billing);
            result.SuccessIds.Add(billingId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        result.SuccessCount = result.SuccessIds.Count;
        result.FailedCount = result.FailedIds.Count;
        result.Message = $"{result.SuccessCount} billing record(s) marked as paid. {result.FailedCount} failed.";

        return result;
    }

    public async Task<RemittanceUpdateResult> UpdateRemittanceAsync(RemittanceUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var result = new RemittanceUpdateResult();

        foreach (var billingId in request.BillingIds)
        {
            var billing = await _unitOfWork.CaseBillings.GetByIdAsync(billingId);
            if (billing == null || billing.DateDeleted != null)
            {
                result.FailedIds.Add(billingId);
                continue;
            }

            billing.Remittance = request.RemittanceNumber;
            billing.DateUpdated = DateTime.Now;

            await _unitOfWork.CaseBillings.UpdateAsync(billing);
            result.SuccessIds.Add(billingId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        result.SuccessCount = result.SuccessIds.Count;
        result.FailedCount = result.FailedIds.Count;
        result.Message = $"{result.SuccessCount} billing record(s) updated with remittance '{request.RemittanceNumber}'. {result.FailedCount} failed.";

        return result;
    }

    public async Task<IEnumerable<CaseBillingDto>> GetByRemittanceAsync(string remittanceNumber, CancellationToken cancellationToken = default)
    {
        var billings = await _unitOfWork.CaseBillings
            .FindAsync(cb => cb.DateDeleted == null && cb.Remittance != null && cb.Remittance == remittanceNumber);

        return billings.OrderByDescending(c => c.DateInserted).Select(e => e.ToDto());
    }
}
