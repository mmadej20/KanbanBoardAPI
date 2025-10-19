using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;

namespace KanbanBoard.Application.Boards.Commands
{
    public class AssignMemberToBoardItem
    {
        public record Command(Guid TaskId, Guid MemberId) : IRequest<UnitResult<Error>>;

        //Handler

        public class Handler(IBoardService boardService) : IRequestHandler<Command, UnitResult<Error>>
        {
            private readonly IBoardService _boardService = boardService;

            public async Task<UnitResult<Error>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _boardService.AssignMemberToTask(request.TaskId, request.MemberId);
            }
        }
    }
}
