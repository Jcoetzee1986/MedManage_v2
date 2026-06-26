using MedManage.Infrastructure.Mapping.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseChecklist;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseChecklistService : ICaseChecklistService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseChecklistService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseChecklistDto>> GetAllAsync()
    {
        var checklists = await _unitOfWork.CaseChecklists
            .FindAsync(c => c.DateDeleted == null);
        return checklists.OrderByDescending(c => c.DateInserted).Select(e => e.ToDto());
    }

    public async Task<CaseChecklistDto?> GetByIdAsync(int caseId, int checklistTemplateId)
    {
        var checklist = (await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.ChecklistTemplateId == checklistTemplateId && c.DateDeleted == null))
            .FirstOrDefault();

        return checklist == null ? null : checklist.ToDto();
    }

    public async Task<IEnumerable<CaseChecklistDto>> GetByCaseIdAsync(int caseId)
    {
        var checklists = await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.DateDeleted == null);
        return checklists.OrderByDescending(c => c.DateInserted).Select(e => e.ToDto());
    }

    public async Task<CaseChecklistDto> CreateAsync(CreateCaseChecklistDto dto)
    {
        var checklist = dto.ToEntity();
        checklist.DateInserted = DateTime.Now;

        await _unitOfWork.CaseChecklists.AddAsync(checklist);
        await _unitOfWork.SaveChangesAsync();

        return checklist.ToDto();
    }

    public async Task<CaseChecklistDto> UpdateAsync(int caseId, int checklistTemplateId, UpdateCaseChecklistDto dto)
    {
        var checklist = (await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.ChecklistTemplateId == checklistTemplateId && c.DateDeleted == null))
            .FirstOrDefault();

        if (checklist == null)
            throw new KeyNotFoundException($"CaseChecklist with CaseId {caseId} and ChecklistTemplateId {checklistTemplateId} not found");

        dto.ApplyTo(checklist);
        checklist.DateUpdated = DateTime.Now;

        await _unitOfWork.CaseChecklists.UpdateAsync(checklist);
        await _unitOfWork.SaveChangesAsync();

        return checklist.ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, int checklistTemplateId)
    {
        var checklist = (await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.ChecklistTemplateId == checklistTemplateId && c.DateDeleted == null))
            .FirstOrDefault();

        if (checklist == null)
            return false;

        checklist.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseChecklists.UpdateAsync(checklist);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
