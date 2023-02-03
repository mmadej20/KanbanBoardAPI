using DataAccess.Models;
using KanbanBoard.Domain;
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


}
