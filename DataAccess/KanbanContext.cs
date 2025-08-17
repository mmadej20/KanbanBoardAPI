using KanbanBoard.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.DataAccess
{
    public class KanbanContext : DbContext
    {
        public KanbanContext() { }

        public KanbanContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<ToDoEntity> ToDos { get; set; }

        public virtual DbSet<MemberEntity> Members { get; set; }

        public virtual DbSet<BoardEntity> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoEntity>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<BoardEntity>()
                .HasMany(b => b.ToDoItems)
                .WithOne(t => t.Board)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberEntity>()
                .HasMany(m => m.ToDos)
                .WithOne(t => t.AssignedMember)
                .HasForeignKey(t => t.AssignedMemberId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BoardMemberEntity>()
                .HasKey(bm => new { bm.BoardId, bm.MemberId });

            modelBuilder.Entity<BoardMemberEntity>()
                .HasOne(bm => bm.Board)
                .WithMany(b => b.BoardMembers)
                .HasForeignKey(bm => bm.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BoardMemberEntity>()
                .HasOne(bm => bm.Member)
                .WithMany(m => m.BoardMembers)
                .HasForeignKey(bm => bm.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            //TODO: Check performance implications of AutoInclude

            modelBuilder.Entity<BoardEntity>()
                .Navigation(b => b.ToDoItems).AutoInclude();

            modelBuilder.Entity<BoardEntity>()
                .Navigation(b => b.BoardMembers).AutoInclude();

            modelBuilder.Entity<BoardMemberEntity>()
                .Navigation(bm => bm.Member).AutoInclude();

            base.OnModelCreating(modelBuilder);
        }

    }
}
