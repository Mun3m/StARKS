using AutoMapper;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using StARKS.Application.Exceptions;
using StARKS.Application.Students.Commands;
using StARKS.Application.Test.Services;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Students.Commands
{
    [Collection("ServiceCollection")]
    public class UpdateStudentCommandHandlerTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;
        private readonly UpdateStudentCommandValidator queryValidatior;

        public UpdateStudentCommandHandlerTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
            this.queryValidatior = new UpdateStudentCommandValidator();
        }

        [Fact]
        public void Should_have_error_when_validate_query()
        {
            var result = queryValidatior.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
            result = queryValidatior.ShouldHaveValidationErrorFor(x => x.FirstName, Enumerable.Repeat("s", 51).ToString());
        }

        [Fact]
        public void Should_not_have_error_when_validate_query()
        {
            queryValidatior.ShouldNotHaveValidationErrorFor(x => x.Id, Guid.NewGuid());
            queryValidatior.ShouldNotHaveValidationErrorFor(x => x.FirstName, "Milos");
        }

        [Fact]
        public async Task Should_update_student()
        {

            // create student
            var createStudentCommand = new CreateStudentCommand()
            {
                Id = Guid.NewGuid(),
                FirstName = "Milos",
                LastName = "Stojkovic",
                Address = "Bata Noleta 31",
                City = "Sokobanja",
                DateOfBirth = new DateTime(1991, 3, 18),
                State = "Srbija",
                Gender = (int)Domain.Enumerations.Gender.Male
            };

            var CreateStudentCommandHandler = new CreateStudentCommandHandler(this.autoMapper, this.context);

            var result = await CreateStudentCommandHandler.Handle(createStudentCommand, CancellationToken.None);
            result.ShouldBe(true);

            // update student
            var updateStudentCommand = new UpdateStudentCommand()
            {
                Id = createStudentCommand.Id,
                FirstName = "Milos",
                LastName = "Stojkovic",
                Address = "Bata Noleta 31",
                City = "Beograd",
                DateOfBirth = new DateTime(1991, 3, 18),
                State = "Srbija",
                Gender = (int)Domain.Enumerations.Gender.Male
            };

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(this.autoMapper, this.context);

            result = await updateStudentCommandHandler.Handle(updateStudentCommand, CancellationToken.None);
            result.ShouldBe(true);

            var dbStudent = await this.context.Student.FirstOrDefaultAsync(s => s.Id == updateStudentCommand.Id);
            dbStudent.ShouldNotBeNull();
            dbStudent.City.ShouldBe("Beograd");
        }

        [Fact]
        public async Task Should_throw_not_found_exception()
        {
            var updateStudentCommand = new UpdateStudentCommand()
            {
                Id = Guid.NewGuid(),
                FirstName = "Milos",
                LastName = "Stojkovic",
                Address = "Bata Noleta 31",
                City = "Beograd",
                DateOfBirth = new DateTime(1991, 3, 18),
                State = "Srbija",
                Gender = (int)Domain.Enumerations.Gender.Male
            };

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(this.autoMapper, this.context);

            await Assert.ThrowsAsync<NotFoundException>(() => updateStudentCommandHandler.Handle(updateStudentCommand, CancellationToken.None));
        }
    }
}
