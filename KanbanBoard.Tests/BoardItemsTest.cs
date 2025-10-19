using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using KanbanBoard.Infrastructure.Services;
using KanbanBoard.IntegrationTests.DatabaseFixture;
using Shouldly;

namespace KanbanBoard.IntegrationTests
{
    [ClassDataSource(typeof(KanbanDatabaseFixture))]
    public class BoardItemsTest
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private static BoardService _boardService;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Before(Class)]
        public static void Initialize()
        {
            _boardService = ServicesWithFixtureDatabase.GetBoardService();
        }

        [Test]
        [NotInParallel]
        public async Task TaskShouldBeAddedToRepository()
        {
            var result = await _boardService.AddToDo("TestTask");

            result.IsSuccess.ShouldBeTrue();
        }

        [Test]
        [NotInParallel]
        public async Task GetAllTasksShouldReturnListOfToDo()
        {
            var tasks = await _boardService.GetAllTasks();
            tasks.IsSuccess.ShouldBe(true);
            tasks.Value.ShouldBeOfType<List<BoardItem?>>();
        }

        [Test]
        [NotInParallel]
        public async Task StatusShouldBeChangeToInProgress()
        {
            var result = await _boardService.ChangeStatus(KanbanDatabaseFixture.FirstTaskGuid, StatusType.InProgress);

            result.IsSuccess.ShouldBe(true);

            var inProgressTask = await _boardService.GetToDoById(KanbanDatabaseFixture.FirstTaskGuid);

            inProgressTask.IsSuccess.ShouldBe(true);
            inProgressTask.Value.Status.ToString().ShouldBe(StatusType.InProgress.ToString());
        }
    }
}