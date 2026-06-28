using FluentValidation;
using MedManage.Core.DTOs.ServiceProvider;

namespace MedManage.Core.Validators;

public class CreateServiceProviderRequestValidator : AbstractValidator<CreateServiceProviderRequest>
{
    public CreateServiceProviderRequestValidator()
    {
        RuleFor(x => x.ServiceProviderName)
            .MaximumLength(300)
            .WithMessage("Service provider name cannot exceed 300 characters");

        RuleFor(x => x.ServiceProviderSurname)
            .MaximumLength(300)
            .WithMessage("Service provider surname cannot exceed 300 characters");

        RuleFor(x => x.ServiceProviderInitials)
            .MaximumLength(10)
            .WithMessage("Service provider initials cannot exceed 10 characters");

        RuleFor(x => x.PracticeName)
            .MaximumLength(200)
            .WithMessage("Practice name cannot exceed 200 characters");

        RuleFor(x => x.GroupPracticeNr)
            .MaximumLength(300)
            .WithMessage("Group practice number cannot exceed 300 characters");

        RuleFor(x => x.PracticeNr)
            .MaximumLength(300)
            .WithMessage("Practice number cannot exceed 300 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(100)
            .WithMessage("Phone number cannot exceed 100 characters");

        RuleFor(x => x.FaxNumber)
            .MaximumLength(100)
            .WithMessage("Fax number cannot exceed 100 characters");

        RuleFor(x => x.EmailAddress)
            .MaximumLength(300)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.EmailAddress))
            .WithMessage("Email address must be a valid email format");

        RuleFor(x => x.CellNumber)
            .MaximumLength(50)
            .WithMessage("Cell number cannot exceed 50 characters");

        RuleFor(x => x.NoOfPartners)
            .GreaterThanOrEqualTo(0)
            .When(x => x.NoOfPartners.HasValue)
            .WithMessage("Number of partners cannot be negative");
    }
}
