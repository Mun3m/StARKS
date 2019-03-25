using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Exceptions;
using StARKS.Application.Test.Services;
using StARKS.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Courses.Commands
{
    [Collection("ServiceCollection")]
    public class UpdateCourseCommandHandlerTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;
        private readonly UpdateCourseCommandValidator queryValidatior;

        public UpdateCourseCommandHandlerTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
            this.queryValidatior = new UpdateCourseCommandValidator();
        }

        [Fact]
        public async Task Should_update_student()
        {
            // create course
            var createCourseCommand = new CreateCourseCommand()
            {
                Id = Guid.NewGuid(),
                Code = 1,
                Name = "Course 1",
                Description = "Test"
            };

            var createCourseCommandHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);

            var result = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            var updateCourseCommand = new UpdateCourseCommand()
            {
                Id = createCourseCommand.Id,
                Code = 1,
                Name = "Course 2",
                Description = "Test"
            };

            var updateCourseCommandHandler = new UpdateCourseCommandHandler(this.autoMapper, this.context);

            result = await updateCourseCommandHandler.Handle(updateCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            var dbCourse = await this.context.Course.FirstOrDefaultAsync(s => s.Id == updateCourseCommand.Id);
            dbCourse.ShouldNotBeNull();
            dbCourse.Name.ShouldBe(updateCourseCommand.Name);
        }

        [Fact]
        public async Task Should_throw_not_found_exception()
        {
            var updateCourseCommand = new UpdateCourseCommand()
            {
                Id = Guid.NewGuid(),
                Code = 1,
                Name = "Course 1",
                Description = "Test"
            };

            var updateCourseCommandHandler = new UpdateCourseCommandHandler(this.autoMapper, this.context);

            await Assert.ThrowsAsync<NotFoundException>(() => updateCourseCommandHandler.Handle(updateCourseCommand, CancellationToken.None));
        }
    }
}
