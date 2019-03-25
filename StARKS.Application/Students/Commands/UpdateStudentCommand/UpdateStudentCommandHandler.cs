using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Application.Exceptions;
using StARKS.Domain.Entities;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Students.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper mapper;

        public UpdateStudentCommandHandler(IMapper mapper, StARKSDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Student.SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException(nameof(Student), request.Id);

            this.mapper.Map(request, entity);

            this.context.Student.Update(entity);
            var result = await this.context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
