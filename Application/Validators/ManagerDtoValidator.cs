using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class ManagerDtoValidator : AbstractValidator<ManagerDto>
{
    public ManagerDtoValidator()
    {
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(100).WithMessage("Department must not exceed 100 characters");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.HireDate)
            .NotNull().WithMessage("Hire date is required")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Hire date must be today or later");
    }
}
