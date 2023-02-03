using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public StatusType Status { get; set; }

        public int? BoardId { get; set; }
        [JsonIgnore]
        public Board? Board { get; set; }
        public int? AssignedMemberId { get; set; }
        [JsonIgnore]
        public Member? AssignedMember { get; set; }
    }
}