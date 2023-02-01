using MediatR;
using System.Threading.Tasks;
using System.Threading;
using KanbanBoard.Services.Interfaces;
using DataAccess.Enums;

namespace KanbanBoard.Commands.BoardItems;

public class MarkAsCancelled
{
    public record Command(int Id) : IRequest<int>;

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IBoardItemService _kanbanService;

        public Handler(IBoardItemService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.ChangeStatus(request.Id, StatusType.Canceled);
        }
    }
}