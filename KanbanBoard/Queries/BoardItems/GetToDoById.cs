using AutoMapper;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Queries.BoardItems;

public class GetToDoById
{
    //Query
    public record Query(int Id) : IRequest<Response>;

    //Handler
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IBoardItemService _kanbanService;
        private readonly IMapper _mapper;

        public Handler(IBoardItemService kanbanService, IMapper mapper)
        {
            _kanbanService = kanbanService;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _kanbanService.GetToDoById(request.Id);
            return _mapper.Map<Response>(result);
        }
    }

    //Response
    public record Response(int Id, string Name, string Status);
}