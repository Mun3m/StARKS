using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Application.Exceptions;
using StARKS.Domain.Entities;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Enrollments.Commands
{
    public class CreateOrUpdateEnrollmentCommandHandler : IRequestHandler<CreateOrUpdateEnrollmentCommand, bool>
    {
        private readonly StARKSDbContext context;

        public CreateOrUpdateEnrollmentCommandHandler(StARKSDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(CreateOrUpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await this.context.Enrollment
                                               .FirstOrDefaultAsync(e => e.Course.Code == request.CourseCode && e.Student.Id == request.Id, cancellationToken);
            if (enrollment == null)
            {
                var student = await this.context.Student.SingleOrDefaultAsync(s => s.Id == request.Id);
                if (student == null)
                    throw new NotFoundException(nameof(Student), request.Id);

                var course = await this.context.Course.SingleOrDefaultAsync(s => s.Code == request.CourseCode);
                if (course == null)
                    throw new NotFoundException(nameof(Course), request.CourseCode);

                //if (student == null || course == null)
                //    throw new ArgumentException($"The relationship between student and course does not exist, StudentId: {request.Id}, CourseCode: {request.CourseCode}");

                this.context.Enrollment.Add(new Enrollment()
                {
                    Id = Guid.NewGuid(),
                    CourseId = course.Id,
                    StudentId = student.Id,
                    Course = course,
                    Student = student,
                    Grade = (Domain.Enumerations.Grade)request.Grade
                });
            }
            else
            {
                enrollment.Grade = (Domain.Enumerations.Grade)request.Grade;
            }

            var result = await this.context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
