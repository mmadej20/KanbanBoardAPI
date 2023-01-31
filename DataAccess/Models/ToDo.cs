using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

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
    }
}