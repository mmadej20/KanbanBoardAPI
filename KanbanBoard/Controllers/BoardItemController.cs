using KanbanBoard.Api.Commands.BoardItems;
using KanbanBoard.Api.Queries.BoardItems;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetToDoById(int id)
    {
        var response = await _mediator.Send(new GetToDoById.Query(id));

        if (response.IsFailure)
        {
            return NotFound(response.Error);
        }

        return Ok(response);
    }

    //[HttpGet("all")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> GetAllTasks()
    //{
    //    var response = await _mediator.Send(new GetAllTasks.Query());
    //    return response == null ? NotFound("There is no available tasks") : Ok(response);
    //}

    //[HttpPost("add")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<IActionResult> AddToDo(AddToDo.Command command) => Ok(await _mediator.Send(command));

    [HttpPatch("{id}/complete/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsCompleted(int id) => Ok(await _mediator.Send(new MarkAsCompleted.Command(id)));

    [HttpPatch("{id}/cancel/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsCancelled(int id) => Ok(await _mediator.Send(new MarkAsCancelled.Command(id)));

    [HttpPatch("{id}/inProgress/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsInProgress(int id) => Ok(await _mediator.Send(new MarkAsInProgress.Command(id)));
}