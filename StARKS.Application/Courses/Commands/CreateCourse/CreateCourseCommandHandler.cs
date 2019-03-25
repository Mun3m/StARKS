using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StARKS.Domain.Entities;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StARKS.Application.Courses.Commands
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, bool>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper mapper;

        public CreateCourseCommandHandler(IMapper mapper, StARKSDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }   

        public async Task<bool> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<Course>(request);

            var course = await this.context.Course.SingleOrDefaultAsync(c=> c.Code == request.Code);
            if (course != null)
                throw new ArgumentException($"The course already exisit with code: {request.Code} , code must be unique");

            context.Course.Add(entity);
            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
