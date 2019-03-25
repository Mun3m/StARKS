using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Application.Courses.Models;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Courses.Queries
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly StARKSDbContext context;

        public GetCoursesQueryHandler(IMapper mapper, StARKSDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Course> query;
            query = this.context.Course;

            if (string.IsNullOrWhiteSpace(request.FilterByName) == false)
                query = query.Where(c => c.Name.ToLower().Contains(request.FilterByName.ToLower()) == true);

            var courses = await query.Select(c => new CourseDto()
            {
                Name = c.Name,
                Code = c.Code,
                Description = c.Description
            }).AsNoTracking().ToListAsync(cancellationToken);

            return courses;
        }
    }
}
