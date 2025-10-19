using KanbanBoard.Application.Members.Commands;
using KanbanBoard.Application.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberById([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetMemberById.Query(id));

            if (response.IsFailure)
            {
                return NotFound(response.Error);
            }

            return Ok(response.Value);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMember(AddMember.Command command)
        {
            var response = await _mediator.Send(command);

            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMember(UpdateMember.Command command)
        {
            var response = await _mediator.Send(command);

            if (response.IsFailure)
            {
                return NotFound(response.Error);
            }

            return Ok();
        }
    }
}
