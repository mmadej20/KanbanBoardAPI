using KanbanBoard.Domain.Enums;
using System;

namespace KanbanBoard.Api.Contracts.BoardItem.Responses
{
    public class BoardItemResponse
    {
        public Guid Id { get; set; }

        public Guid BoardId { get; set; }

        public Guid? AssignedMemberId { get; set; }

        public string Name { get; set; }

        public StatusType Status { get; set; }
    }
}
