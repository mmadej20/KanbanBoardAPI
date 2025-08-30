using KanbanBoard.Application.Boards.Commands;
using KanbanBoard.Application.Boards.Errors;
using KanbanBoard.Application.Boards.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBoard(CreateBoard.Command command)
    {
        var response = await _mediator.Send(command);
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBoardById(int id)
    {
        var response = await _mediator.Send(new GetBoardById.Query(id));
        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.BoardNotFound(id))
            {
                return NotFound(response.Error);
            }
            else
            {
                return BadRequest(BoardServiceErrors.GenericError);
            }
        }
        return Ok(response.Value);
    }

    [HttpPost("boardItem/create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItemInBoard(CreateItemInBoard.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }

        return Ok();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteBoard(DeleteBoard.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }

        return Ok();
    }

    [HttpPost("addMember")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMemberToBoard(AddMemberToBoard.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.BoardNotFound(command.BoardId)
                || response.Error == BoardServiceErrors.MemberNotFound(command.MemberId))
            {
                return NotFound(response.Error);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        return Ok();
    }

    [HttpPatch("removeMember")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveMemberFromBoard(RemoveMemberFromBoard.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.BoardNotFound(command.BoardId)
                || response.Error == BoardServiceErrors.MemberNotFound(command.MemberId))
            {
                return NotFound(response.Error);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        return Ok();
    }

    [HttpPost("assignToTask")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignMemberToTask(AssignMemberToTask.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.ItemNotFound(command.TaskId)
                || response.Error == BoardServiceErrors.MemberNotFound(command.MemberId))
            {
                return NotFound(response.Error);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        return Ok();
    }
}
