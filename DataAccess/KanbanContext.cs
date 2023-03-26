using DataAccess.Enums;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class KanbanContext : DbContext
    {
        public KanbanContext()
        {

        }
        public KanbanContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<ToDo> ToDos { get; set; }

        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>()
                .Property(s => s.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Board>()
                .Navigation(n => n.ToDoItems).AutoInclude();

            modelBuilder.Entity<Board>()
                .Navigation(n => n.Members).AutoInclude();

            modelBuilder.Entity<Board>()
                .HasMany(m => m.ToDoItems)
                .WithOne(o => o.Board)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Member>()
                .HasMany(t => t.ToDos)
                .WithOne(t => t.AssignedMember)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
