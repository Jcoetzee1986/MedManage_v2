using MedManage.Core.DTOs.Tariff;

namespace MedManage.Core.Interfaces.Services;

public interface ITariffService
{
    // SP-wrapped tariff lookup
    Task<TariffLookupResult?> LookupTariffAsync(TariffLookupRequest request);

    // Simple text search on base tariffs (for autocomplete)
    Task<IEnumerable<BaseTariffDto>> SearchBaseTariffsAsync(string? code, string? description = null);

    // Search tariffs with case context (uses SP to calculate rates)
    Task<IEnumerable<TariffLookupResult>> LookupTariffForCaseAsync(int caseId, string tariffCode);

    // SP-wrapped case tariff calculation (fn_sc_TotalTariffPerCase)
    Task<IEnumerable<CaseTariffCalculationResult>> CalculateCaseTariffAsync(int caseId);

    // Base Tariff CRUD
    Task<IEnumerable<BaseTariffDto>> GetAllBaseTariffsAsync();
    Task<BaseTariffDto?> GetBaseTariffByIdAsync(string id);
    Task<BaseTariffDto> CreateBaseTariffAsync(CreateBaseTariffDto dto);
    Task<BaseTariffDto> UpdateBaseTariffAsync(string id, UpdateBaseTariffDto dto);
    Task<bool> DeleteBaseTariffAsync(string id);

    // Tariff Rate/Period CRUD
    Task<IEnumerable<TariffRateDto>> GetAllTariffRatesAsync();
    Task<IEnumerable<TariffRateDto>> GetTariffRatesByBaseTariffIdAsync(string baseTariffId);
    Task<TariffRateDto?> GetTariffRateByIdAsync(int id);
    Task<TariffRateDto> CreateTariffRateAsync(CreateTariffRateDto dto);
    Task<TariffRateDto> UpdateTariffRateAsync(int id, UpdateTariffRateDto dto);
    Task<bool> DeleteTariffRateAsync(int id);

    // Tariff Name CRUD
    Task<IEnumerable<TariffNameDto>> GetAllTariffNamesAsync();
    Task<TariffNameDto?> GetTariffNameByIdAsync(int id);
    Task<TariffNameDto> CreateTariffNameAsync(CreateTariffNameDto dto);
    Task<TariffNameDto> UpdateTariffNameAsync(int id, UpdateTariffNameDto dto);
    Task<bool> DeleteTariffNameAsync(int id);
}
