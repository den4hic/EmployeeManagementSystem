using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Position is required")
            .MaximumLength(100).WithMessage("Position must not exceed 100 characters");

        RuleFor(x => x.HireDate)
            .NotNull().WithMessage("Hire date is required")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Hire date must be today or later");

        RuleFor(x => x.Salary)
            .NotEmpty().WithMessage("Salary is required")
            .GreaterThan(0).WithMessage("Salary must be greater than 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}
