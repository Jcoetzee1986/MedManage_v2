using MedManage.Core.DTOs.Tariff;

namespace MedManage.Core.Interfaces.Services;

public interface IMainClientTariffService
{
    Task<IEnumerable<MainClientTariffDto>> GetByMainClientIdAsync(int mainClientId);
    Task<MainClientTariffDto> CreateAsync(CreateMainClientTariffDto dto);
    Task<bool> DeleteAsync(int mainClientId, int tariffNameId);
}
