using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;

namespace KanbanBoard.Application.Members.Commands
{
    public class UpdateMember
    {
        public record Command(Guid Id, string Name, string Email) : IRequest<UnitResult<Error>>;

        //Handler

        public class Handler(IMemberService memberService) : IRequestHandler<Command, UnitResult<Error>>
        {
            private readonly IMemberService _memberService = memberService;

            public async Task<UnitResult<Error>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _memberService.UpdateMember(request.Id, request.Name, request.Email);
            }
        }
    }
}
