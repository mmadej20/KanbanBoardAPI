using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Queries.BoardItems;

public class GetAllTasks
{
    public record Query() : IRequest<Result<IList<ToDo>, Error>>;

    public class Handler : IRequestHandler<Query, Result<IList<ToDo>, Error>>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<Result<IList<ToDo>, Error>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetAllTasks();
            return result;
        }
    }
}