using KanbanBoard.Application.Services;
using KanbanBoard.IntegrationTests.DatabaseFixture;
using Shouldly;

namespace KanbanBoard.IntegrationTests
{
    [ClassDataSource(typeof(KanbanDatabaseFixture))]
    public class BoardsTest
    {
        private static IBoardService _boardService;

        [Before(Class)]
        public static void Initialize()
        {
            _boardService = ServicesWithFixtureDatabase.GetBoardService();
        }

        [Test]
        [NotInParallel]
        [Arguments("Board created by test")]
        public async Task CreateBoardShouldBeSuccessful(string myBoardName)
        {
            var returnValue = await _boardService.CreateBoard(myBoardName);

            returnValue.IsSuccess.ShouldBe(true);
        }

        [Test]
        [NotInParallel]
        [Arguments("Task assigned to board of id 1")]
        public async Task TaskShouldBeAssignedToBoard(string taskName)
        {
            await _boardService.CreateItemInBoard(KanbanDatabaseFixture.FirstBoardGuid, taskName);
            var boardWithTask = await _boardService.GetBoardById(KanbanDatabaseFixture.FirstBoardGuid);

            boardWithTask.IsSuccess.ShouldBe(true);
            var taskFromBoard = boardWithTask.Value.BoardItems?.FirstOrDefault(x => x.Name == taskName);

            taskFromBoard?.Name.ShouldBe(taskName);
        }

        [Test]
        [NotInParallel]
        public async Task DeleteBoardShouldDeleteAssignedTasks()
        {
            var board = await _boardService.GetBoardById(KanbanDatabaseFixture.SecondBoardGuid);

            board.IsSuccess.ShouldBe(true);
            var taskAssignedToBoard = board.Value.BoardItems?.FirstOrDefault();

            await _boardService.DeleteBoard(KanbanDatabaseFixture.SecondBoardGuid);

            var result = await _boardService.GetToDoById(taskAssignedToBoard!.Id);

            result.IsFailure.ShouldBe(true);
        }
    }
}
