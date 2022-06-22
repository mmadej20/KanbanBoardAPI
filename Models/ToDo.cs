using KanbanBoard.Enums;

namespace KanbanBoard.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StatusType Status { get; set; }
    }
}