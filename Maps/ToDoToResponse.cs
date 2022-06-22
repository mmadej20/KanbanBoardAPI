using AutoMapper;
using KanbanBoard.Enums;
using KanbanBoard.Models;
using static KanbanBoard.Queries.GetToDoById;

namespace KanbanBoard.Maps
{
    public class ToDoToResponse : Profile
    {
        public ToDoToResponse()
        {
            CreateMap<ToDo, Response>();
        }
    }
}