using AutoMapper;
using KanbanBoard.Application.Mapping;
using KanbanBoard.DataAccess;
using KanbanBoard.DataAccess.Entities;
using KanbanBoard.Domain.Enums;
using KanbanBoard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace KanbanBoard.IntegrationTests.DatabaseFixture
{
    public class KanbanDatabaseFixture
    {
        public static Guid FirstBoardGuid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static Guid SecondBoardGuid = Guid.Parse("22222222-1111-1111-1111-111111111111");
        public static Guid FirstMemberGuid = Guid.Parse("11111111-2222-1111-1111-111111111111");
        public static Guid SecondMemberGuid = Guid.Parse("22222222-2222-1111-1111-111111111111");
        public static Guid FirstTaskGuid = Guid.Parse("11111111-2222-3333-1111-111111111111");
        public static Guid SecondTaskGuid = Guid.Parse("22222222-2222-3333-1111-111111111111");
        public static Guid ThirdTaskGuid = Guid.Parse("33333333-3333-3333-1111-111111111111");

        private const string _connectionString = @"Server=(localdb)\mssqllocaldb;Database=KanbanTestDatabase;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public IMapper Mapper { get; }

        public KanbanDatabaseFixture()
        {
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
                            new BoardEntity { Id = FirstBoardGuid, Name = "Board623" },
                            new BoardEntity { Id = Guid.NewGuid(), Name = "BoardToDelete" },
                            new BoardEntity { Id = SecondBoardGuid, Name = "BoardWithTasks" }
                            );
                        context.BoardItems.AddRange(
                            new BoardItemEntity { Id = FirstTaskGuid, Name = "TaskInBoardToDelete", Status = StatusType.ToDo, BoardId = FirstBoardGuid },
                            new BoardItemEntity { Id = SecondTaskGuid, Name = "Task432", Status = StatusType.ToDo, BoardId = SecondBoardGuid },
                            new BoardItemEntity { Id = ThirdTaskGuid, Name = "Task999", Status = StatusType.OnHold, BoardId = SecondBoardGuid }
                            );
                        context.Members.AddRange(
                            new MemberEntity { Id = FirstMemberGuid, MemberName = "User1", Email = "user1@liamg.com" },
                            new MemberEntity { Id = SecondMemberGuid, MemberName = "DeleteMe", Email = "deleted@liamg.com" }
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
