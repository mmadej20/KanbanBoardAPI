using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Application.Members.Queries
{
    public class GetMemberById
    {
        public record Query(int Id) : IRequest<Result<Member, Error>>;

        //Handler
        public class Handler : IRequestHandler<Query, Result<Member, Error>>
        {
            private readonly IMemberService _memberService;

            public Handler(IMemberService memberService)
            {
                _memberService = memberService;
            }

            public async Task<Result<Member, Error>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _memberService.GetMemberById(request.Id);
                if (result.IsSuccess)
                {
                    return result.Value;
                }

                return result.Error;
            }
        }
    }
}
