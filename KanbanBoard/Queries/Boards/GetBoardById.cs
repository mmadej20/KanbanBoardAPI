using AutoMapper;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using DataAccess.Models;

namespace KanbanBoard.Queries.Boards;

public class GetBoardById
{
    public record Query(int Id) : IRequest<Response>;

    //Handler
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;

        public Handler(IBoardService boardService, IMapper mapper)
        {
            _boardService = boardService;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _boardService.GetBoardById(request.Id);
            return _mapper.Map<Response>(result);
        }
    }

    //Response
    public record Response(int Id, string Name, IEnumerable<ToDo> ToDoItems, IEnumerable<Member> Members);
}
