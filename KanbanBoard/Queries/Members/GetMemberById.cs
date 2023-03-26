using AutoMapper;
using DataAccess.Models;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace KanbanBoard.Queries.Members
{
    public class GetMemberById
    {
        public record Query(int Id) : IRequest<Member>;

        //Handler
        public class Handler : IRequestHandler<Query, Member>
        {
            private readonly IMemberService _memberService;

            public Handler(IMemberService memberService)
            {
                _memberService = memberService;
            }

            public async Task<Member> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _memberService.GetMemberById(request.Id);
                return result;
            }
        }
    }
}
