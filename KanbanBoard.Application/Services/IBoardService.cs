using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;

namespace KanbanBoard.Application.Services;

/// <summary>
/// Service interface for managing Kanban boards, tasks, and members.
/// Provides methods for board creation, item management, member assignment, and status updates.
/// </summary>
public interface IBoardService
{
    /// <summary>
    /// Create board with given name.
    /// </summary>
    /// <param name="name">New board name.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> CreateBoard(string name);

    /// <summary>
    /// Adds ToDo item to board.
    /// </summary>
    /// <param name="boardId">ID of board where item is going to be added.</param>
    /// <param name="itemId">ID of ToDo item.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> AddItemToBoard(int boardId, int itemId);

    /// <summary>
    /// Creates task and adds it to board immediately.
    /// </summary>
    /// <param name="boardId">ID of board where item is going to be added.</param>
    /// <param name="taskName">Name of new task.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> CreateItemInBoard(int boardId, string taskName);

    /// <summary>
    /// Get board with members and tasks.
    /// </summary>
    /// <param name="boardId">ID of board.</param>
    /// <returns>Board with members and tasks.</returns>
    Task<Result<Board, Error>> GetBoardById(int boardId);

    /// <summary>
    /// Change board name to new one.
    /// </summary>
    /// <param name="boardId">ID of board to update.</param>
    /// <param name="newName">New name of board.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> UpdateBoardName(int boardId, string newName);

    /// <summary>
    /// Delete board with tasks.
    /// </summary>
    /// <param name="boardId">ID of board to be removed.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> DeleteBoard(int boardId);

    /// <summary>
    /// Adds member to board.
    /// </summary>
    /// <param name="boardId">ID of board where new member is going to be added.</param>
    /// <param name="memberId">ID of new member.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> AddMemberToBoard(int boardId, int memberId);

    /// <summary>
    /// Removes member from board and all tasks assigned to them.
    /// </summary>
    /// <param name="boardId">ID of board from which member will be removed.</param>
    /// <param name="memberId">ID of existing member.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> RemoveMemberFromBoard(int boardId, int memberId);

    /// <summary>
    /// Assigns member to task by Id.
    /// </summary>
    /// <param name="taskId">ID of task.</param>
    /// <param name="memberId">ID of existing member.</param>
    /// <returns>Result of an operation with error message in case of failure.</returns>
    Task<UnitResult<Error>> AssignMemberToTask(int taskId, int memberId);

    /// <summary>
    /// Gets Kanban item by its ID.
    /// </summary>
    /// <param name="id">ID of item in database.</param>
    /// <returns>Kanban item of corresponding ID.</returns>
    Task<Result<ToDo, Error>> GetToDoById(int id);

    /// <summary>
    /// Get all tasks from Kanban Board.
    /// </summary>
    /// <returns>List of Kanban items.</returns>
    Task<Result<IList<ToDo>, Error>> GetAllTasks();

    /// <summary>
    /// Adds new item to Kanban Board.
    /// </summary>
    /// <param name="name">Description of item.</param>
    /// <returns>ID of added task if successful.</returns>
    Task<Result<int, Error>> AddToDo(string name);

    /// <summary>
    /// Change status of item.
    /// </summary>
    /// <param name="id">Item ID.</param>
    /// <param name="status">Type of status.</param>
    /// <returns>Number of affected entries.</returns>
    Task<Result<int, Error>> ChangeStatus(int id, StatusType status);
}
