using KanbanBoard.Enums;
using KanbanBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services.Interfaces
{
    public interface IKanbanService
    {
        Task<ToDo> GetToDoById(int id);

        Task<IList<ToDo>> GetAllTasks();

        Task<int> AddToDo(string name);

        Task<bool> ChangeStatus(int id, StatusType status);
    }
}