using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Domain;
using KanbanBoard.Services;
using KanbanBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Reflection.Metadata;

namespace KanbanBoard.Tests.Mocks
{
    public static class MockRepositories
    {
        private static Mock<KanbanContext> MockToDoDbSet()
        {
            var toDoItems = new List<ToDo>()
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
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ToDo>>();
            mockSet.As<IQueryable<ToDo>>().Setup(m => m.Provider).Returns(toDoItems.Provider);
            mockSet.As<IQueryable<ToDo>>().Setup(m => m.Expression).Returns(toDoItems.Expression);
            mockSet.As<IQueryable<ToDo>>().Setup(m => m.ElementType).Returns(toDoItems.ElementType);
            mockSet.As<IQueryable<ToDo>>().Setup(m => m.GetEnumerator()).Returns(() => toDoItems.GetEnumerator());

            var mockContext = new Mock<KanbanContext>();
            mockContext.Setup(c => c.ToDos).Returns(mockSet.Object);
            mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            return mockContext;
        }
        public static BoardService GetToDoService()
        {
            var mockContext = MockToDoDbSet();

            var mockRepo = new BoardService(mockContext.Object);

            return mockRepo;
        }
    }
}