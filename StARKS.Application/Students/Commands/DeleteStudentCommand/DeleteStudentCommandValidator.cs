using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Students.Commands
{
    public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
