using AutoMapper;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Shouldly;
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
    public class CreateStudentCommandTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;
        private readonly CreateStudentCommandValidator queryValidatior;

        public CreateStudentCommandTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
            this.queryValidatior = new CreateStudentCommandValidator();
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
        public async Task Should_create_student()
        {
            var createStudentCommand = new CreateStudentCommand()
            {
                Id = Guid.NewGuid(),
                FirstName = "Milos",
                LastName = "Stojkovic",
                Address = "Bata Noleta 31",
                City = "Sokobanja",
                DateOfBirth = new DateTime(1991, 3, 18),
                State = "Srbija",
                Gender = (int)Domain.Enumerations.Gender.Female
            };

            var createStudentCommandHandler = new CreateStudentCommandHandler(this.autoMapper, this.context);

            var result = await createStudentCommandHandler.Handle(createStudentCommand, CancellationToken.None);
            result.ShouldBe(true);

            var dbStudent = await this.context.Student.FirstOrDefaultAsync(s => s.Id == createStudentCommand.Id);
            dbStudent.ShouldNotBeNull();
        }
    }
}
