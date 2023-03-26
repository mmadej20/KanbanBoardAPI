using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace KanbanBoard.Commands.Boards
{
    public class AssignMemberToTask
    {
        public record Command(int TaskId, int MemberId) : IRequest<OperationResult>;

        //Handler

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IBoardService _boardService;

            public Handler(IBoardService boardService)
            {
                _boardService = boardService;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _boardService.AssignMemberToTask(request.TaskId, request.MemberId);
            }
        }
    }
}
