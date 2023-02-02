using AutoMapper.Execution;
using DataAccess;
using DataAccess.Models;
using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Services;

public class BoardService : IBoardService
{
    private readonly KanbanContext _kanbanContext;

    public BoardService(KanbanContext kanbanContext)
    {
        _kanbanContext = kanbanContext;
    }

    public async Task<OperationResult> AddMemberToBoard(int boardId, int memberId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        var memberToAdd = await _kanbanContext.Members.FindAsync(memberId);

        if (memberToAdd == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}'" };

        boardToUpdate.Members.Add(memberToAdd);

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Member has beend added" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> CreateBoard(string name)
    {
        await _kanbanContext.Boards.AddAsync(new Board { Name = name });

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Board '{name}' has been created" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> DeleteBoard(int boardId)
    {
        var boardToRemove = await GetBoardById(boardId);

        if (boardToRemove == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        _kanbanContext.Boards.Remove(boardToRemove);

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Board '{boardToRemove.Name}' has been removed" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> RemoveMemberFromBoard(int boardId, int memberId)
    {
        var boardToUpdate = await GetBoardById(boardId);

        if (boardToUpdate == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no board with id '{boardId}'" };

        var memberToRemove = await _kanbanContext.Members.FindAsync(memberId);

        if (memberToRemove == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}'" };

        var removeResult = boardToUpdate.Members.Remove(memberToRemove);

        if (!removeResult)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no member with id '{memberId}' related to this board!" };

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Member has been removed from board" };

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

        var itemToAdd = await _kanbanContext.ToDos.FindAsync(itemId);

        if (itemToAdd == null)
            return new OperationResult { IsSuccesfull = false, Message = $"There is no item with id '{itemId}'" };

        boardToUpdate.ToDoItems.Add(itemToAdd);

        if (await UpdateBoard(boardToUpdate) > 0)
            return new OperationResult { IsSuccesfull = true, Message = "Item has been added to board" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<Board> GetBoardById(int boardId) => await _kanbanContext.Boards.FindAsync(boardId);

    /// <returns>Affected entries of update command</returns>
    private async Task<int> UpdateBoard(Board boardToUpdate)
    {
        _kanbanContext.Boards.Update(boardToUpdate);

        return await _kanbanContext.SaveChangesAsync();
    }

}
