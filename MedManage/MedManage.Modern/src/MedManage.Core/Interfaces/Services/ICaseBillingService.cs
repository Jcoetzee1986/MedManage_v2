using System.Collections.Generic;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseBillingService
{
    Task<IEnumerable<CaseBillingDto>> GetAllAsync();
    Task<CaseBillingDto?> GetByIdAsync(int id);
    Task<IEnumerable<CaseBillingDto>> GetByCaseIdAsync(int caseId);
    Task<CaseBillingDto> CreateAsync(CreateCaseBillingDto dto);
    Task<CaseBillingDto> UpdateAsync(int id, UpdateCaseBillingDto dto);
    Task<bool> DeleteAsync(int id);
}
