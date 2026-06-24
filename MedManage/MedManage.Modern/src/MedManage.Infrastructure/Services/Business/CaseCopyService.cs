using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Case;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class CaseCopyService : ICaseCopyService
{
    private readonly MedManageDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CaseCopyService(
        MedManageDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CaseDto> CopyAsync(int sourceCaseId, CaseCopyRequest? request = null, CancellationToken cancellationToken = default)
    {
        request ??= new CaseCopyRequest();

        // Load source case
        var sourceCase = await _context.Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CaseId == sourceCaseId && c.DateDeleted == null, cancellationToken);

        if (sourceCase == null)
        {
            throw new KeyNotFoundException($"Case with ID {sourceCaseId} not found");
        }

        // Create new case with same data but reset audit fields
        var newCase = new Case
        {
            AuthNumber = sourceCase.AuthNumber,
            AccountNr = sourceCase.AccountNr,
            MemberId = sourceCase.MemberId,
            ReferToId = sourceCase.ReferToId,
            ReferFromId = sourceCase.ReferFromId,
            AdmissionDate = sourceCase.AdmissionDate,
            AdmissionTime = sourceCase.AdmissionTime,
            DischargeDate = sourceCase.DischargeDate,
            DischargeTime = sourceCase.DischargeTime,
            AuthTypeId = sourceCase.AuthTypeId,
            WcaIod = sourceCase.WcaIod,
            TotalLengthOfStay = sourceCase.TotalLengthOfStay,
            TotalAmount = sourceCase.TotalAmount,
            FinalInvoiceAmount = sourceCase.FinalInvoiceAmount,
            FinalInvoiceAmountUpdated = sourceCase.FinalInvoiceAmountUpdated,
            StatusId = sourceCase.StatusId,
            CaseDescription = sourceCase.CaseDescription,
            Changes = sourceCase.Changes,
            Limits = sourceCase.Limits,
            Exclusions = sourceCase.Exclusions,
            DateCreated = DateOnly.FromDateTime(DateTime.UtcNow),
            HasBooking = sourceCase.HasBooking,
            PenaltyPercentage = sourceCase.PenaltyPercentage,
            CaseCategoryId = sourceCase.CaseCategoryId,
            // Reset audit fields — DbContext SaveChanges will set DateInserted/UserID
        };

        _context.Cases.Add(newCase);
        await _context.SaveChangesAsync(cancellationToken);

        var newCaseId = newCase.CaseId;

        // Deep copy sub-entities
        if (request.IncludeCptCodes)
        {
            await CopyCptCodes(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeIcdCodes)
        {
            await CopyIcdCodes(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeTariffs)
        {
            await CopyTariffs(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeFacilityTypes)
        {
            await CopyFacilityTypes(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeExclusions)
        {
            await CopyExclusions(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeChecklist)
        {
            await CopyChecklist(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeNotes)
        {
            await CopyNotes(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeComments)
        {
            await CopyComments(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeNappiCodes)
        {
            await CopyNappiCodes(sourceCaseId, newCaseId, cancellationToken);
        }

        if (request.IncludeLetterNotes)
        {
            await CopyLetterNotes(sourceCaseId, newCaseId, cancellationToken);
        }

        // Reload the new case for the response DTO
        var createdCase = await _context.Cases
            .Include(c => c.Member)
            .Include(c => c.Status)
            .Include(c => c.ReferTo)
            .Include(c => c.ReferFrom)
            .FirstAsync(c => c.CaseId == newCaseId, cancellationToken);

        return _mapper.Map<CaseDto>(createdCase);
    }

    private async Task CopyCptCodes(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseCpts
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseCpts.Add(new CaseCpt
            {
                CaseId = newCaseId,
                Cptid = item.Cptid,
                DateOfProcedure = item.DateOfProcedure,
                PrimaryCode = item.PrimaryCode,
                SecondaryCode = item.SecondaryCode,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyIcdCodes(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseIcds
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseIcds.Add(new CaseIcd
            {
                CaseId = newCaseId,
                Icdid = item.Icdid,
                DateOfProcedure = item.DateOfProcedure,
                PrimaryCode = item.PrimaryCode,
                SecondaryCode = item.SecondaryCode,
                CoMorbidityCode = item.CoMorbidityCode,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyTariffs(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseTariffs
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseTariffs.Add(new CaseTariff
            {
                CaseId = newCaseId,
                TariffId = item.TariffId,
                Value = item.Value,
                Qty = item.Qty,
                AgreedRateOverride = item.AgreedRateOverride,
                ValueIsTotal = item.ValueIsTotal,
                Rejected = item.Rejected,
                DateOfProcedure = item.DateOfProcedure,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyFacilityTypes(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseFacilityTypes
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseFacilityTypes.Add(new CaseFacilityType
            {
                CaseId = newCaseId,
                FacilityTypeId = item.FacilityTypeId,
                DateAdmitted = item.DateAdmitted,
                DateDischarged = item.DateDischarged,
                Los = item.Los,
                FacilityTypeCode = item.FacilityTypeCode,
                MinutesOnVentilator = item.MinutesOnVentilator,
                Comments = item.Comments,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyExclusions(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseExclusions
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseExclusions.Add(new CaseExclusion
            {
                CaseId = newCaseId,
                ExclusionId = item.ExclusionId,
                Comment = item.Comment,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyChecklist(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseChecklists
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseChecklists.Add(new CaseChecklist
            {
                CaseId = newCaseId,
                ChecklistTemplateId = item.ChecklistTemplateId,
                Checked = item.Checked,
                Date = item.Date,
                Comments = item.Comments,
                NotApplicable = item.NotApplicable,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyNotes(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseNotes
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseNotes.Add(new CaseNote
            {
                CaseId = newCaseId,
                CaseNote1 = item.CaseNote1,
                DateCreated = DateTime.UtcNow,
                InterimAmount = item.InterimAmount,
                CaseNumber = item.CaseNumber,
                InterimHospital = item.InterimHospital,
                InterimRadiology = item.InterimRadiology,
                InterimDialysis = item.InterimDialysis,
                InterimSpecialist = item.InterimSpecialist,
                InterimPhysio = item.InterimPhysio,
                InterimTransport = item.InterimTransport,
                InterimAccomodation = item.InterimAccomodation,
                InterimScript = item.InterimScript,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyComments(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseComments
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseComments.Add(new CaseComment
            {
                CaseId = newCaseId,
                CaseComment1 = item.CaseComment1,
                DateCreated = DateTime.UtcNow,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyNappiCodes(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var items = await _context.CaseNappiCodes
            .AsNoTracking()
            .Where(x => x.CaseId == sourceCaseId && x.DateDeleted == null)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            _context.CaseNappiCodes.Add(new CaseNappiCode
            {
                CaseId = newCaseId,
                NappiId = item.NappiId,
                Value = item.Value,
                Quantity = item.Quantity,
                Dispensary = item.Dispensary,
                Ward = item.Ward,
                Theater = item.Theater,
                Tto = item.Tto,
                _0201 = item._0201,
                Date = item.Date,
            });
        }

        if (items.Any())
            await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CopyLetterNotes(int sourceCaseId, int newCaseId, CancellationToken cancellationToken)
    {
        var item = await _context.CaseLetterNotes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CaseId == sourceCaseId && x.DateDeleted == null, cancellationToken);

        if (item != null)
        {
            _context.CaseLetterNotes.Add(new CaseLetterNote
            {
                CaseId = newCaseId,
                Note = item.Note,
                IncludeDischargeForm = item.IncludeDischargeForm,
                IncludeReferralLetter = item.IncludeReferralLetter,
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
