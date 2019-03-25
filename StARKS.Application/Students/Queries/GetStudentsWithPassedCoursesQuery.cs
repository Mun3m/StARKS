using MediatR;
using StARKS.Application.Students.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Students.Queries
{
    public class GetStudentsWithPassedCoursesQuery : IRequest<IEnumerable<StudentDto>>
    {
        public string FilterByFullName { get; set; }
    }
}
