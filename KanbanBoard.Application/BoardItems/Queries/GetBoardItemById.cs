using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.BoardItems.Queries;

public class GetBoardItemById
{
    //Query
    public record Query(Guid Id) : IRequest<Result<BoardItem, Error>>;

    //Handler
    public class Handler(IBoardService kanbanService) : IRequestHandler<Query, Result<BoardItem, Error>>
    {
        private readonly IBoardService _kanbanService = kanbanService;

        public async Task<Result<BoardItem, Error>> Handle(Query request, CancellationToken cancellationToken)
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