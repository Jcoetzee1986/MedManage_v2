using System.Collections.Generic;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseTariff;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseTariffService
{
    Task<IEnumerable<CaseTariffDto>> GetByCaseIdAsync(int caseId);
    Task<CaseTariffDto?> GetByIdAsync(int caseId, long caseIdTariffId);
    Task<CaseTariffDto> CreateAsync(int caseId, CreateCaseTariffRequest request);
    Task<CaseTariffDto> UpdateAsync(int caseId, long caseIdTariffId, UpdateCaseTariffRequest request);
    Task<bool> DeleteAsync(int caseId, long caseIdTariffId);
}
