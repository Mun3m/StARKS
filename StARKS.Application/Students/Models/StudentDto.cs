using System;
using System.Collections.Generic;

namespace StARKS.Application.Students.Models
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ExpendCourseDto> Courses { get; set; }
    }
}
