using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedManage.Core.DTOs.Tariff;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class TariffService : ITariffService
{
    private readonly MedManageDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TariffService(MedManageDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // --- SP-wrapped tariff lookup ---

    public async Task<TariffLookupResult?> LookupTariffAsync(TariffLookupRequest request)
    {
        var tariffCodeParam = new SqlParameter("@TariffCode", request.TariffCode);
        var providerIdParam = new SqlParameter("@ProviderID", request.ProviderId);
        var treatmentDateParam = new SqlParameter("@TreatmentDate", request.TreatmentDate.ToDateTime(TimeOnly.MinValue));

        var results = await _context.Database
            .SqlQueryRaw<TariffLookupResult>(
                "EXEC Tariff.usp_TariffLookup @TariffCode, @ProviderID, @TreatmentDate",
                tariffCodeParam, providerIdParam, treatmentDateParam)
            .ToListAsync();

        return results.FirstOrDefault();
    }

    // --- SP-wrapped case tariff calculation (fn_sc_TotalTariffPerCase) ---

    public async Task<IEnumerable<CaseTariffCalculationResult>> CalculateCaseTariffAsync(int caseId)
    {
        var caseIdParam = new SqlParameter("@CaseID", caseId);

        var results = await _context.Database
            .SqlQueryRaw<CaseTariffCalculationResult>(
                "SELECT * FROM Tariff.fn_sc_TotalTariffPerCase(@CaseID)",
                caseIdParam)
            .ToListAsync();

        return results;
    }

    // --- Base Tariff CRUD (standard EF Core LINQ) ---

    public async Task<IEnumerable<BaseTariffDto>> GetAllBaseTariffsAsync()
    {
        var tariffs = await _unitOfWork.BaseTariffs
            .FindAsync(bt => bt.DateDeleted == null);
        return _mapper.Map<IEnumerable<BaseTariffDto>>(tariffs.OrderBy(bt => bt.TariffDescription));
    }

    public async Task<BaseTariffDto?> GetBaseTariffByIdAsync(string id)
    {
        var tariffs = await _unitOfWork.BaseTariffs
            .FindAsync(bt => bt.BaseTariffId == id && bt.DateDeleted == null);
        var tariff = tariffs.FirstOrDefault();
        if (tariff == null) return null;
        return _mapper.Map<BaseTariffDto>(tariff);
    }

    public async Task<BaseTariffDto> CreateBaseTariffAsync(CreateBaseTariffDto dto)
    {
        var tariff = _mapper.Map<BaseTariff>(dto);
        tariff.DateInserted = DateTime.Now;

        await _unitOfWork.BaseTariffs.AddAsync(tariff);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BaseTariffDto>(tariff);
    }

    public async Task<BaseTariffDto> UpdateBaseTariffAsync(string id, UpdateBaseTariffDto dto)
    {
        var tariffs = await _unitOfWork.BaseTariffs
            .FindAsync(bt => bt.BaseTariffId == id && bt.DateDeleted == null);
        var tariff = tariffs.FirstOrDefault();
        if (tariff == null)
            throw new KeyNotFoundException($"BaseTariff with ID {id} not found");

        _mapper.Map(dto, tariff);
        tariff.DateUpdated = DateTime.Now;

        await _unitOfWork.BaseTariffs.UpdateAsync(tariff);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BaseTariffDto>(tariff);
    }

    public async Task<bool> DeleteBaseTariffAsync(string id)
    {
        var tariffs = await _unitOfWork.BaseTariffs
            .FindAsync(bt => bt.BaseTariffId == id && bt.DateDeleted == null);
        var tariff = tariffs.FirstOrDefault();
        if (tariff == null) return false;

        tariff.DateDeleted = DateTime.Now;
        await _unitOfWork.BaseTariffs.UpdateAsync(tariff);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    // --- Tariff Rate/Period CRUD ---

    public async Task<IEnumerable<TariffRateDto>> GetAllTariffRatesAsync()
    {
        var rates = await _context.Tariffs
            .Where(t => t.DateDeleted == null)
            .OrderBy(t => t.BaseTariffId)
            .ThenBy(t => t.StartDate)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TariffRateDto>>(rates);
    }

    public async Task<IEnumerable<TariffRateDto>> GetTariffRatesByBaseTariffIdAsync(string baseTariffId)
    {
        var rates = await _context.Tariffs
            .Where(t => t.BaseTariffId == baseTariffId && t.DateDeleted == null)
            .OrderBy(t => t.StartDate)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TariffRateDto>>(rates);
    }

    public async Task<TariffRateDto?> GetTariffRateByIdAsync(int id)
    {
        var rate = await _context.Tariffs
            .FirstOrDefaultAsync(t => t.TariffId == id && t.DateDeleted == null);
        if (rate == null) return null;
        return _mapper.Map<TariffRateDto>(rate);
    }

    public async Task<TariffRateDto> CreateTariffRateAsync(CreateTariffRateDto dto)
    {
        var rate = _mapper.Map<Tariff>(dto);
        rate.DateInserted = DateTime.Now;

        await _context.Tariffs.AddAsync(rate);
        await _context.SaveChangesAsync();

        return _mapper.Map<TariffRateDto>(rate);
    }

    public async Task<TariffRateDto> UpdateTariffRateAsync(int id, UpdateTariffRateDto dto)
    {
        var rate = await _context.Tariffs
            .FirstOrDefaultAsync(t => t.TariffId == id && t.DateDeleted == null);
        if (rate == null)
            throw new KeyNotFoundException($"Tariff rate with ID {id} not found");

        _mapper.Map(dto, rate);
        rate.DateUpdated = DateTime.Now;

        _context.Tariffs.Update(rate);
        await _context.SaveChangesAsync();

        return _mapper.Map<TariffRateDto>(rate);
    }

    public async Task<bool> DeleteTariffRateAsync(int id)
    {
        var rate = await _context.Tariffs
            .FirstOrDefaultAsync(t => t.TariffId == id && t.DateDeleted == null);
        if (rate == null) return false;

        rate.DateDeleted = DateTime.Now;
        _context.Tariffs.Update(rate);
        await _context.SaveChangesAsync();

        return true;
    }

    // --- Tariff Name CRUD ---

    public async Task<IEnumerable<TariffNameDto>> GetAllTariffNamesAsync()
    {
        var names = await _context.TariffNames
            .Where(tn => tn.DateDeleted == null)
            .OrderBy(tn => tn.TariffName1)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TariffNameDto>>(names);
    }

    public async Task<TariffNameDto?> GetTariffNameByIdAsync(int id)
    {
        var name = await _context.TariffNames
            .FirstOrDefaultAsync(tn => tn.TariffNameId == id && tn.DateDeleted == null);
        if (name == null) return null;
        return _mapper.Map<TariffNameDto>(name);
    }

    public async Task<TariffNameDto> CreateTariffNameAsync(CreateTariffNameDto dto)
    {
        var name = _mapper.Map<TariffName>(dto);
        name.DateInserted = DateTime.Now;

        await _context.TariffNames.AddAsync(name);
        await _context.SaveChangesAsync();

        return _mapper.Map<TariffNameDto>(name);
    }

    public async Task<TariffNameDto> UpdateTariffNameAsync(int id, UpdateTariffNameDto dto)
    {
        var name = await _context.TariffNames
            .FirstOrDefaultAsync(tn => tn.TariffNameId == id && tn.DateDeleted == null);
        if (name == null)
            throw new KeyNotFoundException($"TariffName with ID {id} not found");

        _mapper.Map(dto, name);
        name.DateUpdated = DateTime.Now;

        _context.TariffNames.Update(name);
        await _context.SaveChangesAsync();

        return _mapper.Map<TariffNameDto>(name);
    }

    public async Task<bool> DeleteTariffNameAsync(int id)
    {
        var name = await _context.TariffNames
            .FirstOrDefaultAsync(tn => tn.TariffNameId == id && tn.DateDeleted == null);
        if (name == null) return false;

        name.DateDeleted = DateTime.Now;
        _context.TariffNames.Update(name);
        await _context.SaveChangesAsync();

        return true;
    }
}
