using System;

namespace KanbanBoard.Api.Contracts.Board.Reqeusts
{
    public class RemoveMemberFromBoardRequest
    {
        public Guid BoardId { get; set; }
        public Guid MemberId { get; set; }
    }
}
