using KanbanBoard.Models;
using KanbanBoard.Repositories;
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
                    Status = Enums.StatusType.InProgress
                },
                new ToDo()
                {
                    Id = 20,
                    Name = "Validate",
                    Status = Enums.StatusType.ToDo
                }
            };

            var mockRepo = new Mock<IKanbanService>();
            mockRepo.Setup(r => r.GetAllTasks()).ReturnsAsync(kanbanItems);

            mockRepo.Setup(r => r.AddToDo(It.IsAny<string>())).ReturnsAsync((int idOfTask) =>
            {
                return idOfTask;
            });

            return mockRepo;
        }
    }
}