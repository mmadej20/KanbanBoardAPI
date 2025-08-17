using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Commands.BoardItems;

public class AddToDo
{
    //Command
    public record Command(string Name) : IRequest<Result<int, Error>>;

    //Handler

    public class Handler : IRequestHandler<Command, Result<int, Error>>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<Result<int, Error>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.AddToDo(request.Name);
        }
    }
}