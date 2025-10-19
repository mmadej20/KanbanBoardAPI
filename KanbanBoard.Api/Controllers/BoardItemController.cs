using KanbanBoard.Api.Contracts.BoardItem.Requests;
using KanbanBoard.Api.Routing;
using KanbanBoard.Application.BoardItems.Commands;
using KanbanBoard.Application.BoardItems.Queries;
using KanbanBoard.Application.Boards.Commands;
using KanbanBoard.Application.Boards.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers;

[Route(ApiRoutes.V1.BoardItems.Base)]
[ApiController]
public class BoardItemController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet(ApiRoutes.V1.BoardItems.GetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBoardItemById([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetBoardItemById.Query(id));

        return response.IsFailure ? NotFound(response.Error) : Ok(response.Value);
    }

    [HttpGet(ApiRoutes.V1.BoardItems.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllBoardItems()
    {
        var response = await _mediator.Send(new GetAllBoardItems.Query());

        return response.IsFailure ? NotFound(response.Error) : Ok(response.Value);
    }

    [HttpPost(ApiRoutes.V1.BoardItems.AssignMember)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignMemberToBoardItem([FromBody] AssignMemberToBoardItemRequest request)
    {
        var response = await _mediator.Send(new AssignMemberToBoardItem.Command(request.TaskId, request.MemberId));

        if (response.IsFailure)
        {
            if (response.Error == BoardServiceErrors.ItemNotFound(request.TaskId)
                || response.Error == BoardServiceErrors.MemberNotFound(request.MemberId))
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

    [HttpPut(ApiRoutes.V1.BoardItems.MarkAsCompleted)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsCompleted([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new MarkAsCompleted.Command(id));

        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }

    [HttpPut(ApiRoutes.V1.BoardItems.MarkAsCancelled)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsCancelled([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new MarkAsCancelled.Command(id));

        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }

    [HttpPut(ApiRoutes.V1.BoardItems.MarkAsInProgress)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsInProgress([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new MarkAsInProgress.Command(id));

        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }
}