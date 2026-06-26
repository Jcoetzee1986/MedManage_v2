using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedManage.Core.DTOs.Admin;
using MedManage.Core.DTOs.Common;
using MedManage.Core.Interfaces.Services;

namespace MedManage.API.Controllers;

/// <summary>
/// Admin API for managing active editing sessions
/// </summary>
[ApiController]
[Route("api/admin/sessions")]
[Authorize(Roles = "System Administrator")]
public class SessionAdminController : ControllerBase
{
    private readonly ISessionAdminService _service;
    private readonly ILogger<SessionAdminController> _logger;

    public SessionAdminController(ISessionAdminService service, ILogger<SessionAdminController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// List all active sessions (who is editing what case)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CaseEditingSessionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActiveSessions()
    {
        try
        {
            var sessions = await _service.GetAllActiveSessionsAsync();
            return Ok(ApiResponse<IEnumerable<CaseEditingSessionDto>>.SuccessResponse(sessions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active sessions");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving active sessions"));
        }
    }

    /// <summary>
    /// Force-terminate a session (admin only)
    /// </summary>
    [HttpDelete("{sessionId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TerminateSession(int sessionId)
    {
        try
        {
            var result = await _service.TerminateSessionAsync(sessionId);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse($"Session for case {sessionId} not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Session terminated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error terminating session {SessionId}", sessionId);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while terminating the session"));
        }
    }
}
