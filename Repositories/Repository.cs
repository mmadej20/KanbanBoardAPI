using KanbanBoard.Models;
using System.Collections.Generic;

namespace KanbanBoard.Repositories
{
    public class Repository
    {
        public List<ToDo> ToDos { get; } = new List<ToDo>
        {
            new ToDo {Id = 1, Name = "Buy a coffee", Status = Enums.StatusType.Canceled},
            new ToDo {Id = 2, Name = "Learn CQRS", Status = Enums.StatusType.Completed },
            new ToDo {Id = 3, Name = "Learn ASP.NET", Status = Enums.StatusType.InProgress},
            new ToDo {Id = 4, Name = "Try to learn React!", Status = Enums.StatusType.InProgress},
            new ToDo {Id = 5, Name = "Become a .NET Developer!", Status = Enums.StatusType.ToDo}
        };
    }
}