namespace KanbanBoard.DataAccess.Entities
{
    public class BoardMemberEntity
    {
        public Guid BoardId { get; set; }
        public BoardEntity Board { get; set; } = null!;

        public Guid MemberId { get; set; }
        public MemberEntity Member { get; set; } = null!;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsOwner { get; set; }
    }
}