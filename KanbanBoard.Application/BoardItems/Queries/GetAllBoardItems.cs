using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;

namespace KanbanBoard.Application.BoardItems.Queries;

public class GetAllBoardItems
{
    public record Query() : IRequest<Result<IList<BoardItem>, Error>>;

    public class Handler : IRequestHandler<Query, Result<IList<BoardItem>, Error>>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<Result<IList<BoardItem>, Error>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetAllTasks();
            return result;
        }
    }
}