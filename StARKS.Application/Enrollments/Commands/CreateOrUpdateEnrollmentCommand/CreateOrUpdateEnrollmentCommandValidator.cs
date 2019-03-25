using FluentValidation;
using StARKS.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Enrollments.Commands
{
    public class CreateOrUpdateEnrollmentCommandValidator : AbstractValidator<CreateOrUpdateEnrollmentCommand>
    {
        public CreateOrUpdateEnrollmentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.CourseCode).NotEmpty().ExclusiveBetween(1, int.MaxValue);
            RuleFor(x => x.Grade).NotEmpty().ExclusiveBetween(5, 11);
        }
    }
}
