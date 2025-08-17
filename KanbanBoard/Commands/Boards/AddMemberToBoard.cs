using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Commands.Boards
{
    public class AddMemberToBoard
    {
        public record Command(int BoardId, int MemberId) : IRequest<UnitResult<Error>>;

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
                return await _boardService.AddMemberToBoard(request.BoardId, request.MemberId);
            }
        }
    }
}
