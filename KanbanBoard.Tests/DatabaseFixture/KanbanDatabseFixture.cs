using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Tests.DatabaseFixture
{
    public class KanbanDatabaseFixture
    {
        private const string _connectionString = @"Server=(localdb)\mssqllocaldb;Database=KanbanTestDatabase;Trusted_Connection=True";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public KanbanDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.Boards.AddRange(
                            new Board { Name = "Board623" },
                            new Board { Name = "BoardToDelete" }
                            );
                        context.ToDos.AddRange(
                            new ToDo { Name = "Task432", Status = StatusType.ToDo, BoardId = 2 },
                            new ToDo { Name = "Task999", Status = StatusType.OnHold, BoardId = 2 }
                            );
                        context.Members.AddRange(
                            new Member { MemberName = "User1", Email = "user1@liamg.com" },
                            new Member { MemberName = "DeleteMe", Email = "deleted@liamg.com"}
                            );
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public KanbanContext CreateContext()
            => new(
                new DbContextOptionsBuilder<KanbanContext>()
                    .UseSqlServer(_connectionString)
                    .EnableThreadSafetyChecks()
                    .Options);
    }

    public static class ServicesWithFixtureDatabase
    {
        public static BoardService GetBoardService()
        {
            var context = new KanbanDatabaseFixture().CreateContext();

            var mockRepo = new BoardService(context);
            return mockRepo;
        }

        public static MemberService GetMemberService()
        {
            var context = new KanbanDatabaseFixture().CreateContext();

            var mockRepo = new MemberService(context);
            return mockRepo;
        }
    }
}
