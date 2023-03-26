using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services.Interfaces;

public interface IBoardService
{
    /// <summary>
    /// Create board with given name
    /// </summary>
    /// <param name="name">New board name</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> CreateBoard(string name);

    /// <summary>
    /// Adds ToDo item to board
    /// </summary>
    /// <param name="boardId">ID of board where item is going to be added</param>
    /// <param name="itemId">ID of ToDo item</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> AddItemToBoard(int boardId, int itemId);

    /// <summary>
    /// Creates task and adds it to board immediatly
    /// </summary>
    /// <param name="boardId">ID of board where item is going to be added</param>
    /// <param name="taskName">Name of new task</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> CreateItemInBoard(int boardId, string taskName);

    /// <summary>
    /// Get board with members and tasks
    /// </summary>
    /// <param name="boardId">ID of board</param>
    /// <returns>Board with members and tasks</returns>
    Task<Board> GetBoardById(int boardId);

    /// <summary>
    /// Change board name to new one
    /// </summary>
    /// <param name="boardId">ID of board to update</param>
    /// <param name="newName">New name of board</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> UpdateBoardName(int boardId, string newName);

    /// <summary>
    /// Delete board with tasks
    /// </summary>
    /// <param name="boardId">ID of board to be removed</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> DeleteBoard(int boardId);

    /// <summary>
    /// Adds member to board
    /// </summary>
    /// <param name="boardId">ID of board where new member is going to be add</param>
    /// <param name="memberId">ID of new member</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> AddMemberToBoard(int boardId, int memberId);

    /// <summary>
    /// Removes member from board and all task assigned to him
    /// </summary>
    /// <param name="boardId">ID of board from member will be removed</param>
    /// <param name="memberId">ID of existing member</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> RemoveMemberFromBoard(int boardId, int memberId);

    /// <summary>
    /// Assigns member to task by Id
    /// </summary>
    /// <param name="taskId">ID of task</param>
    /// <param name="memberId">ID of existing member</param>
    /// <returns>Result of an operation</returns>
    Task<OperationResult> AssignMemberToTask(int taskId,int memberId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID of item in database</param>
    /// <returns>Kanban item of corressponding ID</returns>
    Task<ToDo> GetToDoById(int id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>List of Kanban items</returns>
    Task<IList<ToDo>> GetAllTasks();

    /// <summary>
    /// Adds new item to Kanban Board
    /// </summary>
    /// <param name="name">Description of item</param>
    /// <returns>ID of added task if successfull otherwise '-1'</returns>
    Task<int> AddToDo(string name);

    /// <summary>
    /// Change status of item 
    /// </summary>
    /// <param name="id">Item ID</param>
    /// <param name="status">Type of status</param>
    /// <returns>Number of affected entries</returns>
    Task<OperationResult> ChangeStatus(int id, StatusType status);
}
