using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Services;
using KanbanBoard.Services.Interfaces;
using KanbanBoard.Tests.DatabaseFixture;
using Moq;

namespace KanbanBoard.Tests
{
    [CollectionDefinition("BoardItems", DisableParallelization = true)]
    public class BoardItemsTest : IClassFixture<KanbanDatabaseFixture>
    {
        //private readonly BoardItemService _mockService;

        private readonly BoardService _boardService;

        public BoardItemsTest()
        {
            //_mockService = Mocks.MockRepositories.GetToDoService();
            _boardService = ServicesWithFixtureDatabase.GetBoardItemService();
        }

        [Fact]
        public async Task TaskShouldBeAddedToRepository()
        {
            var result = await _boardService.AddToDo("TestTask");

            Assert.NotEqual(-1, result);
        }

        [Fact]
        public async Task GetAllTasksShouldReturnListOfToDo()
        {
            Assert.IsType<List<ToDo>>(await _boardService.GetAllTasks());
        }

        [Fact]
        public async Task StatusShouldBeChangeToInProgress()
        {
            await _boardService.ChangeStatus(1, StatusType.InProgress);

            var inProgressTask = await _boardService.GetToDoById(1);

            Assert.StartsWith("InProgress", inProgressTask.Status.ToString());
        }
    }
}