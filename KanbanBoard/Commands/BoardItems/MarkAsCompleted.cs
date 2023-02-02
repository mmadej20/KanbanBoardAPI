using DataAccess.Enums;
using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands.BoardItems;

public class MarkAsCompleted
{
    public record Command(int Id) : IRequest<OperationResult>;

    public class Handler : IRequestHandler<Command, OperationResult>
    {
        private readonly IBoardItemService _kanbanService;

        public Handler(IBoardItemService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.ChangeStatus(request.Id, StatusType.Completed);
        }
    }
}