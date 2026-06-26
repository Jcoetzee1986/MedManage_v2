using MedManage.Infrastructure.Mapping.Manual;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseLinkedFile;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseLinkedFileService : ICaseLinkedFileService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseLinkedFileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseLinkedFileDto>> GetAllAsync()
    {
        var files = await _unitOfWork.CaseLinkedFiles.GetAllAsync();
        return files.Select(e => e.ToDto());
    }

    public async Task<CaseLinkedFileDto?> GetByIdAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        return file == null ? null : file.ToDto();
    }

    public async Task<IEnumerable<CaseLinkedFileDto>> GetByCaseIdAsync(int caseId)
    {
        var files = await _unitOfWork.CaseLinkedFiles
            .FindAsync(f => f.CaseId == caseId);
        
        var sortedFiles = files.OrderByDescending(f => f.DateAdded);
        return sortedFiles.Select(e => e.ToDto());
    }

    public async Task<IEnumerable<CaseLinkedFileDto>> GetByMemberIdAsync(int memberId)
    {
        var files = await _unitOfWork.CaseLinkedFiles
            .FindAsync(f => f.MemberId == memberId);
        
        var sortedFiles = files.OrderByDescending(f => f.DateAdded);
        return sortedFiles.Select(e => e.ToDto());
    }

    public async Task<CaseLinkedFileDto> CreateAsync(CreateCaseLinkedFileDto dto)
    {
        var file = dto.ToEntity();
        file.DateInserted = DateTime.UtcNow;
        
        await _unitOfWork.CaseLinkedFiles.AddAsync(file);
        await _unitOfWork.SaveChangesAsync();
        
        return file.ToDto();
    }

    public async Task<CaseLinkedFileDto> UpdateAsync(int id, UpdateCaseLinkedFileDto dto)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        if (file == null)
            throw new KeyNotFoundException($"CaseLinkedFile with ID {id} not found");

        dto.ApplyTo(file);
        file.DateUpdated = DateTime.UtcNow;
        
        await _unitOfWork.CaseLinkedFiles.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();
        
        return file.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        if (file == null)
            return false;

        file.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseLinkedFiles.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}
