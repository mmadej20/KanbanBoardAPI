using System;

namespace KanbanBoard.Api.Contracts.Member.Responses
{
    public class MemberResponse
    {
        public Guid Id { get; set; }

        public string MemberName { get; set; }

        public string Email { get; set; }
    }
}
