using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
{
    public ProjectCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be today or later than start date");

        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("Status ID is required");
    }
}
