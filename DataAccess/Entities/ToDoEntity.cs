using KanbanBoard.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KanbanBoard.DataAccess.Entities
{
    public class ToDoEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public required StatusType Status { get; set; }

        public int? BoardId { get; set; }

        [JsonIgnore]
        public BoardEntity? Board { get; set; }

        public int? AssignedMemberId { get; set; }

        [JsonIgnore]
        public MemberEntity? AssignedMember { get; set; }
    }
}