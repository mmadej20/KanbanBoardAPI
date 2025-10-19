namespace KanbanBoard.Domain.Entities
{
    public class Board
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public List<BoardItem>? BoardItems { get; set; }

        public List<Member>? Members { get; set; }
    }
}
