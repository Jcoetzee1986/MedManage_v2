using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.CaseTariff;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

public class CaseTariffService : ICaseTariffService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MedManageDbContext _dbContext;
    private readonly IMapper _mapper;

    public CaseTariffService(IUnitOfWork unitOfWork, MedManageDbContext dbContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseTariffDto>> GetByCaseIdAsync(int caseId)
    {
        var tariffs = await _unitOfWork.CaseTariffs.GetByCaseIdAsync(caseId);
        return _mapper.Map<IEnumerable<CaseTariffDto>>(tariffs);
    }

    public async Task<CaseTariffDto?> GetByIdAsync(int caseId, long caseIdTariffId)
    {
        var tariff = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == caseIdTariffId && t.DateDeleted == null))
            .FirstOrDefault();

        return tariff == null ? null : _mapper.Map<CaseTariffDto>(tariff);
    }

    public async Task<CaseTariffDto> CreateAsync(int caseId, CreateCaseTariffRequest request)
    {
        var parameters = new[]
        {
            new SqlParameter("@CaseID", caseId),
            new SqlParameter("@TariffID", request.TariffId),
            new SqlParameter("@Value", (object?)request.Value ?? DBNull.Value),
            new SqlParameter("@Qty", (object?)request.Qty ?? DBNull.Value),
            new SqlParameter("@AgreedRateOverride", (object?)request.AgreedRateOverride ?? DBNull.Value),
            new SqlParameter("@ValueIsTotal", (object?)request.ValueIsTotal ?? DBNull.Value),
            new SqlParameter("@Rejected", (object?)request.Rejected ?? DBNull.Value),
            new SqlParameter("@DateOfProcedure", request.DateOfProcedure.ToDateTime(TimeOnly.MinValue))
        };

        await _dbContext.CaseTariffs.FromSqlRaw(
            "EXEC Tariff.usp_Case_Tariff_Insert @CaseID, @TariffID, @Value, @Qty, @AgreedRateOverride, @ValueIsTotal, @Rejected, @DateOfProcedure",
            parameters).ToListAsync();

        // Retrieve the newly created tariff (latest by DateInserted for this case + tariff combo)
        var created = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.TariffId == request.TariffId && t.DateDeleted == null))
            .OrderByDescending(t => t.DateInserted)
            .FirstOrDefault();

        return _mapper.Map<CaseTariffDto>(created!);
    }

    public async Task<CaseTariffDto> UpdateAsync(int caseId, long caseIdTariffId, UpdateCaseTariffRequest request)
    {
        // Verify existence first
        var existing = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == caseIdTariffId && t.DateDeleted == null))
            .FirstOrDefault();

        if (existing == null)
            throw new KeyNotFoundException($"CaseTariff with CaseId {caseId} and Id {caseIdTariffId} not found");

        var parameters = new[]
        {
            new SqlParameter("@CaseID_TariffID", caseIdTariffId),
            new SqlParameter("@Value", (object?)request.Value ?? DBNull.Value),
            new SqlParameter("@Qty", (object?)request.Qty ?? DBNull.Value),
            new SqlParameter("@AgreedRateOverride", (object?)request.AgreedRateOverride ?? DBNull.Value),
            new SqlParameter("@ValueIsTotal", (object?)request.ValueIsTotal ?? DBNull.Value),
            new SqlParameter("@Rejected", (object?)request.Rejected ?? DBNull.Value),
            new SqlParameter("@DateOfProcedure", request.DateOfProcedure.ToDateTime(TimeOnly.MinValue))
        };

        await _dbContext.CaseTariffs.FromSqlRaw(
            "EXEC Tariff.usp_Case_Tariff_Update @CaseID_TariffID, @Value, @Qty, @AgreedRateOverride, @ValueIsTotal, @Rejected, @DateOfProcedure",
            parameters).ToListAsync();

        // Retrieve updated record
        var updated = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == caseIdTariffId && t.DateDeleted == null))
            .FirstOrDefault();

        return _mapper.Map<CaseTariffDto>(updated!);
    }

    public async Task<bool> DeleteAsync(int caseId, long caseIdTariffId)
    {
        var existing = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == caseIdTariffId && t.DateDeleted == null))
            .FirstOrDefault();

        if (existing == null)
            return false;

        var parameters = new[]
        {
            new SqlParameter("@CaseID_TariffID", caseIdTariffId)
        };

        await _dbContext.CaseTariffs.FromSqlRaw(
            "EXEC Tariff.usp_Case_Tariff_Delete @CaseID_TariffID",
            parameters).ToListAsync();

        return true;
    }
}
