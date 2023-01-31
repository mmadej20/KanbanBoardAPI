using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Services.Interfaces;
using Moq;

namespace KanbanBoard.Tests
{
    public class GetToDoItemsTest
    {
        private readonly Mock<IKanbanService> _mockRepo;

        public GetToDoItemsTest()
        {
            _mockRepo = Mocks.MockRepositories.GetKanbanRepository();
        }

        [Fact]
        public void GetAllTasksShouldReturnListOfToDo()
        {
            Assert.IsType<List<ToDo>>(_mockRepo.Object.GetAllTasks().Result);
        }

        [Fact]
        public void StatusShouldBeChangeToInProgress()
        {
            _mockRepo.Object.ChangeStatus(1, StatusType.InProgress);
            var inProgressTask = _mockRepo.Object.GetToDoById(1).Result;
            Assert.StartsWith("InProgress", inProgressTask.Status.ToString());
        }
    }
}