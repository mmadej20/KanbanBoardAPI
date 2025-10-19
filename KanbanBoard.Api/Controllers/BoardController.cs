using KanbanBoard.Api.Contracts.Board.Reqeusts;
using KanbanBoard.Api.Mapping;
using KanbanBoard.Application.Boards.Commands;
using KanbanBoard.Application.Boards.Errors;
using KanbanBoard.Application.Boards.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var response = await _mediator.Send(new CreateBoard.Command(request.Name));
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }

        return Created();
    }

    [HttpGet("{boardId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBoardById([FromRoute] Guid boardId)
    {
        var response = await _mediator.Send(new GetBoardById.Query(boardId));
        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.BoardNotFound(boardId))
            {
                return NotFound(response.Error);
            }
            else
            {
                return BadRequest(BoardServiceErrors.GenericError);
            }
        }

        if (response.TryGetValue(out var board))
        {
            var boardResponse = board.ToBoardResponse();
            return Ok(boardResponse);
        }

        return BadRequest(BoardServiceErrors.GenericError);
    }

    [HttpPost("boardItem/create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItemInBoard([FromBody] CreateItemInBoardRequest request)
    {
        var response = await _mediator.Send(new CreateItemInBoard.Command(request.BoardId, request.Name));

        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }

        return Created();
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteBoard([FromRoute] Guid boardId)
    {
        var response = await _mediator.Send(new DeleteBoard.Command(boardId));

        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }

        return Ok();
    }

    [HttpPost("addMember")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMemberToBoard(AddMemberToBoardRequest request)
    {
        var response = await _mediator.Send(new AddMemberToBoard.Command(request.BoardId, request.MemberId));

        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.BoardNotFound(request.BoardId)
                || response.Error == BoardServiceErrors.MemberNotFound(request.MemberId))
            {
                return NotFound(response.Error);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        return Created();
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
    public async Task<IActionResult> AssignMemberToTask(AssignMemberToBoardItem.Command command)
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
