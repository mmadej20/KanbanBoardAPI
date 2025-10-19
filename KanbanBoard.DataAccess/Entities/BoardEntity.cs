using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.DataAccess.Entities
{
    public class BoardEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public List<BoardItemEntity>? BoardItems { get; set; }

        public List<BoardMemberEntity> BoardMembers { get; set; } = [];

        public void AddMember(MemberEntity member)
        {
            BoardMembers.Add(new BoardMemberEntity
            {
                Board = this,
                BoardId = this.Id,
                Member = member,
                MemberId = member.Id
            });
        }
    }
}
