using AutoMapper;
using AutoMapper.Collection;
using KanbanBoard.Enums;
using KanbanBoard.Models;
using System.Collections.Generic;
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