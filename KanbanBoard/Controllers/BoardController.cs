using KanbanBoard.Commands.Boards;
using KanbanBoard.Queries.Boards;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateBoard(CreateBoard.Command command) => Ok(await _mediator.Send(command));

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBoardById(int id)
    {
        var response = await _mediator.Send(new GetBoardById.Query(id));
        return response == null ? NotFound() : Ok(response);
    }

    [HttpPost("boardItem/create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItemInBoard(CreateItemInBoard.Command command) => Ok(await _mediator.Send(command));

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteBoard(DeleteBoard.Command command) => Ok(await _mediator.Send(command));

    [HttpPost("addMember")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMemberToBoard(AddMemberToBoard.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsNotFound)
            return NotFound(response.Message);

        return response.IsSuccesfull == false ? BadRequest(response) : Ok(response);
    }

    [HttpPatch("removeMember")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveMemberFromBoard(RemoveMemberFromBoard.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsNotFound)
            return NotFound(response.Message);

        return response.IsSuccesfull == false ? BadRequest(response) : Ok(response);
    }

    [HttpPost("assignToTask")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignMemberToTask(AssignMemberToTask.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsNotFound)
            return NotFound(response.Message);

        return response.IsSuccesfull == false ? BadRequest(response) : Ok(response);
    }
}
