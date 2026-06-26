using MedManage.Infrastructure.Mapping.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedManage.Core.DTOs.CaseBilling;
using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using MedManage.Core.Interfaces.Services;

namespace MedManage.Infrastructure.Services.Business;

public class CaseDiscountService : ICaseDiscountService
{
    private readonly IUnitOfWork _unitOfWork;

    public CaseDiscountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CaseDiscountDto>> GetByCaseIdAsync(int caseId, CancellationToken cancellationToken = default)
    {
        var discounts = await _unitOfWork.CaseDiscounts.GetByCaseIdAsync(caseId);
        return discounts.Select(e => e.ToDto());
    }

    public async Task<CaseDiscountDto> CreateAsync(int caseId, CreateCaseDiscountDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new CaseDiscount
        {
            CaseId = caseId,
            Discount = dto.Discount,
            DateInserted = DateTime.Now
        };

        await _unitOfWork.CaseDiscounts.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int caseId, decimal discount, CancellationToken cancellationToken = default)
    {
        var discounts = await _unitOfWork.CaseDiscounts.GetByCaseIdAsync(caseId);
        var target = discounts.FirstOrDefault(d => d.Discount == discount);

        if (target == null)
            return false;

        // Soft delete
        target.DateDeleted = DateTime.Now;
        await _unitOfWork.CaseDiscounts.UpdateAsync(target);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
