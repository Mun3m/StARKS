using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Application.Exceptions;
using StARKS.Domain.Entities;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Students.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly StARKSDbContext context;

        public DeleteStudentCommandHandler(StARKSDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Student.SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException(nameof(Student), request.Id);

            var hasCourses = await this.context.Enrollment.AnyAsync(e => e.StudentId == request.Id, cancellationToken);
            if (hasCourses)
                throw new DeleteFailureException(nameof(Student), request.Id, "There are existing courses associated with this student.");

            this.context.Student.Remove(entity);
            var result = await this.context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
