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

namespace StARKS.Application.Courses.Commands
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, bool>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper mapper;

        public UpdateCourseCommandHandler(IMapper mapper, StARKSDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context.Course.SingleOrDefaultAsync(c => c.Id == request.Id && c.Code == request.Code, cancellationToken);
            if (entity == null)
                throw new NotFoundException(nameof(Course), request.Id);

            this.mapper.Map(request, entity);

            this.context.Course.Update(entity);
            var result = await this.context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
