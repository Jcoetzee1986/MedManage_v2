using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace MedManage.Infrastructure.Services.Business;

public class CaseBillingService : ICaseBillingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaseBillingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CaseBillingDto>> GetAllAsync()
    {
        var billings = await _unitOfWork.CaseBillings
            .FindAsync(c => c.DateDeleted == null);
        return _mapper.Map<IEnumerable<CaseBillingDto>>(billings.OrderByDescending(c => c.DateInserted));
    }

    public async Task<CaseBillingDto?> GetByIdAsync(int id)
    {
        var billing = await _unitOfWork.CaseBillings.GetByIdAsync(id);
        if (billing == null || billing.DateDeleted != null)
            return null;

        return _mapper.Map<CaseBillingDto>(billing);
    }

    public async Task<IEnumerable<CaseBillingDto>> GetByCaseIdAsync(int caseId)
    {
        var billings = await _unitOfWork.CaseBillings
            .FindAsync(c => c.CaseId == caseId && c.DateDeleted == null);
        return _mapper.Map<IEnumerable<CaseBillingDto>>(billings.OrderByDescending(c => c.DateInserted));
    }

    public async Task<CaseBillingDto> CreateAsync(CreateCaseBillingDto dto)
    {
        var billing = _mapper.Map<CaseBilling>(dto);
        billing.DateInserted = DateTime.Now;

        await _unitOfWork.CaseBillings.AddAsync(billing);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseBillingDto>(billing);
    }

    public async Task<CaseBillingDto> UpdateAsync(int id, UpdateCaseBillingDto dto)
    {
        var billing = await _unitOfWork.CaseBillings.GetByIdAsync(id);
        if (billing == null || billing.DateDeleted != null)
            throw new KeyNotFoundException($"CaseBilling with ID {id} not found");

        _mapper.Map(dto, billing);
        billing.DateUpdated = DateTime.Now;

        await _unitOfWork.CaseBillings.UpdateAsync(billing);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CaseBillingDto>(billing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var billing = await _unitOfWork.CaseBillings.GetByIdAsync(id);
        if (billing == null || billing.DateDeleted != null)
            return false;

        billing.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseBillings.UpdateAsync(billing);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
