using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Application.Students.Models;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Students.Queries
{
    public class GetStudentsWithPassedCoursesQueryHandler : IRequestHandler<GetStudentsWithPassedCoursesQuery, IEnumerable<StudentDto>>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper mapper;

        public GetStudentsWithPassedCoursesQueryHandler(IMapper mapper, StARKSDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> Handle(GetStudentsWithPassedCoursesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Student> query;
            query = this.context.Student;

            if (string.IsNullOrWhiteSpace(request.FilterByFullName) == false)
                query = query.Where(c => $"{c.FirstName} {c.LastName}".ToLower().Contains(request.FilterByFullName.ToLower()) == true);

            var students = await query.Select(s => new StudentDto()
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.LastName}",
                Courses = s.Enrollments
                         .Select(e => new ExpendCourseDto()
                         {
                             Name = e.Course.Name,
                             Code = e.Course.Code,
                             Grade = (int)e.Grade
                         })
            }).AsNoTracking().ToListAsync(cancellationToken);

            return students;
        }
    }
}
