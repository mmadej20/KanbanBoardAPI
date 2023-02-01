using DataAccess.Enums;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands.BoardItems;

public class MarkAsCompleted
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
            return await _kanbanService.ChangeStatus(request.Id, StatusType.Completed);
        }
    }
}