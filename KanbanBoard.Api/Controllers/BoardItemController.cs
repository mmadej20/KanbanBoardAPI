using KanbanBoard.Application.BoardItems.Commands;
using KanbanBoard.Application.BoardItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardItemController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetToDoById(int id)
    {
        var response = await _mediator.Send(new GetToDoById.Query(id));

        return response.IsFailure ? NotFound(response.Error) : Ok(response.Value);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTasks()
    {
        var response = await _mediator.Send(new GetAllTasks.Query());

        return response.IsFailure ? NotFound(response.Error) : Ok(response.Value);
    }

    //[HttpPost("add")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> AddToDo(AddToDo.Command command)
    //{
    //    var response = await _mediator.Send(command);

    //    return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    //}

    [HttpPatch("{id}/complete/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsCompleted(int id)
    {
        var response = await _mediator.Send(new MarkAsCompleted.Command(id));

        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }

    [HttpPatch("{id}/cancel/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsCancelled(int id)
    {
        var response = await _mediator.Send(new MarkAsCancelled.Command(id));

        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }

    [HttpPatch("{id}/inProgress/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsInProgress(int id)
    {
        var response = await _mediator.Send(new MarkAsInProgress.Command(id));

        return response.IsFailure ? BadRequest(response.Error) : Ok(response.Value);
    }
}