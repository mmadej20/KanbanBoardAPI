using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.Members.Queries
{
    public class GetMemberById
    {
        public record Query(Guid Id) : IRequest<Result<Member, Error>>;

        //Handler
        public class Handler(IMemberService memberService) : IRequestHandler<Query, Result<Member, Error>>
        {
            private readonly IMemberService _memberService = memberService;

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
