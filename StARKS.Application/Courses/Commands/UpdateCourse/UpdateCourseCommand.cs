using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Courses.Commands
{
    public class UpdateCourseCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
