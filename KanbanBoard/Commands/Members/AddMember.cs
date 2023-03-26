using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace KanbanBoard.Commands.Members
{
    public class AddMember
    {
        public record Command(string Name, string Email) : IRequest<OperationResult>;

        //Handler

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IMemberService _memberService;

            public Handler(IMemberService memberService)
            {
                _memberService = memberService;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _memberService.AddMember(request.Name,request.Email);
            }
        }
    }
}
