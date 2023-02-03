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
    public record Query() : IRequest<IList<Response>>;

    public class Handler : IRequestHandler<Query, IList<Response>>
    {
        private readonly IBoardItemService _kanbanService;
        private readonly IMapper _mapper;

        public Handler(IBoardItemService kanbanService, IMapper mapper)
        {
            _kanbanService = kanbanService;
            _mapper = mapper;
        }

        public async Task<IList<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetAllTasks();
            return _mapper.Map<IList<ToDo>, IList<Response>>(result);
        }
    }
}