using KanbanBoard.Api.Commands.Members;
using KanbanBoard.Api.Queries.Members;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var response = await _mediator.Send(new GetMemberById.Query(id));

            if (response.IsFailure)
            {
                return NotFound(response.Error);
            }

            return Ok(response);
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

            return Ok(response);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMember(UpdateMember.Command command)
        {
            var response = await _mediator.Send(command);

            if (response.IsFailure)
            {
                return NotFound(response.Error);
            }

            return Ok(response);
        }
    }
}
