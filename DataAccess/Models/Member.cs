﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public List<Board> Boards { get; set; }

        [JsonIgnore]
        public List<ToDo> ToDos { get; set; }
    }
}
