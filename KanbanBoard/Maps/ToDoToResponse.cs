using AutoMapper;
using DataAccess.Models;
using static KanbanBoard.Queries.GetToDoById;

namespace KanbanBoard.Maps
{
    public class ToDoToResponse : Profile
    {
        public ToDoToResponse()
        {
            CreateMap<ToDo, Response>();
            //CreateMap<IList<ToDo>, IList<Response>>();
        }
    }
}