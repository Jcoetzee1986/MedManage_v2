using MedManage.Core.DTOs.CaseBilling;

namespace MedManage.Core.Interfaces.Services;

public interface ICaseBillingCommentService
{
    Task<IEnumerable<CaseBillingCommentDto>> GetByBillingIdAsync(int billingId);
    Task<CaseBillingCommentDto> CreateAsync(int billingId, CreateCaseBillingCommentDto dto);
    Task<bool> DeleteAsync(int id);
}
