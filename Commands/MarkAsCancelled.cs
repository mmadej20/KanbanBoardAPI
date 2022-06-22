using KanbanBoard.Enums;
using KanbanBoard.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using KanbanBoard.Services.Interfaces;

namespace KanbanBoard.Commands
{
    public static class MarkAsCancelled
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
                return await _kanbanService.ChangeStatus(request.Id, StatusType.Canceled);
            }
        }
    }
}