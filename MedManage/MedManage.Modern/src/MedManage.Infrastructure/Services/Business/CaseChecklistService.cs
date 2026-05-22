using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedManage.Core.DTOs.CaseChecklist;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseChecklistService : ICaseChecklistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseChecklistService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseChecklistDto>> GetAllAsync()
    {
        var checklists = await _unitOfWork.CaseChecklists
            .FindAsync(c => c.DateDeleted == null);
        return _mapper.Map<IEnumerable<CaseChecklistDto>>(checklists.OrderByDescending(c => c.DateInserted));
    }

    public async Task<CaseChecklistDto?> GetByIdAsync(int caseId, int checklistTemplateId)
    {
        var checklist = (await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.ChecklistTemplateId == checklistTemplateId && c.DateDeleted == null))
            .FirstOrDefault();

        return checklist == null ? null : _mapper.Map<CaseChecklistDto>(checklist);
    }

    public async Task<IEnumerable<CaseChecklistDto>> GetByCaseIdAsync(int caseId)
    {
        var checklists = await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.DateDeleted == null);
        return _mapper.Map<IEnumerable<CaseChecklistDto>>(checklists.OrderByDescending(c => c.DateInserted));
    }

    public async Task<CaseChecklistDto> CreateAsync(CreateCaseChecklistDto dto)
    {
        var checklist = _mapper.Map<CaseChecklist>(dto);
        checklist.DateInserted = DateTime.Now;

        await _unitOfWork.CaseChecklists.AddAsync(checklist);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseChecklistDto>(checklist);
    }

    public async Task<CaseChecklistDto> UpdateAsync(int caseId, int checklistTemplateId, UpdateCaseChecklistDto dto)
    {
        var checklist = (await _unitOfWork.CaseChecklists
            .FindAsync(c => c.CaseId == caseId && c.ChecklistTemplateId == checklistTemplateId && c.DateDeleted == null))
            .FirstOrDefault();

        if (checklist == null)
            throw new KeyNotFoundException($"CaseChecklist with CaseId {caseId} and ChecklistTemplateId {checklistTemplateId} not found");

        _mapper.Map(dto, checklist);
        checklist.DateUpdated = DateTime.Now;

        await _unitOfWork.CaseChecklists.UpdateAsync(checklist);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseChecklistDto>(checklist);
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
