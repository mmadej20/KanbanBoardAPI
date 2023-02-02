using AutoMapper;
using DataAccess.Models;
using static KanbanBoard.Queries.Boards.GetBoardById;

namespace KanbanBoard.Maps
{
    public class BoardToResponse : Profile
    {
        public BoardToResponse() 
        {
            CreateMap<Board, Response>();
        }
    }
}
