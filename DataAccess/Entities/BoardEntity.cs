using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.DataAccess.Entities
{
    public class BoardEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public List<ToDoEntity>? ToDoItems { get; set; }

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
