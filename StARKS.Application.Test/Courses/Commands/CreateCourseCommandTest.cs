using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Test.Services;
using StARKS.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Courses.Commands
{
    [Collection("ServiceCollection")]
    public class CreateCourseCommandTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;
        private readonly CreateCourseCommandValidator queryValidatior;

        public CreateCourseCommandTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
            this.queryValidatior = new CreateCourseCommandValidator();
        }

        [Fact]
        public async Task Should_create_course()
        {
            var createCourseCommand = new CreateCourseCommand()
            {
                Id = Guid.NewGuid(),
                Code = 12,
                Name = "Course 1",
                Description = "Test"
            };

            var createCourseCommandHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);

            var result = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            var dbCourse = await this.context.Course.FirstOrDefaultAsync(s => s.Id == createCourseCommand.Id);
            dbCourse.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_throw_argument_exception()
        {
            var createCourseCommand = new CreateCourseCommand()
            {
                Id = Guid.NewGuid(),
                Code = 11,
                Name = "Course 1",
                Description = "Test"
            };

            var createCourseCommandHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);

            var result = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            await Assert.ThrowsAsync<ArgumentException>(() => createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None));
        }
    }
}
