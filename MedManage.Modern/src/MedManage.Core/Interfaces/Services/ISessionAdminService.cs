using MedManage.Core.DTOs.Admin;

namespace MedManage.Core.Interfaces.Services;

public interface ISessionAdminService
{
    Task<IEnumerable<CaseEditingSessionDto>> GetAllActiveSessionsAsync();
    Task<bool> TerminateSessionAsync(int caseId);
}
