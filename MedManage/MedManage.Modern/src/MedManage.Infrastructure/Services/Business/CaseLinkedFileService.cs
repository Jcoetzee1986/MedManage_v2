using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseLinkedFile;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseLinkedFileService : ICaseLinkedFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseLinkedFileService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseLinkedFileDto>> GetAllAsync()
    {
        var files = await _unitOfWork.CaseLinkedFiles.GetAllAsync();
        return _mapper.Map<IEnumerable<CaseLinkedFileDto>>(files);
    }

    public async Task<CaseLinkedFileDto?> GetByIdAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        return file == null ? null : _mapper.Map<CaseLinkedFileDto>(file);
    }

    public async Task<IEnumerable<CaseLinkedFileDto>> GetByCaseIdAsync(int caseId)
    {
        var files = await _unitOfWork.CaseLinkedFiles
            .FindAsync(f => f.CaseId == caseId);
        
        var sortedFiles = files.OrderByDescending(f => f.DateAdded);
        return _mapper.Map<IEnumerable<CaseLinkedFileDto>>(sortedFiles);
    }

    public async Task<IEnumerable<CaseLinkedFileDto>> GetByMemberIdAsync(int memberId)
    {
        var files = await _unitOfWork.CaseLinkedFiles
            .FindAsync(f => f.MemberId == memberId);
        
        var sortedFiles = files.OrderByDescending(f => f.DateAdded);
        return _mapper.Map<IEnumerable<CaseLinkedFileDto>>(sortedFiles);
    }

    public async Task<CaseLinkedFileDto> CreateAsync(CreateCaseLinkedFileDto dto)
    {
        var file = _mapper.Map<CaseLinkedFile>(dto);
        file.DateInserted = DateTime.UtcNow;
        
        await _unitOfWork.CaseLinkedFiles.AddAsync(file);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CaseLinkedFileDto>(file);
    }

    public async Task<CaseLinkedFileDto> UpdateAsync(int id, UpdateCaseLinkedFileDto dto)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        if (file == null)
            throw new KeyNotFoundException($"CaseLinkedFile with ID {id} not found");

        _mapper.Map(dto, file);
        file.DateUpdated = DateTime.UtcNow;
        
        await _unitOfWork.CaseLinkedFiles.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CaseLinkedFileDto>(file);
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
