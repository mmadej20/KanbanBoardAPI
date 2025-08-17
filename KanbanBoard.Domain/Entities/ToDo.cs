using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Domain.Entities
{
    public class ToDo
    {
        public int Id { get; set; }

        public int BoardId { get; set; }

        public int? AssignedMemberId { get; set; }

        public required string Name { get; set; }

        public required StatusType Status { get; set; }
    }
}
