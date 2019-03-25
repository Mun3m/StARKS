using StARKS.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Domain.Entities
{
    public class Student
    {
        public Student()
        {
            this.Enrollments = new HashSet<Enrollment>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
