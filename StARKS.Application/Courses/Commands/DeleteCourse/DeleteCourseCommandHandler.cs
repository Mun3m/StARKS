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

namespace StARKS.Application.Courses.Commands
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly StARKSDbContext context;

        public DeleteCourseCommandHandler(StARKSDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Course.SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException(nameof(Course), request.Id);

            var hasStudents = await this.context.Enrollment.AnyAsync(e => e.CourseId == request.Id, cancellationToken);
            if (hasStudents == true)
                throw new DeleteFailureException(nameof(Student), request.Id, "There are existing students associated with this course.");

            this.context.Course.Remove(entity);
            var result = await this.context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
