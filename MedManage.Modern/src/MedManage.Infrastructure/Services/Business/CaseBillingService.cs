using MedManage.Infrastructure.Mapping.Manual;
using MedManage.Infrastructure.Persistence;
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
    private readonly MedManageDbContext _dbContext;

    public CaseBillingService(IUnitOfWork unitOfWork, MedManageDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
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
        var query = from cb in _dbContext.CaseBillings
                    where cb.DateDeleted == null
                    join c in _dbContext.Cases on cb.CaseId equals c.CaseId into caseJoin
                    from c in caseJoin.DefaultIfEmpty()
                    join sp in _dbContext.ServiceProviders on cb.ServiceProviderId equals sp.ServiceProviderId into spJoin
                    from sp in spJoin.DefaultIfEmpty()
                    join bs in _dbContext.BillingStatuses on cb.BillingStatusId equals bs.BillingStatusId into bsJoin
                    from bs in bsJoin.DefaultIfEmpty()
                    select new { cb, c, sp, bs };

        // Apply filters
        if (request.ServiceProviderId.HasValue)
            query = query.Where(x => x.cb.ServiceProviderId == request.ServiceProviderId.Value);

        if (!string.IsNullOrEmpty(request.AccountNumber))
            query = query.Where(x => x.cb.AccountNumber != null && x.cb.AccountNumber.Contains(request.AccountNumber));

        if (!string.IsNullOrEmpty(request.InvoiceNumber))
            query = query.Where(x => x.cb.InvoiceNumber != null && x.cb.InvoiceNumber.Contains(request.InvoiceNumber));

        if (!string.IsNullOrEmpty(request.Remittance))
            query = query.Where(x => x.cb.Remittance != null && x.cb.Remittance.Contains(request.Remittance));

        if (request.CaseId.HasValue)
            query = query.Where(x => x.cb.CaseId == request.CaseId.Value);

        if (request.BillingStatusId.HasValue)
            query = query.Where(x => x.cb.BillingStatusId == request.BillingStatusId.Value);

        if (request.Paid.HasValue)
            query = query.Where(x => x.cb.Paid == request.Paid.Value);

        if (request.DateReceivedFrom.HasValue)
            query = query.Where(x => x.cb.DateReceived >= request.DateReceivedFrom.Value);

        if (request.DateReceivedTo.HasValue)
            query = query.Where(x => x.cb.DateReceived <= request.DateReceivedTo.Value);

        if (!string.IsNullOrEmpty(request.ProviderName))
            query = query.Where(x => x.sp != null && x.sp.PracticeName != null && x.sp.PracticeName.Contains(request.ProviderName));

        if (!string.IsNullOrEmpty(request.MemberName))
            query = query.Where(x =>
                (x.cb.PatientSurname != null && x.cb.PatientSurname.Contains(request.MemberName)) ||
                (x.cb.PatientName != null && x.cb.PatientName.Contains(request.MemberName)));

        if (!string.IsNullOrEmpty(request.MemberNumber))
            query = query.Where(x => x.c != null && x.c.Member != null && x.c.Member.MemberNumber != null && x.c.Member.MemberNumber.Contains(request.MemberNumber));

        // Main client filter (through case → member → medical aid)
        if (request.MainClientId.HasValue)
            query = query.Where(x => x.c != null && x.c.Member != null && x.c.Member.MedicalAid != null && x.c.Member.MedicalAid.MainClientId == request.MainClientId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.cb.DateReceived ?? DateOnly.MinValue)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new CaseBillingDto
            {
                CaseBillingId = x.cb.CaseBillingId,
                CaseId = x.cb.CaseId,
                CaseNumber = x.c != null ? x.c.AuthNumber : null,
                ServiceProviderId = x.cb.ServiceProviderId,
                ProviderName = x.sp != null ? x.sp.PracticeName : null,
                MemberName = x.cb.PatientSurname != null ? (x.cb.PatientSurname + ", " + (x.cb.PatientName ?? "")) : null,
                BillingStatusName = x.bs != null ? x.bs.BillingStatus1 : null,
                AccountDate = x.cb.AccountDate,
                AccountToDate = x.cb.AccountToDate,
                AccountNumber = x.cb.AccountNumber,
                InvoiceNumber = x.cb.InvoiceNumber,
                DateReceived = x.cb.DateReceived,
                Submitted = x.cb.Submitted,
                DateSubmitted = x.cb.DateSubmitted,
                AmountDue = x.cb.AmountDue,
                Discount = x.cb.Discount,
                Penalty = x.cb.Penalty,
                Paid = x.cb.Paid,
                DatePaid = x.cb.DatePaid,
                Remittance = x.cb.Remittance,
                FinalInvoiceAmountDue = x.cb.FinalInvoiceAmountDue,
                BillingStatusId = x.cb.BillingStatusId,
                PatientName = x.cb.PatientName,
                PatientSurname = x.cb.PatientSurname,
                Comment = x.cb.Comment,
                DateInserted = x.cb.DateInserted,
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CaseBillingDto>
        {
            Items = items,
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
            billing.BillingStatusId = 2; // Submitted — auto-set when remittance is created
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

    public async Task<BulkPaymentResult> ImportStatusUpdatesAsync(List<BillingStatusImportItem> items, CancellationToken cancellationToken = default)
    {
        var result = new BulkPaymentResult();
        var ids = items.Select(i => i.Id).ToList();

        var billings = await _dbContext.CaseBillings
            .Where(cb => ids.Contains(cb.CaseBillingId) && cb.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            var billing = billings.FirstOrDefault(b => b.CaseBillingId == item.Id);
            if (billing == null)
            {
                result.FailedCount++;
                result.FailedIds.Add(item.Id);
                continue;
            }

            // Update paid status and infer BillingStatusId
            billing.Paid = item.Paid;
            if (item.Paid)
            {
                billing.BillingStatusId = 3; // Paid
            }
            else if (!string.IsNullOrWhiteSpace(item.RemittanceNumber) && string.IsNullOrWhiteSpace(billing.Remittance))
            {
                // New remittance number added where there wasn't one — set to Submitted
                billing.BillingStatusId = 2; // Submitted
            }

            if (!string.IsNullOrWhiteSpace(item.DatePaid) && DateOnly.TryParse(item.DatePaid, out var datePaid))
            {
                billing.DatePaid = datePaid;
            }

            if (!string.IsNullOrWhiteSpace(item.RemittanceNumber))
            {
                billing.Remittance = item.RemittanceNumber;
            }

            // Update financial fields (but NOT AmountDue)
            if (item.Discount.HasValue)
                billing.Discount = item.Discount.Value;

            if (item.Penalty.HasValue)
                billing.Penalty = item.Penalty.Value;

            if (item.RejectedAmount.HasValue)
                billing.Rejected = item.RejectedAmount.Value;

            if (item.FinalInvoiceAmount.HasValue)
                billing.FinalInvoiceAmountDue = item.FinalInvoiceAmount.Value;

            billing.DateUpdated = DateTime.UtcNow;
            result.SuccessCount++;
            result.SuccessIds.Add(item.Id);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        result.Message = $"Successfully updated {result.SuccessCount} record(s). {result.FailedCount} failed.";
        return result;
    }
}
