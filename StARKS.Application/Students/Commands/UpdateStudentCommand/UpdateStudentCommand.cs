using MediatR;
using StARKS.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Students.Commands
{
    public class UpdateStudentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
    }
}
