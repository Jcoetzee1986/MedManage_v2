using MedManage.Core.DTOs.ReferenceData;

namespace MedManage.Core.Interfaces.Services;

public interface IBillingStatusService
{
    Task<IEnumerable<BillingStatusDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<BillingStatusDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BillingStatusDto> CreateAsync(CreateBillingStatusDto dto, CancellationToken cancellationToken = default);
    Task<BillingStatusDto> UpdateAsync(UpdateBillingStatusDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
