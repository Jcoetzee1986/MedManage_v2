using FluentValidation;
using MedManage.Core.DTOs.Case;

namespace MedManage.Core.Validators;

public class CreateCaseRequestValidator : AbstractValidator<CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(x => x.AuthNumber)
            .MaximumLength(50)
            .WithMessage("Auth number cannot exceed 50 characters");

        RuleFor(x => x.AccountNr)
            .MaximumLength(50)
            .WithMessage("Account number cannot exceed 50 characters");

        RuleFor(x => x.FinalInvoiceAmountUpdated)
            .MaximumLength(100)
            .WithMessage("Final invoice amount updated cannot exceed 100 characters");

        RuleFor(x => x.TotalLengthOfStay)
            .GreaterThanOrEqualTo(0)
            .When(x => x.TotalLengthOfStay.HasValue)
            .WithMessage("Total length of stay cannot be negative");

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.TotalAmount.HasValue)
            .WithMessage("Total amount cannot be negative");

        RuleFor(x => x.FinalInvoiceAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.FinalInvoiceAmount.HasValue)
            .WithMessage("Final invoice amount cannot be negative");

        RuleFor(x => x.PenaltyPercentage)
            .InclusiveBetween(0, 100)
            .When(x => x.PenaltyPercentage.HasValue)
            .WithMessage("Penalty percentage must be between 0 and 100");

        RuleFor(x => x.DischargeDate)
            .GreaterThanOrEqualTo(x => x.AdmissionDate)
            .When(x => x.AdmissionDate.HasValue && x.DischargeDate.HasValue)
            .WithMessage("Discharge date must be on or after admission date");
    }
}
