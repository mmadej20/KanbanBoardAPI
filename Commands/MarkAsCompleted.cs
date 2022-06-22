using KanbanBoard.Enums;
using KanbanBoard.Repositories;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands
{
    public static class MarkAsCompleted
    {
        public record Command(int Id) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IKanbanService _kanbanService;

            public Handler(IKanbanService kanbanService)
            {
                _kanbanService = kanbanService;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _kanbanService.ChangeStatus(request.Id, StatusType.Completed);
            }
        }
    }
}