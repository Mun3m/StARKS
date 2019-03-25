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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Enrollments.Commands
{
    [Collection("ServiceCollection")]
    public class CreateOrUpdateEnrollmentCommandHandlerTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;
        private readonly CreateOrUpdateEnrollmentCommandValidator queryValidatior;

        public CreateOrUpdateEnrollmentCommandHandlerTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
            this.queryValidatior = new CreateOrUpdateEnrollmentCommandValidator();
        }

        [Fact]
        public void Should_have_error_when_course_code_is_negative_value()
        {
            var result = queryValidatior.ShouldHaveValidationErrorFor(x => x.CourseCode, -1);
        }

        [Fact]
        public void Should_have_error_when_course_code_exceed_value()
        {
            var result = queryValidatior.ShouldHaveValidationErrorFor(x => x.CourseCode, int.Parse(int.MaxValue.ToString()) + 1);
        }

        [Fact]
        public void Should_have_error_when_course_grade_is_five()
        {
            var result = queryValidatior.ShouldHaveValidationErrorFor(x => x.Grade, 5);
        }

        [Fact]
        public async Task Should_throw_not_argument_exception()
        {
            var createOrUpdateEnrollmentCommand = new CreateOrUpdateEnrollmentCommand()
            {
                Id = Guid.NewGuid(),
                CourseCode = 9999,
                Grade = (int)Domain.Enumerations.Grade.Six
            };

            var createOrUpdateEnrollmentCommandHandler = new CreateOrUpdateEnrollmentCommandHandler(this.context);

            await Assert.ThrowsAsync<NotFoundException>(() => createOrUpdateEnrollmentCommandHandler.Handle(createOrUpdateEnrollmentCommand, CancellationToken.None));
        }

        [Fact]
        public async Task Should_update_grade()
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

            var courseResult = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);

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

            var studentResult = await createStudentCommandHandler.Handle(createStudentCommand, CancellationToken.None);

            var createOrUpdateEnrollmentCommand = new CreateOrUpdateEnrollmentCommand()
            {
                Id = createStudentCommand.Id,
                CourseCode = createCourseCommand.Code,
                Grade = (int)Domain.Enumerations.Grade.Seven
            };

            var createOrUpdateEnrollmentCommandHandler = new CreateOrUpdateEnrollmentCommandHandler(this.context);
            var result = await createOrUpdateEnrollmentCommandHandler.Handle(createOrUpdateEnrollmentCommand, CancellationToken.None);

            result.ShouldBe(true);

            var enrollment = await this.context.Enrollment
                                              .FirstOrDefaultAsync(e => e.Course.Code == createOrUpdateEnrollmentCommand.CourseCode && e.Student.Id == createOrUpdateEnrollmentCommand.Id, CancellationToken.None);

            enrollment.Grade.ShouldBe(Domain.Enumerations.Grade.Seven);

            createOrUpdateEnrollmentCommand.Grade = (int)Domain.Enumerations.Grade.Eight;
            var updateResult = await createOrUpdateEnrollmentCommandHandler.Handle(createOrUpdateEnrollmentCommand, CancellationToken.None);

            updateResult.ShouldBe(true);
            var updatedEnrollment = await this.context.Enrollment
                                              .FirstOrDefaultAsync(e => e.Course.Code == createOrUpdateEnrollmentCommand.CourseCode && e.Student.Id == createOrUpdateEnrollmentCommand.Id, CancellationToken.None);

            updatedEnrollment.Grade.ShouldBe(Domain.Enumerations.Grade.Eight);
        }
    }
}
