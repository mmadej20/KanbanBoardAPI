using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Application.BoardItems.Queries;

public class GetToDoById
{
    //Query
    public record Query(int Id) : IRequest<Result<ToDo, Error>>;

    //Handler
    public class Handler : IRequestHandler<Query, Result<ToDo, Error>>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<Result<ToDo, Error>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetToDoById(request.Id);
            if (result.IsSuccess)
            {
                return result.Value;
            }

            return result.Error;
        }
    }
}