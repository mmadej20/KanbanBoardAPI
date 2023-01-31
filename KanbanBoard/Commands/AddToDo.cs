using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands
{
    public class AddToDo
    {
        //Command
        public record Command(string Name) : IRequest<bool>;

        //Handler

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IKanbanService _kanbanService;

            public Handler(IKanbanService kanbanService)
            {
                _kanbanService = kanbanService;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _kanbanService.AddToDo(request.Name);
            }
        }
    }
}