using KanbanBoard.DataAccess.Entities;

public class BoardMemberEntity
{
    public int BoardId { get; set; }
    public BoardEntity Board { get; set; } = null!;

    public int MemberId { get; set; }
    public MemberEntity Member { get; set; } = null!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsOwner { get; set; }

}