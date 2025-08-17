namespace KanbanBoard.Domain.Entities
{
    public class Board
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<ToDo>? ToDoItems { get; set; }

        public List<Member>? Members { get; set; }
    }
}
