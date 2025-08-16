using CSharpFunctionalExtensions;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Domain.Errors;
using KanbanBoard.Models;
using KanbanBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Services;

public class BoardService(KanbanContext kanbanContext) : IBoardService
{
    private readonly KanbanContext _repository = kanbanContext;

    public async Task<UnitResult<Error>> CreateBoard(string name)
    {
        await _repository.Boards.AddAsync(new Board { Name = name });

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.BoardCreateFailure;
    }

    public async Task<UnitResult<Error>> DeleteBoard(int boardId)
    {
        var boardToRemove = await GetBoardById(boardId);

        if (boardToRemove.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        _repository.Boards.Remove(boardToRemove.Value);

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> AddMemberToBoard(int boardId, int memberId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var memberToAdd = await _repository.Members.FindAsync(memberId);

        if (memberToAdd == null)
        {
            return BoardServiceErrors.MemberNotFound(memberId);
        }

        if (boardToUpdate.Value.Members.Any(x => x.MemberName == memberToAdd.MemberName))
        {
            return BoardServiceErrors.MemberAlreadyExists(memberId);
        }

        if (boardToUpdate.Value.Members == null)
        {
            boardToUpdate.Value.Members = [memberToAdd];
        }
        else
        {
            boardToUpdate.Value.Members.Add(memberToAdd);
        }

        if ((await UpdateBoard(boardToUpdate.Value)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> RemoveMemberFromBoard(int boardId, int memberId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var memberToRemove = await _repository.Members.FindAsync(memberId);

        if (memberToRemove == null)
        {
            return BoardServiceErrors.MemberNotFound(memberId);
        }

        var removeResult = boardToUpdate.Value.Members.Remove(memberToRemove);

        if (!removeResult)
        {
            return BoardServiceErrors.GenericError;
        }

        if ((await UpdateBoard(boardToUpdate.Value)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> AssignMemberToTask(int taskId, int memberId)
    {
        var taskToAssign = await GetToDoById(taskId);

        if (taskToAssign.IsFailure)
        {
            return taskToAssign.Error;
        }

        var memberToAssign = await _repository.Members.Include(m => m.Boards).FirstOrDefaultAsync(x => x.Id == memberId);

        if (memberToAssign == null)
        {
            return BoardServiceErrors.MemberNotFound(memberId);
        }

        var isMemberAssignedToBoard = memberToAssign.Boards.Any(x => x.Id == taskToAssign.Value.BoardId);

        if (!isMemberAssignedToBoard)
        {
            return BoardServiceErrors.MemberIsNotAssignedToBoard(memberId);
        }

        taskToAssign.Value.AssignedMember = memberToAssign;

        _repository.ToDos.Update(taskToAssign.Value);
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> UpdateBoardName(int boardId, string newName)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        boardToUpdate.Value.Name = newName;

        if ((await UpdateBoard(boardToUpdate.Value)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> AddItemToBoard(int boardId, int itemId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var itemToAdd = await _repository.ToDos.FindAsync(itemId);

        if (itemToAdd == null)
        {
            return BoardServiceErrors.ItemNotFound(itemId);
        }

        boardToUpdate.Value.ToDoItems.Add(itemToAdd);

        if ((await UpdateBoard(boardToUpdate.Value)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> CreateItemInBoard(int boardId, string taskName)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var newTaskId = await AddToDo(taskName);
        var newTaskObject = await _repository.ToDos.FindAsync(newTaskId);

        if (newTaskId.IsFailure)
        {
            return newTaskId.Error;
        }

        var result = await AddItemToBoard(boardToUpdate.Value, newTaskObject);

        return result;
    }

    public async Task<Result<Board, Error>> GetBoardById(int boardId)
    {
        var board = await _repository.Boards.FindAsync(boardId);

        if (board == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        return board;
    }

    public async Task<Result<int, Error>> AddToDo(string name)
    {
        var addedTask = await _repository.ToDos.AddAsync(new ToDo { Status = StatusType.ToDo, Name = name });
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return addedTask.Entity.Id;
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<Result<int, Error>> ChangeStatus(int id, StatusType status)
    {
        _repository.ToDos.FirstOrDefault(i => i.Id == id).Status = status;
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return affectedEntries;
        }

        return BoardServiceErrors.GenericError;
    }

    public async Task<Result<IList<ToDo>, Error>> GetAllTasks()
    {
        var toDosList = await _repository.ToDos.ToListAsync();

        if (toDosList == null || !toDosList.Any())
        {
            return BoardServiceErrors.NoTasksFound;
        }

        return toDosList;
    }

    public async Task<Result<ToDo, Error>> GetToDoById(int id)
    {
        var todo = await _repository.ToDos.FirstOrDefaultAsync(x => x.Id == id);
        if (todo == null)
        {
            return BoardServiceErrors.ItemNotFound(id);
        }

        return todo;
    }

    private async Task<UnitResult<Error>> AddItemToBoard(Board board, ToDo taskObject)
    {
        board.ToDoItems.Add(taskObject);

        if ((await UpdateBoard(board)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <returns>Result of operation and affected entries of update command. Error message if operation fails.</returns>
    private async Task<Result<int, Error>> UpdateBoard(Board boardToUpdate)
    {
        _repository.Boards.Update(boardToUpdate);

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return affectedEntries;
        }

        return BoardServiceErrors.GenericError;
    }
}
