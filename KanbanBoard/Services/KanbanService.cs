using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Services
{
    public class KanbanService : IKanbanService
    {
        private readonly KanbanContext _repository;

        public KanbanService(KanbanContext repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddToDo(string name)
        {
            var newItem = _repository.ToDos.Add(new ToDo {Status = StatusType.ToDo, Name = name });
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatus(int id, StatusType status)
        {
            _repository.ToDos.FirstOrDefault(i => i.Id == id).Status = status;
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<IList<ToDo>> GetAllTasks() => _repository.ToDos.ToList();

        public async Task<ToDo> GetToDoById(int id)
        {
            var todo = _repository.ToDos.FirstOrDefault(x => x.Id == id);
            return todo == null ? null : todo;
        }
    }
}