using KanbanBoard.Application.Services;
using KanbanBoard.Tests.DatabaseFixture;
using Shouldly;

namespace KanbanBoard.Tests
{
    [ClassDataSource(typeof(KanbanDatabaseFixture))]
    public class BoardsTest
    {
        private static IBoardService _boardService;

        [Before(HookType.Class)]
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
        [Arguments(1, "Task assigned to board of id 1")]
        public async Task TaskShouldBeAssignedToBoard(int boardId, string taskName)
        {
            await _boardService.CreateItemInBoard(boardId, taskName);
            var boardWithTask = await _boardService.GetBoardById(boardId);

            boardWithTask.IsSuccess.ShouldBe(true);
            var taskFromBoard = boardWithTask.Value.ToDoItems?.FirstOrDefault(x => x.Name == taskName);

            taskFromBoard?.Name.ShouldBe(taskName);
        }

        [Test]
        [NotInParallel]
        public async Task DeleteBoardShouldDeleteAssignedTasks()
        {
            var board = await _boardService.GetBoardById(2);

            board.IsSuccess.ShouldBe(true);
            var taskAssignedToBoard = board.Value.ToDoItems?.FirstOrDefault();

            await _boardService.DeleteBoard(2);

            var result = await _boardService.GetToDoById(taskAssignedToBoard!.Id);

            result.IsFailure.ShouldBe(true);
        }
    }
}
