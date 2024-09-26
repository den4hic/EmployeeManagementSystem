using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class StatusDtoValidator : AbstractValidator<StatusDto>
{
    public StatusDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
    }
}
