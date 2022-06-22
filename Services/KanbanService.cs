using KanbanBoard.Enums;
using KanbanBoard.Models;
using KanbanBoard.Repositories;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using static KanbanBoard.Queries.GetToDoById;

namespace KanbanBoard.Services
{
    public class KanbanService : IKanbanService
    {
        private readonly Repository _repository;

        public KanbanService(Repository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddToDo(string name)
        {
            int randomId = new System.Random(6847298).Next(10000000);
            _repository.ToDos.Add(new ToDo { Id = randomId, Status = StatusType.ToDo, Name = name });
            return randomId;
        }

        public async Task<bool> ChangeStatus(int id, StatusType status)
        {
            _repository.ToDos.FirstOrDefault(i => i.Id == id).Status = status;
            return true;
        }

        public async Task<ToDo> GetToDoById(int id)
        {
            var todo = _repository.ToDos.FirstOrDefault(x => x.Id == id);
            return todo == null ? null : new ToDo() { Id = todo.Id, Name = todo.Name, Status = todo.Status };
        }
    }
}