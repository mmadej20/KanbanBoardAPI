using KanbanBoard.Models;
using KanbanBoard.Repositories;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands
{
    public class AddToDo
    {
        //Command
        public record Command(string Name) : IRequest<int>;

        //Handler

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IKanbanService _kanbanService;

            public Handler(IKanbanService kanbanService)
            {
                _kanbanService = kanbanService;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _kanbanService.AddToDo(request.Name);
                await _kanbanService.AddToDo("asd");
            }
        }
    }
}