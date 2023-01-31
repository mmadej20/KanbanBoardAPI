using MediatR;
using System.Threading.Tasks;
using System.Threading;
using KanbanBoard.Services.Interfaces;
using DataAccess.Enums;

namespace KanbanBoard.Commands
{
    public class MarkAsCancelled
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