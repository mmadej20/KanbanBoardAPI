using System;

namespace KanbanBoard.Api.Contracts.Board.Reqeusts
{
    public class CreateItemInBoardRequest
    {
        public Guid BoardId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
