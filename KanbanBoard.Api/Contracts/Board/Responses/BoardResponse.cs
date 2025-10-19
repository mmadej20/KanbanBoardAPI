using KanbanBoard.Api.Contracts.BoardItem.Responses;
using KanbanBoard.Api.Contracts.Member.Responses;
using System;
using System.Collections.Generic;

namespace KanbanBoard.Api.Contracts.Board.Responses
{
    public class BoardResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public IReadOnlyCollection<BoardItemResponse> ToDoItems { get; set; }
        public IReadOnlyCollection<MemberResponse> Members { get; set; }
    }
}
