using AutoMapper;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Enrollments.Commands;
using StARKS.Application.Exceptions;
using StARKS.Application.Students.Commands;
using StARKS.Application.Test.Services;
using StARKS.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Courses.Commands
{

    [Collection("ServiceCollection")]
    public class DeleteCourseCommandHandlerTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;
        private readonly DeleteCourseCommandValidator queryValidatior;

        public DeleteCourseCommandHandlerTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
            this.queryValidatior = new DeleteCourseCommandValidator();
        }

        [Fact]
        public void Should_have_error_when_validate_query()
        {
            var result = queryValidatior.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_validate_query()
        {
            queryValidatior.ShouldNotHaveValidationErrorFor(x => x.Id, Guid.NewGuid());
        }

        public async Task Should_delete_course()
        {
            var deleteCourseCommand = new DeleteCourseCommand()
            {
                Id = Guid.NewGuid()
            };

            var deleteCourseCommandHandler = new DeleteCourseCommandHandler(this.context);

            var result = await deleteCourseCommandHandler.Handle(deleteCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            var dbCourse = await this.context.Course.FirstOrDefaultAsync(s => s.Id == deleteCourseCommand.Id);
            dbCourse.ShouldBeNull();
        }

        [Fact]
        public async Task Should_throw_not_found_exception()
        {
            var deleteCourseCommand = new DeleteCourseCommand()
            {
                Id = Guid.NewGuid()
            };

            var deleteCourseCommandHandler = new DeleteCourseCommandHandler(this.context);

            await Assert.ThrowsAsync<NotFoundException>(() => deleteCourseCommandHandler.Handle(deleteCourseCommand, CancellationToken.None));
        }

        [Fact]
        public async Task Should_throw_delete_failure_exception()
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
            var commandResult = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);
            commandResult.ShouldBe(true);

            // Create student
            var createStudentCommand = new CreateStudentCommand()
            {
                Id = Guid.NewGuid(),
                FirstName = "Milos",
                LastName = "Stojkovic",
                Address = "Bata Noleta 31",
                City = "Sokobanja",
                DateOfBirth = new DateTime(1991, 3, 18),
                State = "Srbija",
                Gender = 0
            };

            var createStudentCommandHandler = new CreateStudentCommandHandler(this.autoMapper, this.context);
            commandResult = await createStudentCommandHandler.Handle(createStudentCommand, CancellationToken.None);
            commandResult.ShouldBe(true);

            var createOrUpdateEnrollmentCommand = new CreateOrUpdateEnrollmentCommand()
            {
                Id = createStudentCommand.Id,
                CourseCode = createCourseCommand.Code,
                Grade = (int)Domain.Enumerations.Grade.Seven
            };

            var createOrUpdateEnrollmentCommandHandler = new CreateOrUpdateEnrollmentCommandHandler(this.context);
            commandResult = await createOrUpdateEnrollmentCommandHandler.Handle(createOrUpdateEnrollmentCommand, CancellationToken.None);
            commandResult.ShouldBe(true);

            var deleteCourseCommand = new DeleteCourseCommand()
            {
                Id = createCourseCommand.Id
            };

            var deleteCourseCommandHandler = new DeleteCourseCommandHandler(this.context);
            await Assert.ThrowsAsync<DeleteFailureException>(() => deleteCourseCommandHandler.Handle(deleteCourseCommand, CancellationToken.None));
        }
    }
}
