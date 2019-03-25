using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StARKS.Application.Enrollments.Commands;
using StARKS.Application.Students.Commands;
using StARKS.Application.Students.Models;
using StARKS.Application.Students.Queries;

namespace StARKS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator mediator;

        public StudentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("passedCourses")]
        [ProducesResponseType(typeof(IEnumerable<StudentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetPassedCourses([FromQuery] string filterByFullName)
        {
            return this.Ok(await this.mediator.Send(new GetStudentsWithPassedCoursesQuery() { FilterByFullName = filterByFullName }));
        }

        [HttpPost("setCrade")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateStudentGrade([FromBody] CreateOrUpdateEnrollmentCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post([FromBody] CreateStudentCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put([FromBody] UpdateStudentCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(DeleteStudentCommand command)
        {
            return this.Ok(await this.mediator.Send(command));
        }
    }
}
