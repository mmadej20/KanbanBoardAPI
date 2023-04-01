using AutoMapper;
using DataAccess.Models;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Queries.BoardItems;

public class GetToDoById
{
    //Query
    public record Query(int Id) : IRequest<ToDo>;

    //Handler
    public class Handler : IRequestHandler<Query, ToDo>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<ToDo> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetToDoById(request.Id);
            return result;
        }
    }
}