using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseNoteRepository : IRepository<CaseNote>
{
    Task<IEnumerable<CaseNote>> GetByCaseIdAsync(int caseId);
    Task<CaseNote?> GetLastNoteByCaseIdAsync(int caseId);
}
