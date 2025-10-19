using System;

namespace KanbanBoard.Api.Contracts.Board.Reqeusts
{
    public class AddMemberToBoardRequest
    {
        public Guid BoardId { get; set; }
        public Guid MemberId { get; set; }
        public string MemberEmail { get; set; } = string.Empty;
    }
}
