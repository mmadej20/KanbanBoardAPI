using KanbanBoard.Commands;
using KanbanBoard.Queries;
using MediatR;
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
        public async Task<IActionResult> GetToDoById(int id)
        {
            var response = await _mediator.Send(new GetToDoById.Query(id));
            return response == null ? NotFound() : Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToDo(AddToDo.Command command) => Ok(await _mediator.Send(command));

        [HttpPatch("/complete/{id}")]
        public async Task<IActionResult> MarkAsCompleted(int id) => Ok(await _mediator.Send(new MarkAsCompleted.Command(id)));
    }
}