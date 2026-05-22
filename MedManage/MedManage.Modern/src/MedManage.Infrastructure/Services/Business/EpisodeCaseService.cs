using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedManage.Core.DTOs.EpisodeCase;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class EpisodeCaseService : IEpisodeCaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EpisodeCaseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EpisodeCaseDto>> GetAllAsync()
    {
        var episodeCases = await _unitOfWork.EpisodeCases
            .FindAsync(e => e.DateDeleted == null);
        return _mapper.Map<IEnumerable<EpisodeCaseDto>>(episodeCases.OrderByDescending(e => e.DateInserted));
    }

    public async Task<EpisodeCaseDto?> GetByIdAsync(int episodeId, int caseId)
    {
        var episodeCase = (await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.CaseId == caseId && e.DateDeleted == null))
            .FirstOrDefault();

        return episodeCase == null ? null : _mapper.Map<EpisodeCaseDto>(episodeCase);
    }

    public async Task<IEnumerable<EpisodeCaseDto>> GetByEpisodeIdAsync(int episodeId)
    {
        var episodeCases = await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.DateDeleted == null);
        return _mapper.Map<IEnumerable<EpisodeCaseDto>>(episodeCases.OrderByDescending(e => e.DateInserted));
    }

    public async Task<IEnumerable<EpisodeCaseDto>> GetByCaseIdAsync(int caseId)
    {
        var episodeCases = await _unitOfWork.EpisodeCases
            .FindAsync(e => e.CaseId == caseId && e.DateDeleted == null);
        return _mapper.Map<IEnumerable<EpisodeCaseDto>>(episodeCases.OrderByDescending(e => e.DateInserted));
    }

    public async Task<EpisodeCaseDto> CreateAsync(CreateEpisodeCaseDto dto)
    {
        var episodeCase = _mapper.Map<EpisodeCase>(dto);
        episodeCase.DateInserted = DateTime.Now;

        await _unitOfWork.EpisodeCases.AddAsync(episodeCase);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EpisodeCaseDto>(episodeCase);
    }

    public async Task<EpisodeCaseDto> UpdateAsync(int episodeId, int caseId, UpdateEpisodeCaseDto dto)
    {
        var episodeCase = (await _unitOfWork.EpisodeCases
            .FindAsync(e => e.EpisodeId == episodeId && e.CaseId == caseId && e.DateDeleted == null))
            .FirstOrDefault();

        if (episodeCase == null)
            throw new KeyNotFoundException($"EpisodeCase with EpisodeId {episodeId} and CaseId {caseId} not found");

        _mapper.Map(dto, episodeCase);
        episodeCase.DateUpdated = DateTime.Now;

        await _unitOfWork.EpisodeCases.UpdateAsync(episodeCase);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EpisodeCaseDto>(episodeCase);
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
