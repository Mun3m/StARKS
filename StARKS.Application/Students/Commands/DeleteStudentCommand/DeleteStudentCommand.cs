using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Students.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
