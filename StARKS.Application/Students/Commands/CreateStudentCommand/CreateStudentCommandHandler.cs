using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Domain.Entities;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Students.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, bool>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper mapper;

        public CreateStudentCommandHandler(IMapper mapper, StARKSDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }   

        public async Task<bool> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Student.SingleOrDefaultAsync(c => c.Id == request.Id);
            if (entity != null)
                throw new ArgumentException($"The student already exisit with id: {request.Id}, id must be unique");

            var student = mapper.Map<Student>(request);

            context.Student.Add(student);
            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
