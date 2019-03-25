using MediatR;
using StARKS.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Enrollments.Commands
{
    public class CreateOrUpdateEnrollmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public int CourseCode { get; set; }
        public int Grade { get; set; }
    }
}
