using MedManage.Infrastructure.Mapping.Manual;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

    public CaseTariffService(IUnitOfWork unitOfWork, MedManageDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CaseTariffDto>> GetByCaseIdAsync(int caseId)
    {
        // Call the existing stored procedure which calculates agreed rates, discounts, penalties
        var connection = _dbContext.Database.GetDbConnection();
        var results = new List<CaseTariffDto>();

        try
        {
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "Tariff.usp_Case_Tariff_Select";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CaseID", caseId));

            using var reader = await command.ExecuteReaderAsync();
            // The SP returns multiple result sets — the first one (#TariffFinal) has the calculated values
            while (await reader.ReadAsync())
            {
                results.Add(new CaseTariffDto
                {
                    CaseIdTariffId = reader.IsDBNull("Seq") ? 0 : Convert.ToInt64(reader["Seq"]),
                    CaseId = caseId,
                    TariffId = reader.IsDBNull("TariffID") ? 0 : Convert.ToInt32(reader["TariffID"]),
                    TariffCode = reader.IsDBNull("TariffCode") ? null : reader["TariffCode"].ToString(),
                    TariffDescription = reader.IsDBNull("TariffDescription") ? null : reader["TariffDescription"].ToString(),
                    Value = reader.IsDBNull("Value") ? null : Convert.ToDecimal(reader["Value"]),
                    Qty = reader.IsDBNull("Qty") ? null : Convert.ToDecimal(reader["Qty"]),
                    AgreedRateOverride = reader.IsDBNull("AgreedRateOverride") ? null : Convert.ToDecimal(reader["AgreedRateOverride"]),
                    ValueIsTotal = reader.IsDBNull("ValueIsTotal") ? null : Convert.ToBoolean(reader["ValueIsTotal"]),
                    Rejected = reader.IsDBNull("Rejected") ? null : Convert.ToBoolean(reader["Rejected"]),
                    DateOfProcedure = reader.IsDBNull("DateOfProcedure") ? default : DateOnly.FromDateTime(Convert.ToDateTime(reader["DateOfProcedure"])),
                    // Calculated fields from SP
                    FullValue = reader.IsDBNull("FullValue") ? null : Convert.ToDecimal(reader["FullValue"]),
                    AgreedRate = reader.IsDBNull("AgreedRate") ? null : Convert.ToDecimal(reader["AgreedRate"]),
                    Discount = reader.IsDBNull("Discount") ? null : Convert.ToDecimal(reader["Discount"]),
                    TotalOvercharged = reader.IsDBNull("TotalOvercharged") ? null : Convert.ToDecimal(reader["TotalOvercharged"]),
                    TotalPayable = reader.IsDBNull("TotalPayable") ? null : Convert.ToDecimal(reader["TotalPayable"]),
                    TotalPenalty = reader.IsDBNull("TotalPenalty") ? null : Convert.ToDecimal(reader["TotalPenalty"]),
                    PenaltyPercentage = reader.IsDBNull("PenaltyPercentage") ? null : Convert.ToDecimal(reader["PenaltyPercentage"]),
                    Colour = reader.IsDBNull("Colour") ? "White" : reader["Colour"].ToString(),
                });
            }
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }

        return results;
    }

    public async Task<CaseTariffDto?> GetByIdAsync(int caseId, long caseIdTariffId)
    {
        var tariff = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == caseIdTariffId && t.DateDeleted == null))
            .FirstOrDefault();

        return tariff == null ? null : tariff.ToDto();
    }

    public async Task<CaseTariffDto> CreateAsync(int caseId, CreateCaseTariffRequest dto)
    {
        var entity = dto.ToEntity();
        entity.CaseId = caseId;

        await _unitOfWork.CaseTariffs.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<CaseTariffDto> UpdateAsync(int caseId, long id, UpdateCaseTariffRequest dto)
    {
        var entity = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == id && t.DateDeleted == null))
            .FirstOrDefault();

        if (entity == null)
            throw new KeyNotFoundException($"Case tariff with ID {id} not found for case {caseId}");

        dto.ApplyTo(entity);

        await _unitOfWork.CaseTariffs.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, long id)
    {
        var entity = (await _unitOfWork.CaseTariffs
            .FindAsync(t => t.CaseId == caseId && t.CaseIdTariffId == id && t.DateDeleted == null))
            .FirstOrDefault();

        if (entity == null) return false;

        entity.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseTariffs.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
