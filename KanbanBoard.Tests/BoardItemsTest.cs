using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Services;
using KanbanBoard.Services.Interfaces;
using KanbanBoard.Tests.DatabaseFixture;
using Moq;

namespace KanbanBoard.Tests
{
    public class BoardItemsTest : IClassFixture<KanbanDatabaseFixture>
    {
        //private readonly BoardItemService _mockService;

        private readonly BoardItemService _boardItemService;

        public BoardItemsTest(KanbanDatabaseFixture fixture)
        {
            //_mockService = Mocks.MockRepositories.GetToDoService();
            _boardItemService = FixtureDatabase.GetBoardItemServiceWithFixtureDatabase(fixture);
        }

        [Fact]
        public void TaskShouldBeAddedToRepository()
        {
            var result = _boardItemService.AddToDo("TestTask");

            Assert.True(result.Result.IsSuccesfull);
        }

        [Fact]
        public void GetAllTasksShouldReturnListOfToDo()
        {
            Assert.IsType<List<ToDo>>(_boardItemService.GetAllTasks().Result);
        }

        [Fact]
        public void StatusShouldBeChangeToInProgress()
        {
            _boardItemService.ChangeStatus(1, StatusType.InProgress).GetAwaiter();
            var inProgressTask = _boardItemService.GetToDoById(1).Result;
            Assert.StartsWith("InProgress", inProgressTask.Status.ToString());
        }
    }
}