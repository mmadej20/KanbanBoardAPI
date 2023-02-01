using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace KanbanBoard.Commands.Boards;

public class CreateBoard
{
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
        }
    }
}
