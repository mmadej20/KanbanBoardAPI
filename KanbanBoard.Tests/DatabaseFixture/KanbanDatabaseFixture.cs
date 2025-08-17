using AutoMapper;
using KanbanBoard.Application.Mapping;
using KanbanBoard.DataAccess;
using KanbanBoard.DataAccess.Entities;
using KanbanBoard.Domain.Enums;
using KanbanBoard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace KanbanBoard.Tests.DatabaseFixture
{
    public class KanbanDatabaseFixture
    {
        private const string _connectionString = @"Server=(localdb)\mssqllocaldb;Database=KanbanTestDatabase;Trusted_Connection=True";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public IMapper Mapper { get; }

        public KanbanDatabaseFixture()
        {
            // konfiguracja AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, new NullLoggerFactory());

            Mapper = config.CreateMapper();

            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.Boards.AddRange(
                            new BoardEntity { Name = "Board623" },
                            new BoardEntity { Name = "BoardToDelete" }
                            );
                        context.ToDos.AddRange(
                            new ToDoEntity { Name = "Task432", Status = StatusType.ToDo, BoardId = 2 },
                            new ToDoEntity { Name = "Task999", Status = StatusType.OnHold, BoardId = 2 }
                            );
                        context.Members.AddRange(
                            new MemberEntity { MemberName = "User1", Email = "user1@liamg.com" },
                            new MemberEntity { MemberName = "DeleteMe", Email = "deleted@liamg.com" }
                            );
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public KanbanContext CreateContext()
            => new(new DbContextOptionsBuilder<KanbanContext>()
                       .UseSqlServer(_connectionString)
                       .EnableThreadSafetyChecks()
                       .Options);
    }

    public static class ServicesWithFixtureDatabase
    {
        public static BoardService GetBoardService()
        {
            var context = new KanbanDatabaseFixture();

            var mockRepo = new BoardService(context.CreateContext(), context.Mapper);
            return mockRepo;
        }

        public static MemberService GetMemberService()
        {
            var context = new KanbanDatabaseFixture();

            var mockRepo = new MemberService(context.CreateContext(), context.Mapper);
            return mockRepo;
        }
    }
}
