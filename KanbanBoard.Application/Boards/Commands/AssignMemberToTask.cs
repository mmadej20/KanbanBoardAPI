using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Application.Boards.Commands
{
    public class AssignMemberToTask
    {
        public record Command(int TaskId, int MemberId) : IRequest<UnitResult<Error>>;

        //Handler

        public class Handler : IRequestHandler<Command, UnitResult<Error>>
        {
            private readonly IBoardService _boardService;

            public Handler(IBoardService boardService)
            {
                _boardService = boardService;
            }

            public async Task<UnitResult<Error>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _boardService.AssignMemberToTask(request.TaskId, request.MemberId);
            }
        }
    }
}
