using KanbanBoard.Enums;
using KanbanBoard.Models;
using System.Threading.Tasks;

namespace KanbanBoard.Services.Interfaces
{
    public interface IKanbanService
    {
        Task<ToDo> GetToDoById(int id);

        Task<int> AddToDo(string name);

        Task<bool> ChangeStatus(int id, StatusType status);
    }
}