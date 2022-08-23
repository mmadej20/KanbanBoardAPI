using KanbanBoard.Commands;
using KanbanBoard.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetToDoById(int id)
        {
            var response = await _mediator.Send(new GetToDoById.Query(id));
            return response == null ? NotFound() : Ok(response);
        }

        [HttpGet("/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTasks()
        {
            var response = await _mediator.Send(new GetAllTasks.Query());
            return response == null ? NotFound("There is no available tasks") : Ok(response);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddToDo(AddToDo.Command command) => Ok(await _mediator.Send(command));

        [HttpPatch("/complete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkAsCompleted(int id) => Ok(await _mediator.Send(new MarkAsCompleted.Command(id)));

        [HttpPatch("/cancel/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkAsCancelled(int id) => Ok(await _mediator.Send(new MarkAsCancelled.Command(id)));
    }
}