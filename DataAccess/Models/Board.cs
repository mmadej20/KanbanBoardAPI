using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public List<ToDo>? ToDoItems { get; set; }

        public List<Member>? Members { get; set; }
    }
}
