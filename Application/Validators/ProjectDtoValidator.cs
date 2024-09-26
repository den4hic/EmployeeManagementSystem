using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class ProjectDtoValidator : AbstractValidator<ProjectDto>
{
    public ProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start date must be today or later");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be today or later than start date");

        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("Status ID is required");

    }
}
