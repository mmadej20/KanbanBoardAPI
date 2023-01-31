using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Services.Interfaces;
using Moq;

namespace KanbanBoard.Tests.Mocks
{
    public static class MockRepositories
    {
        public static Mock<IKanbanService> GetKanbanRepository()
        {
            var kanbanItems = new List<ToDo>()
            {
                new ToDo()
                {
                    Id = 10,
                    Name = "Create Tests",
                    Status = StatusType.InProgress
                },
                new ToDo()
                {
                    Id = 20,
                    Name = "Validate",
                    Status = StatusType.ToDo
                }
            };

            var mockRepo = new Mock<IKanbanService>();
            mockRepo.Setup(r => r.GetAllTasks()).ReturnsAsync(kanbanItems);

            mockRepo.Setup(r => r.AddToDo(It.IsAny<string>())).ReturnsAsync((bool isCreated) =>
            {
                return isCreated;
            });

            return mockRepo;
        }
    }
}