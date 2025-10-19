using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Domain.Entities
{
    public class BoardItem
    {
        public Guid Id { get; set; }

        public Guid BoardId { get; set; }

        public Guid? AssignedMemberId { get; set; }

        public required string Name { get; set; }

        public required StatusType Status { get; set; }
    }
}
