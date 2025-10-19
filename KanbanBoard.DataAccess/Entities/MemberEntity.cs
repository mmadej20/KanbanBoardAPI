using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.DataAccess.Entities
{
    public class MemberEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string MemberName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        public List<BoardMemberEntity> BoardMembers { get; set; } = [];

        public List<BoardItemEntity>? BoardItems { get; set; }
    }
}
