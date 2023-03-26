using DataAccess.Models;
using KanbanBoard.Services;
using KanbanBoard.Services.Interfaces;
using KanbanBoard.Tests.DatabaseFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Tests
{
    [CollectionDefinition("Boards",DisableParallelization = true)]
    public class BoardsTest : IClassFixture<KanbanDatabaseFixture>
    {
        private readonly IBoardService _boardService;

        public BoardsTest()
        {
            _boardService = ServicesWithFixtureDatabase.GetBoardItemService();
        }

        [Theory]
        [InlineData("Board created by test")]
        public async Task CreateBoardShouldBeSuccessful(string myBoardName)
        {
            var returnValue = await _boardService.CreateBoard(myBoardName);

            Assert.True(returnValue.IsSuccesfull);
        }

        [Theory]
        [InlineData(1, "Task assigned to board of id 1")]
        public async Task TaskShouldBeAssignedToBoard(int boardId, string taskName)
        {
            await _boardService.CreateItemInBoard(boardId, taskName);
            var boardWithTask = await _boardService.GetBoardById(boardId);
            var taskFromBoard = boardWithTask.ToDoItems.FirstOrDefault(x => x.Name == taskName);

            Assert.Equal(taskFromBoard?.Name, taskName);
        }

        [Fact]
        public async Task DeleteBoardShouldDeleteAssignedTasks()
        {
            var board = await _boardService.GetBoardById(2);
            var taskAssignedToBoard = board.ToDoItems.FirstOrDefault();

            await _boardService.DeleteBoard(2);
            
            var result = await _boardService.GetToDoById(taskAssignedToBoard!.Id);

            Assert.Null(result);
        }
    }
}
