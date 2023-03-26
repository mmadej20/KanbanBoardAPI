using KanbanBoard.Commands.Members;
using KanbanBoard.Queries.Members;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
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
            return response == null ? NotFound() : Ok(response);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMember(AddMember.Command command)
        {
            var response = await _mediator.Send(command);
            return response.IsSuccesfull == false ? BadRequest(response) : Ok(response);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMember(UpdateMember.Command command)
        {
            var response = await _mediator.Send(command);
            
            if (response.IsNotFound)
                return NotFound(response.Message);

            return response.IsSuccesfull == false ? BadRequest(response) : Ok(response);
        }
    }
}
