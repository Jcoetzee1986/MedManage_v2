using MedManage.Core.DTOs.Admin;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class SessionAdminService : ISessionAdminService
{
    private readonly MedManageDbContext _context;

    public SessionAdminService(MedManageDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CaseEditingSessionDto>> GetAllActiveSessionsAsync()
    {
        var sessions = await _context.SessionUserCases
            .Select(s => new CaseEditingSessionDto
            {
                CaseId = s.CaseId,
                UserID = s.UserID,
                DateInserted = s.DateInserted
            })
            .ToListAsync();

        return sessions;
    }

    public async Task<bool> TerminateSessionAsync(int caseId)
    {
        var session = await _context.SessionUserCases
            .FirstOrDefaultAsync(s => s.CaseId == caseId);

        if (session == null)
            return false;

        _context.SessionUserCases.Remove(session);
        await _context.SaveChangesAsync();
        return true;
    }
}
