using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Application.Members.Commands
{
    public class DeleteMember
    {
        public record Command(int Id) : IRequest<UnitResult<Error>>;

        //Handler

        public class Handler : IRequestHandler<Command, UnitResult<Error>>
        {
            private readonly IMemberService _memberService;

            public Handler(IMemberService memberService)
            {
                _memberService = memberService;
            }

            public async Task<UnitResult<Error>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _memberService.DeleteMember(request.Id);
            }
        }
    }
}
