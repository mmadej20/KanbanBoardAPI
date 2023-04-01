using AutoMapper.Execution;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Commands.BoardItems;
using KanbanBoard.Domain;
using KanbanBoard.Queries.BoardItems;
using KanbanBoard.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Services;

public class BoardService : IBoardService
{
    private readonly KanbanContext _repository;

    public BoardService(KanbanContext kanbanContext)
    {
        _repository = kanbanContext;
    }
    public async Task<OperationResult> CreateBoard(string name)
    {
        await _repository.Boards.AddAsync(new Board { Name = name });

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Board '{name}' has been created" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> DeleteBoard(int boardId)
    {
        var boardToRemove = await GetBoardById(boardId);

        if (boardToRemove == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        _repository.Boards.Remove(boardToRemove);

        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Board '{boardToRemove.Name}' has been removed" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> AddMemberToBoard(int boardId, int memberId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        var memberToAdd = await _repository.Members.FindAsync(memberId);

        if (memberToAdd == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}'" };

        if (boardToUpdate.Members.Any(x => x.MemberName == memberToAdd.MemberName))
            return new OperationResult { IsSuccesfull = false, Message = $"Member {memberToAdd.MemberName} is already assigned to this board" };

        if (boardToUpdate.Members == null)
            boardToUpdate.Members = new List<DataAccess.Models.Member> { memberToAdd };
        else
            boardToUpdate.Members.Add(memberToAdd);

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Member has beend added" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> RemoveMemberFromBoard(int boardId, int memberId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        var memberToRemove = await _repository.Members.FindAsync(memberId);

        if (memberToRemove == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}'" };

        var removeResult = boardToUpdate.Members.Remove(memberToRemove);

        if (!removeResult)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}' related to this board!" };

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Member has been removed from board" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> AssignMemberToTask(int taskId, int memberId)
    {
        var taskToAssign = await GetToDoById(taskId);

        if (taskToAssign == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no task with id '{taskId}'", IsNotFound = true };

        var memberToAssign = await _repository.Members.Include(m => m.Boards).FirstOrDefaultAsync(x => x.Id == memberId);

        if (memberToAssign == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}'", IsNotFound = true };

        var isMemberAssignedToBoard = memberToAssign.Boards.Any(x => x.Id == taskToAssign.BoardId);

        if (!isMemberAssignedToBoard)
            return new OperationResult { IsSuccesfull = false, Message = $"User '{memberToAssign.MemberName}' is not member of board with task '{taskToAssign.Name}'" };

        taskToAssign.AssignedMember = memberToAssign;

        _repository.ToDos.Update(taskToAssign);
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Member '{memberToAssign.MemberName}' has been assgined to task '{taskToAssign.Name}'" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> UpdateBoardName(int boardId, string newName)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        boardToUpdate.Name = newName;

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Member has been removed from board" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> AddItemToBoard(int boardId, int itemId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        var itemToAdd = await _repository.ToDos.FindAsync(itemId);

        if (itemToAdd == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no item with id '{itemId}'" };

        boardToUpdate.ToDoItems.Add(itemToAdd);

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Item has been added to board" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> CreateItemInBoard(int boardId, string taskName)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        var newTaskId = await AddToDo(taskName);
        var newTaskObject = await _repository.ToDos.FindAsync(newTaskId);

        if (newTaskId == -1)
            return new OperationResult { IsSuccesfull = false, Message = "Failed to create task" };

        var result = await AddItemToBoard(boardToUpdate, newTaskObject);

        return result;
    }

    public async Task<Board> GetBoardById(int boardId) => await _repository.Boards.FindAsync(boardId);

    public async Task<int> AddToDo(string name)
    {
        var addedTask = await _repository.ToDos.AddAsync(new ToDo { Status = StatusType.ToDo, Name = name });
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
            return addedTask.Entity.Id;

        return -1;
    }

    public async Task<OperationResult> ChangeStatus(int id, StatusType status)
    {
        _repository.ToDos.FirstOrDefault(i => i.Id == id).Status = status;
        var affectedEntries = await _repository.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Task status has been changed" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<IList<ToDo>> GetAllTasks() => await _repository.ToDos.ToListAsync();

    public async Task<ToDo> GetToDoById(int id)
    {
        var todo = await _repository.ToDos.FirstOrDefaultAsync(x => x.Id == id);
        return todo ?? null;
    }

    private async Task<OperationResult> AddItemToBoard(Board board, ToDo taskObject)
    {
        board.ToDoItems.Add(taskObject);

        if (await UpdateBoard(board) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Item has been added to board" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    /// <returns>Affected entries of update command</returns>
    private async Task<int> UpdateBoard(Board boardToUpdate)
    {
        _repository.Boards.Update(boardToUpdate);

        return await _repository.SaveChangesAsync();
    }
}
