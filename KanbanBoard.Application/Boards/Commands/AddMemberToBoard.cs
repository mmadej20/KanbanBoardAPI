using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.Boards.Commands
{
    public class AddMemberToBoard
    {
        public record Command(Guid BoardId, Guid MemberId, string MemberEmail = "") : IRequest<UnitResult<Error>>;

        //Handler

        public class Handler(IBoardService boardService, IMemberService memberService) : IRequestHandler<Command, UnitResult<Error>>
        {
            private readonly IBoardService _boardService = boardService;
            private readonly IMemberService _memberService = memberService;

            public async Task<UnitResult<Error>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.MemberId == Guid.Empty && string.IsNullOrWhiteSpace(request.MemberEmail))
                {
                    return UnitResult.Failure(Errors.BoardServiceErrors.InvalidMemberInformation);
                }

                Result<Member, Error> getMemberResult = new();
                if (request.MemberId == Guid.Empty)
                {
                    getMemberResult = await _memberService.GetMemberByEmail(request.MemberEmail);
                }

                if (getMemberResult.IsSuccess)
                {
                    return await _boardService.AddMemberToBoard(request.BoardId, getMemberResult.Value.Id);
                }

                return await _boardService.AddMemberToBoard(request.BoardId, request.MemberId);
            }
        }
    }
}
