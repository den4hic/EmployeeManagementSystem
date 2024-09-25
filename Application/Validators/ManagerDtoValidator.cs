using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class ManagerDtoValidator : AbstractValidator<ManagerDto>
{
    public ManagerDtoValidator()
    {
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}
