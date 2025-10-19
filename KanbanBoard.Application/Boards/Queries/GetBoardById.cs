using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.Boards.Queries;

public class GetBoardById
{
    public record Query(Guid Id) : IRequest<Result<Board, Error>>;

    //Handler
    public class Handler(IBoardService boardService) : IRequestHandler<Query, Result<Board, Error>>
    {
        private readonly IBoardService _boardService = boardService;

        public async Task<Result<Board, Error>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _boardService.GetBoardById(request.Id);
            if (result.IsSuccess)
            {
                return result.Value;
            }

            return result.Error;
        }
    }
}
