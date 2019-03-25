using StARKS.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Domain.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Grade? Grade { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
