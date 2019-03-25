using MediatR;
using StARKS.Application.Courses.Models;
using StARKS.Application.Students.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Courses.Queries
{
    public class GetCoursesQuery : IRequest<IEnumerable<CourseDto>>
    {
        public string FilterByName { get; set; }
    }
}
