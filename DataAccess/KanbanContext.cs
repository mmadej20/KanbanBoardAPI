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
        public KanbanContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>()
                .Property(s => s.Status)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
