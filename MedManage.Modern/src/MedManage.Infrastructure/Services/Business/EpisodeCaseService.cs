using MedManage.Infrastructure.Mapping.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedManage.Core.DTOs.EpisodeCase;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class EpisodeCaseService : IEpisodeCaseService
{
    private readonly IUnitOfWork _unitOfWork;

    public EpisodeCaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EpisodeCaseDto>> GetAllAsync()
    {
        var episodeCases = await _unitOfWork.EpisodeCases
            .FindAsync(e => e.DateDeleted == null);
        return episodeCases.OrderByDescending(e => e.DateInserted).Select(e => e.ToDto());
    }

    public async Task<EpisodeCaseDto?> GetByIdAsync(int episodeId, int caseId)
    {
        var episodeCase = (await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.CaseId == caseId && e.DateDeleted == null))
            .FirstOrDefault();

        return episodeCase == null ? null : episodeCase.ToDto();
    }

    public async Task<IEnumerable<EpisodeCaseDto>> GetByEpisodeIdAsync(int episodeId)
    {
        var episodeCases = await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.DateDeleted == null);
        return episodeCases.OrderByDescending(e => e.DateInserted).Select(e => e.ToDto());
    }

    public async Task<IEnumerable<EpisodeCaseDto>> GetByCaseIdAsync(int caseId)
    {
        var episodeCases = await _unitOfWork.EpisodeCases
            .FindAsync(e => e.CaseId == caseId && e.DateDeleted == null);
        return episodeCases.OrderByDescending(e => e.DateInserted).Select(e => e.ToDto());
    }

    public async Task<EpisodeCaseDto> CreateAsync(CreateEpisodeCaseDto dto)
    {
        var episodeCase = dto.ToEntity();
        episodeCase.DateInserted = DateTime.Now;

        await _unitOfWork.EpisodeCases.AddAsync(episodeCase);
        await _unitOfWork.SaveChangesAsync();

        return episodeCase.ToDto();
    }

    public async Task<EpisodeCaseDto> UpdateAsync(int episodeId, int caseId, UpdateEpisodeCaseDto dto)
    {
        var episodeCase = (await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.CaseId == caseId && e.DateDeleted == null))
            .FirstOrDefault();

        if (episodeCase == null)
            throw new KeyNotFoundException($"EpisodeCase with EpisodeId {episodeId} and CaseId {caseId} not found");

        dto.ApplyTo(episodeCase);
        episodeCase.DateUpdated = DateTime.Now;

        await _unitOfWork.EpisodeCases.UpdateAsync(episodeCase);
        await _unitOfWork.SaveChangesAsync();

        return episodeCase.ToDto();
    }

    public async Task<bool> DeleteAsync(int episodeId, int caseId)
    {
        var episodeCase = (await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.CaseId == caseId && e.DateDeleted == null))
            .FirstOrDefault();

        if (episodeCase == null)
            return false;

        episodeCase.DateDeleted = DateTime.Now;
        await _unitOfWork.EpisodeCases.UpdateAsync(episodeCase);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
