using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string MemberName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        public List<Board> Boards { get; set; }
    }
}
