using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseLetterNoteRepository : IRepository<CaseLetterNote>
{
    Task<IEnumerable<CaseLetterNote>> GetByCaseIdAsync(int caseId);
}
