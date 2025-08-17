namespace KanbanBoard.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }

        public required string MemberName { get; set; }

        public required string Email { get; set; }
    }
}
