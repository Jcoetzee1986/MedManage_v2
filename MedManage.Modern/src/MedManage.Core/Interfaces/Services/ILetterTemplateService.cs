using MedManage.Core.Entities;

namespace MedManage.Core.Interfaces.Services;

public interface ILetterTemplateService
{
    Task<IEnumerable<LetterTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LetterTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<LetterTemplate?> GetForCaseAsync(int caseId, string templateType = "CaseLetter", CancellationToken cancellationToken = default);
    Task<LetterTemplate> CreateAsync(LetterTemplate template, CancellationToken cancellationToken = default);
    Task<LetterTemplate> UpdateAsync(LetterTemplate template, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<byte[]> RenderCaseLetterAsync(int caseId, CancellationToken cancellationToken = default);
}
