using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Services;

public class BoardItemService : IBoardItemService
{
    private readonly KanbanContext _repository;

    public BoardItemService(KanbanContext repository)
    {
        _repository = repository;
    }

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

    public async Task<IList<ToDo>> GetAllTasks() => _repository.ToDos.ToList();

    public async Task<ToDo> GetToDoById(int id)
    {
        var todo = _repository.ToDos.FirstOrDefault(x => x.Id == id);
        return todo == null ? null : todo;
    }
}