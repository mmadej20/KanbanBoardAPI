using AutoMapper;
using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Queries.Boards;

public class GetBoardById
{
    public record Query(int Id) : IRequest<Result<Board, Error>>;

    //Handler
    public class Handler : IRequestHandler<Query, Result<Board, Error>>
    {
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;

        public Handler(IBoardService boardService, IMapper mapper)
        {
            _boardService = boardService;
            _mapper = mapper;
        }

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
