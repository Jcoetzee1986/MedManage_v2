using System.Data.Common;
using MedManage.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedManage.Infrastructure.Persistence;

/// <summary>
/// EF Core interceptor that sets SESSION_CONTEXT('UserID') on every opened SQL connection.
/// This allows SQL audit triggers to read the authenticated user via SESSION_CONTEXT(N'UserID').
/// </summary>
public class SessionContextInterceptor : DbConnectionInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public SessionContextInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        SetSessionContext(connection);
        base.ConnectionOpened(connection, eventData);
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await SetSessionContextAsync(connection, cancellationToken);
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    private void SetSessionContext(DbConnection connection)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return;

        using var command = connection.CreateCommand();
        command.CommandText = "EXEC sp_set_session_context @key = N'UserID', @value = @userId";
        var param = command.CreateParameter();
        param.ParameterName = "@userId";
        param.Value = userId;
        command.Parameters.Add(param);
        command.ExecuteNonQuery();
    }

    private async Task SetSessionContextAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return;

        await using var command = connection.CreateCommand();
        command.CommandText = "EXEC sp_set_session_context @key = N'UserID', @value = @userId";
        var param = command.CreateParameter();
        param.ParameterName = "@userId";
        param.Value = userId;
        command.Parameters.Add(param);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
