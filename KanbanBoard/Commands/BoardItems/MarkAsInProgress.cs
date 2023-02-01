using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using DataAccess.Enums;

namespace KanbanBoard.Commands.BoardItems;

public class MarkAsInProgress
{
    public record Command(int Id) : IRequest<int>;

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IKanbanService _kanbanService;

        public Handler(IKanbanService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.ChangeStatus(request.Id, StatusType.InProgress);
        }
    }
}