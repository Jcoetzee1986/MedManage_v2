using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Repositories;

public interface ICaseCommentRepository : IRepository<CaseComment>
{
    Task<IEnumerable<CaseComment>> GetByCaseIdAsync(int caseId);
}
