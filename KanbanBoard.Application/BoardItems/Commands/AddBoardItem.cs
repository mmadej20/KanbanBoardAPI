using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using MediatR;

namespace KanbanBoard.Application.BoardItems.Commands;

public class AddBoardItem
{
    //Command
    public record Command(string Name) : IRequest<Result<Guid, Error>>;

    //Handler

    public class Handler(IBoardService kanbanService) : IRequestHandler<Command, Result<Guid, Error>>
    {
        private readonly IBoardService _kanbanService = kanbanService;

        public async Task<Result<Guid, Error>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.AddToDo(request.Name);
        }
    }
}