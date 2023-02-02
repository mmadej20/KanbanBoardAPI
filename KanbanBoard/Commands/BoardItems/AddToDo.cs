using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands.BoardItems;

public class AddToDo
{
    //Command
    public record Command(string Name) : IRequest<OperationResult>;

    //Handler

    public class Handler : IRequestHandler<Command, OperationResult>
    {
        private readonly IBoardItemService _kanbanService;

        public Handler(IBoardItemService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.AddToDo(request.Name);
        }
    }
}