using FluentValidation;
using MedManage.Core.DTOs.Member;

namespace MedManage.Core.Validators;

public class CreateMemberRequestValidator : AbstractValidator<CreateMemberRequest>
{
    public CreateMemberRequestValidator()
    {
        RuleFor(x => x.MemberNumber)
            .MaximumLength(200)
            .WithMessage("Member number cannot exceed 200 characters");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Surname is required")
            .MaximumLength(300)
            .WithMessage("Surname cannot exceed 300 characters");

        RuleFor(x => x.Name)
            .MaximumLength(300)
            .WithMessage("Name cannot exceed 300 characters");

        RuleFor(x => x.Initials)
            .MaximumLength(50)
            .WithMessage("Initials cannot exceed 50 characters");

        RuleFor(x => x.Idnumber)
            .MaximumLength(300)
            .WithMessage("ID number cannot exceed 300 characters");

        RuleFor(x => x.PassportNumber)
            .MaximumLength(300)
            .WithMessage("Passport number cannot exceed 300 characters");

        RuleFor(x => x.MemberPhoneNumber)
            .MaximumLength(300)
            .WithMessage("Phone number cannot exceed 300 characters");

        RuleFor(x => x.MemberCellNumber)
            .MaximumLength(300)
            .WithMessage("Cell number cannot exceed 300 characters");

        RuleFor(x => x.MemberAddress1)
            .MaximumLength(500)
            .WithMessage("Address line 1 cannot exceed 500 characters");

        RuleFor(x => x.MemberAddress2)
            .MaximumLength(500)
            .WithMessage("Address line 2 cannot exceed 500 characters");

        RuleFor(x => x.MemberAddress3)
            .MaximumLength(500)
            .WithMessage("Address line 3 cannot exceed 500 characters");
    }
}
