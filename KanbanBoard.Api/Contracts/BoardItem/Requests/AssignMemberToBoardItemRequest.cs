using System;

namespace KanbanBoard.Api.Contracts.BoardItem.Requests
{
    public class AssignMemberToBoardItemRequest
    {
        public Guid TaskId { get; set; }

        public Guid MemberId { get; set; }
    }
}
