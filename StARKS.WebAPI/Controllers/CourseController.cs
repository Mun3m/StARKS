using MediatR;
using Microsoft.AspNetCore.Mvc;
using StARKS.Application.Courses.Commands;
using StARKS.Application.Courses.Models;
using StARKS.Application.Courses.Queries;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace StARKS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator mediator;

        public CourseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Get([FromQuery] string filterByName)
        {
            return this.Ok(await this.mediator.Send(new GetCoursesQuery() { FilterByName = filterByName }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post([FromBody] CreateCourseCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put([FromBody] UpdateCourseCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(DeleteCourseCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }
    }
}
