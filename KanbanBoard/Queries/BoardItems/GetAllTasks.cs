using AutoMapper;
using DataAccess.Models;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static KanbanBoard.Queries.BoardItems.GetToDoById;

namespace KanbanBoard.Queries.BoardItems;

public class GetAllTasks
{
    public record Query() : IRequest<IList<ToDo>>;

    public class Handler : IRequestHandler<Query, IList<ToDo>>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<IList<ToDo>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetAllTasks();
            return result;
        }
    }
}