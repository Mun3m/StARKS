using AutoMapper;
using Shouldly;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Enrollments.Commands;
using StARKS.Application.Students.Commands;
using StARKS.Application.Students.Queries;
using StARKS.Application.Test.Services;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Students.Queries
{
    [Collection("ServiceCollection")]
    public class GetStudentsWithPassedCoursesQueryHandlerTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;

        public GetStudentsWithPassedCoursesQueryHandlerTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
        }

        [Fact]
        public async Task Should_return_sutdents_with_passed_coruces()
        {
            // create course
            var courseQuery = new CreateCourseCommand()
            {
                Id = Guid.NewGuid(),
                Code = 5,
                Name = "Course 2",
                Description = "Test"
            };

            var courseHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);

            var courseResult = await courseHandler.Handle(courseQuery, CancellationToken.None);

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
                CourseCode = courseQuery.Code,
                Grade = (int)Domain.Enumerations.Grade.Seven
            };

            var createOrUpdateEnrollmentCommandHandler = new CreateOrUpdateEnrollmentCommandHandler(this.context);
            var enrollmentResult = await createOrUpdateEnrollmentCommandHandler.Handle(createOrUpdateEnrollmentCommand, CancellationToken.None);
            enrollmentResult.ShouldBe(true);

            var query = new GetStudentsWithPassedCoursesQuery();
            var handler = new GetStudentsWithPassedCoursesQueryHandler(this.autoMapper, this.context);
            var result = await handler.Handle(query, CancellationToken.None);

            result.Where(s=>s.Id == createStudentCommand.Id).Count().ShouldBe(1);
        }

        [Fact]
        public async Task Should_return_filter_Students()
        {
            var firstStudentId = Guid.NewGuid();
            var secondStudentId = Guid.NewGuid();

            // enrollment added 1
            {
                // create course
                var courseQuery = new CreateCourseCommand()
                {
                    Id = Guid.NewGuid(),
                    Code = 2,
                    Name = "Course 2",
                    Description = "Test"
                };

                var courseHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);
                var courseResult = await courseHandler.Handle(courseQuery, CancellationToken.None);

                // Create student
                var studentQuery = new CreateStudentCommand()
                {
                    Id = firstStudentId,
                    FirstName = "Milos",
                    LastName = "Stojkovic",
                    Address = "Bata Noleta 31",
                    City = "Sokobanja",
                    DateOfBirth = new DateTime(1991, 3, 18),
                    State = "Srbija",
                    Gender = 0
                };

                var studenthandler = new CreateStudentCommandHandler(this.autoMapper, this.context);
                var studentResult = await studenthandler.Handle(studentQuery, CancellationToken.None);
                studentResult.ShouldBe(true);

                var enrollmentQuery = new CreateOrUpdateEnrollmentCommand()
                {
                    Id = studentQuery.Id,
                    CourseCode = courseQuery.Code,
                    Grade = (int)Domain.Enumerations.Grade.Seven
                };

                var enrollmentHandler = new CreateOrUpdateEnrollmentCommandHandler(this.context);
                var enrollmentResult = await enrollmentHandler.Handle(enrollmentQuery, CancellationToken.None);
                enrollmentResult.ShouldBe(true);
            }

            // enrollment added 2
            {
                // create course
                var courseQuery = new CreateCourseCommand()
                {
                    Id = Guid.NewGuid(),
                    Code = 3,
                    Name = "Course 2",
                    Description = "Test"
                };

                var courseHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);
                var courseResult = await courseHandler.Handle(courseQuery, CancellationToken.None);

                // Create student
                var studentQuery = new CreateStudentCommand()
                {
                    Id = secondStudentId,
                    FirstName = "Jelica",
                    LastName = "Ilica",
                    Address = "Bata Noleta 31",
                    City = "Sokobanja",
                    DateOfBirth = new DateTime(1991, 3, 18),
                    State = "Srbija",
                    Gender = 0
                };

                var studenthandler = new CreateStudentCommandHandler(this.autoMapper, this.context);
                var studentResult = await studenthandler.Handle(studentQuery, CancellationToken.None);
                studentResult.ShouldBe(true);

                var enrollmentQuery = new CreateOrUpdateEnrollmentCommand()
                {
                    Id = studentQuery.Id,
                    CourseCode = courseQuery.Code,
                    Grade = (int)Domain.Enumerations.Grade.Seven
                };

                var enrollmentHandler = new CreateOrUpdateEnrollmentCommandHandler(this.context);
                var enrollmentResult = await enrollmentHandler.Handle(enrollmentQuery, CancellationToken.None);
                enrollmentResult.ShouldBe(true);
            }

            var query = new GetStudentsWithPassedCoursesQuery()
            {
                FilterByFullName = "Milos"
            };

            var handler = new GetStudentsWithPassedCoursesQueryHandler(this.autoMapper, this.context);
            var result = await handler.Handle(query, CancellationToken.None);
            result.Where(s=>s.Id == firstStudentId).Count().ShouldBe(1);

            query = new GetStudentsWithPassedCoursesQuery()
            {
                FilterByFullName = ""
            };

            handler = new GetStudentsWithPassedCoursesQueryHandler(this.autoMapper, this.context);
            result = await handler.Handle(query, CancellationToken.None);
            result.Where(s => s.Id == firstStudentId || s.Id == secondStudentId).Count().ShouldBe(2);
        }
    }
}
