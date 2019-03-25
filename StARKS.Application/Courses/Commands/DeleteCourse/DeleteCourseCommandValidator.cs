using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Courses.Commands
{
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
