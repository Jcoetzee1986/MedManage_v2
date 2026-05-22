using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseLinkedFileRepository : IRepository<CaseLinkedFile>
{
    Task<IEnumerable<CaseLinkedFile>> GetByCaseIdAsync(int caseId);
    Task DeleteByCaseLinkedFileIdAsync(int caseLinkedFileId);
}
