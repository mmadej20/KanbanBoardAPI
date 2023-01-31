using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using DataAccess.Enums;

namespace KanbanBoard.Commands
{
    public class MarkAsInProgress
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
                return await _kanbanService.ChangeStatus(request.Id, StatusType.InProgress);
            }
        }
    }
}