using AutoMapper;
using CSharpFunctionalExtensions;
using KanbanBoard.Application.Boards.Errors;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.DataAccess;
using KanbanBoard.DataAccess.Entities;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Services;

//TODO: Check if .Update is needed in various places, as it might not be necessary if the entity is already being tracked by the context.

/// <summary>
/// Implementation of <see cref="IBoardService"/> for managing Kanban boards.
/// </summary>
/// <param name="kanbanContext"></param>
/// <param name="mapper"></param>
public class BoardService(KanbanContext kanbanContext, IMapper mapper) : IBoardService
{
    private readonly KanbanContext _repository = kanbanContext;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> CreateBoard(string name)
    {
        await _repository.Boards.AddAsync(new BoardEntity { Name = name });

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.BoardCreateFailure;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> DeleteBoard(int boardId)
    {
        var boardToRemove = await _repository.Boards.FindAsync(boardId);

        if (boardToRemove == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        _repository.Boards.Remove(boardToRemove);

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> AddMemberToBoard(int boardId, int memberId)
    {
        var boardToUpdate = await _repository.Boards.FindAsync(boardId);

        if (boardToUpdate == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var memberEntityToAdd = await _repository.Members.FindAsync(memberId);

        if (memberEntityToAdd == null)
        {
            return BoardServiceErrors.MemberNotFound(memberId);
        }

        if (boardToUpdate.BoardMembers.Any(bm => bm.Member.MemberName == memberEntityToAdd.MemberName))
        {
            return BoardServiceErrors.MemberAlreadyExists(memberId);
        }

        boardToUpdate.AddMember(memberEntityToAdd);

        if ((await UpdateBoard(boardToUpdate)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> RemoveMemberFromBoard(int boardId, int memberId)
    {
        var boardToUpdate = await _repository.Boards.FindAsync(boardId);

        if (boardToUpdate == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var boardMemberToRemove = boardToUpdate.BoardMembers
            .FirstOrDefault(bm => bm.MemberId == memberId);

        if (boardMemberToRemove != null)
        {
            boardToUpdate.BoardMembers.Remove(boardMemberToRemove);
        }

        if ((await UpdateBoard(boardToUpdate)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> AssignMemberToTask(int taskId, int memberId)
    {
        var taskToAssign = await _repository.ToDos.Include(item => item.Board).FirstOrDefaultAsync(task => task.Id == taskId);

        if (taskToAssign == null)
        {
            return BoardServiceErrors.ItemNotFound(taskId);
        }

        var memberToAssign = await _repository.Members
            .FirstOrDefaultAsync(x => x.Id == memberId);

        if (memberToAssign == null)
        {
            return BoardServiceErrors.MemberNotFound(memberId);
        }

        var isMemberAssignedToBoard = taskToAssign.Board!.BoardMembers!
            .Any(bm => bm.BoardId == taskToAssign.BoardId);

        if (!isMemberAssignedToBoard)
        {
            return BoardServiceErrors.MemberIsNotAssignedToBoard(memberId);
        }

        taskToAssign.AssignedMember = memberToAssign;

        _repository.ToDos.Update(taskToAssign);
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> UpdateBoardName(int boardId, string newName)
    {
        var boardToUpdate = await _repository.Boards.FindAsync(boardId);

        if (boardToUpdate == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        boardToUpdate.Name = newName;

        if ((await UpdateBoard(boardToUpdate)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> AddItemToBoard(int boardId, int itemId)
    {
        var boardToUpdate = await _repository.Boards.FindAsync(boardId);

        if (boardToUpdate == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var itemEntityToAdd = await _repository.ToDos.FindAsync(itemId);

        if (itemEntityToAdd == null)
        {
            return BoardServiceErrors.ItemNotFound(itemId);
        }

        boardToUpdate.ToDoItems!.Add(itemEntityToAdd);

        if ((await UpdateBoard(boardToUpdate)).IsSuccess)
        {
            return UnitResult.Success<Error>();
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<UnitResult<Error>> CreateItemInBoard(int boardId, string taskName)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate.IsFailure)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var newTaskId = await AddToDo(taskName);

        if (newTaskId.IsFailure)
        {
            return newTaskId.Error;
        }

        var newTaskObject = await _repository.ToDos.FindAsync(newTaskId.Value);

        var result = await AddItemToBoard(boardToUpdate.Value.Id, newTaskObject!.Id);

        return result;
    }

    /// <inheritdoc/>
    public async Task<Result<Board, Error>> GetBoardById(int boardId)
    {
        var boardEntity = await _repository.Boards
            .FirstOrDefaultAsync(b => b.Id == boardId);

        if (boardEntity == null)
        {
            return BoardServiceErrors.BoardNotFound(boardId);
        }

        var board = _mapper.Map<Board>(boardEntity);
        return board;
    }

    /// <inheritdoc/>
    public async Task<Result<int, Error>> AddToDo(string name)
    {
        var addedTask = await _repository.ToDos.AddAsync(new ToDoEntity { Status = StatusType.ToDo, Name = name });
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return addedTask.Entity.Id;
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<Result<int, Error>> ChangeStatus(int id, StatusType status)
    {
        var taskEntity = await _repository.ToDos.FirstOrDefaultAsync(i => i.Id == id);

        if (taskEntity == null)
        {
            return BoardServiceErrors.ItemNotFound(id);
        }

        taskEntity.Status = status;
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return affectedEntries;
        }

        return BoardServiceErrors.GenericError;
    }

    /// <inheritdoc/>
    public async Task<Result<IList<ToDo>, Error>> GetAllTasks()
    {
        var toDosListEntity = await _repository.ToDos.ToListAsync();

        if (toDosListEntity == null || !toDosListEntity.Any())
        {
            return BoardServiceErrors.NoTasksFound;
        }

        var toDosList = _mapper.Map<List<ToDo>>(toDosListEntity);

        return toDosList;
    }

    /// <inheritdoc/>
    public async Task<Result<ToDo, Error>> GetToDoById(int id)
    {
        var todoEntity = await _repository.ToDos.FirstOrDefaultAsync(x => x.Id == id);

        if (todoEntity == null)
        {
            return BoardServiceErrors.ItemNotFound(id);
        }

        var todo = _mapper.Map<ToDo>(todoEntity);

        return todo;
    }

    /// <returns>Result of operation and affected entries of update command. Error message if operation fails.</returns>
    private async Task<Result<int, Error>> UpdateBoard(BoardEntity boardToUpdate)
    {
        _repository.Boards.Update(boardToUpdate); //TODO: CHECK IF ITS NEEDED LATER

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return affectedEntries;
        }

        return BoardServiceErrors.GenericError;
    }
}
