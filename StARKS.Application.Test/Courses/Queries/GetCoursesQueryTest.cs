using AutoMapper;
using Shouldly;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Courses.Queries;
using StARKS.Application.Test.Services;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StARKS.Application.Test.Courses.Queries
{

    [Collection("ServiceCollection")]
    public class GetCoursesQueryTest : IClassFixture<StARKSDbContextFixture>
    {
        private readonly StARKSDbContext context;
        private readonly IMapper autoMapper;

        public GetCoursesQueryTest(ServiceCollectionFixture serviceCollection, StARKSDbContextFixture dbContextFixture)
        {
            this.context = dbContextFixture.Instance;
            this.autoMapper = serviceCollection.AutoMapperFixture.Instance;
        }

        [Fact]
        public async Task Should_return_courses()
        {
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

            var getCoursesQuery = new GetCoursesQuery();

            var getCoursesQueryHandler = new GetCoursesQueryHandler(this.autoMapper, this.context);

            var list = await getCoursesQueryHandler.Handle(getCoursesQuery, CancellationToken.None);
            list.Where(c => c.Code == createCourseCommand.Code).Count().ShouldBe(1);
        }

        [Fact]
        public async Task Should_return_filter_courses()
        {
            var createCourseCommand = new CreateCourseCommand()
            {
                Id = Guid.NewGuid(),
                Code = 10,
                Name = "Course 1",
                Description = "Test"
            };

            var createCourseCommandHandler = new CreateCourseCommandHandler(this.autoMapper, this.context);

            var result = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            createCourseCommand.Id = Guid.NewGuid();
            createCourseCommand.Code = 2;
            createCourseCommand.Name = "Test 2";

            result = await createCourseCommandHandler.Handle(createCourseCommand, CancellationToken.None);
            result.ShouldBe(true);

            var getCoursesQuery = new GetCoursesQuery() { FilterByName = "est" };
            var getCoursesQueryHandler = new GetCoursesQueryHandler(this.autoMapper, this.context);

            var list = await getCoursesQueryHandler.Handle(getCoursesQuery, CancellationToken.None);
            list.Where(c => c.Code == createCourseCommand.Code).Count().ShouldBe(1);

            getCoursesQuery = new GetCoursesQuery() { FilterByName = "" };
            getCoursesQueryHandler = new GetCoursesQueryHandler(this.autoMapper, this.context);

            list = await getCoursesQueryHandler.Handle(getCoursesQuery, CancellationToken.None);
            list.Where(c => c.Code == 10 || c.Code == 2).Count().ShouldBe(2);
        }
    }
}
