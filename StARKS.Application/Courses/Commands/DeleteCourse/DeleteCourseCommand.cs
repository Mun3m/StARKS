using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Courses.Commands
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
