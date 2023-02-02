using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using DataAccess.Enums;
using KanbanBoard.Domain;

namespace KanbanBoard.Commands.BoardItems;

public class MarkAsInProgress
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
            return await _kanbanService.ChangeStatus(request.Id, StatusType.InProgress);
        }
    }
}