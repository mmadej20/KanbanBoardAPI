using MediatR;
using System.Threading.Tasks;
using System.Threading;
using KanbanBoard.Services.Interfaces;
using DataAccess.Enums;
using KanbanBoard.Domain;

namespace KanbanBoard.Commands.BoardItems;

public class MarkAsCancelled
{
    public record Command(int Id) : IRequest<OperationResult>;

    public class Handler : IRequestHandler<Command, OperationResult>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.ChangeStatus(request.Id, StatusType.Canceled);
        }
    }
}