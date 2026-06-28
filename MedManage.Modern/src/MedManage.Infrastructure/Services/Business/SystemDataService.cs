using Microsoft.EntityFrameworkCore;
using MedManage.Core.DTOs.Admin;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using MedManage.Infrastructure.Persistence;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Service for managing system configuration data
/// </summary>
public class SystemDataService : ISystemDataService
{
    private readonly MedManageDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SystemDataService(MedManageDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<SystemDataDto?> GetAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _context.SystemData.FirstOrDefaultAsync(cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<SystemDataDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.SystemData.FindAsync(new object[] { id }, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<SystemDataDto> CreateAsync(CreateSystemDataRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new SystemDatum
        {
            SystemCountryId = request.SystemCountryId,
            SystemEmailAddress = request.SystemEmailAddress,
            Smtpserver = request.SmtpServer,
            Ssl = request.Ssl,
            Username = request.Username,
            Password = request.Password,
            SpecialIcu = request.SpecialIcu,
            Icu = request.Icu,
            HighCare = request.HighCare,
            NeuroWard = request.NeuroWard,
            InIsolation = request.InIsolation,
            GeneralWard = request.GeneralWard,
            Paediatric = request.Paediatric,
            Maternity = request.Maternity,
            DayCase = request.DayCase,
            StepDown = request.StepDown,
            Psychiatric = request.Psychiatric,
            Address1 = request.Address1,
            Address2 = request.Address2,
            Address3 = request.Address3,
            Address4 = request.Address4,
            AddressCode = request.AddressCode,
            Email = request.Email,
            Fax = request.Fax,
            Telephone = request.Telephone,
            Website = request.Website,
            DefaultProviderId = request.DefaultProviderId,
            SystemUniqueIdentifier = Guid.NewGuid()
        };

        _context.SystemData.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(entity);
    }

    public async Task<SystemDataDto> UpdateAsync(UpdateSystemDataRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _context.SystemData.FindAsync(new object[] { request.SystemDataId }, cancellationToken);
        if (entity == null)
        {
            throw new KeyNotFoundException($"System data with ID {request.SystemDataId} not found");
        }

        entity.SystemCountryId = request.SystemCountryId;
        entity.SystemEmailAddress = request.SystemEmailAddress;
        entity.Smtpserver = request.SmtpServer;
        entity.Ssl = request.Ssl;
        entity.Username = request.Username;
        if (!string.IsNullOrEmpty(request.Password))
        {
            entity.Password = request.Password;
        }
        entity.SpecialIcu = request.SpecialIcu;
        entity.Icu = request.Icu;
        entity.HighCare = request.HighCare;
        entity.NeuroWard = request.NeuroWard;
        entity.InIsolation = request.InIsolation;
        entity.GeneralWard = request.GeneralWard;
        entity.Paediatric = request.Paediatric;
        entity.Maternity = request.Maternity;
        entity.DayCase = request.DayCase;
        entity.StepDown = request.StepDown;
        entity.Psychiatric = request.Psychiatric;
        entity.Address1 = request.Address1;
        entity.Address2 = request.Address2;
        entity.Address3 = request.Address3;
        entity.Address4 = request.Address4;
        entity.AddressCode = request.AddressCode;
        entity.Email = request.Email;
        entity.Fax = request.Fax;
        entity.Telephone = request.Telephone;
        entity.Website = request.Website;
        entity.DefaultProviderId = request.DefaultProviderId;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.SystemData.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;

        _context.SystemData.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> UpdateLogoAsync(int id, byte[] logoData, CancellationToken cancellationToken = default)
    {
        var entity = await _context.SystemData.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;

        entity.Logo = logoData;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<byte[]?> GetLogoAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.SystemData.FindAsync(new object[] { id }, cancellationToken);
        return entity?.Logo;
    }

    private static SystemDataDto MapToDto(SystemDatum entity) => new()
    {
        SystemDataId = entity.SystemDataId,
        SystemCountryId = entity.SystemCountryId,
        SystemUniqueIdentifier = entity.SystemUniqueIdentifier,
        SystemEmailAddress = entity.SystemEmailAddress,
        SmtpServer = entity.Smtpserver,
        Ssl = entity.Ssl,
        Username = entity.Username,
        SpecialIcu = entity.SpecialIcu,
        Icu = entity.Icu,
        HighCare = entity.HighCare,
        NeuroWard = entity.NeuroWard,
        InIsolation = entity.InIsolation,
        GeneralWard = entity.GeneralWard,
        Paediatric = entity.Paediatric,
        Maternity = entity.Maternity,
        DayCase = entity.DayCase,
        StepDown = entity.StepDown,
        Psychiatric = entity.Psychiatric,
        Address1 = entity.Address1,
        Address2 = entity.Address2,
        Address3 = entity.Address3,
        Address4 = entity.Address4,
        AddressCode = entity.AddressCode,
        Email = entity.Email,
        Fax = entity.Fax,
        Telephone = entity.Telephone,
        Website = entity.Website,
        HasLogo = entity.Logo != null && entity.Logo.Length > 0,
        DefaultProviderId = entity.DefaultProviderId
    };
}
