using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Enums;
using MediatR;

namespace KanbanBoard.Application.BoardItems.Commands;

public class MarkAsCompleted
{
    public record Command(Guid Id) : IRequest<Result<int, Error>>;

    public class Handler(IBoardService kanbanService) : IRequestHandler<Command, Result<int, Error>>
    {
        private readonly IBoardService _kanbanService = kanbanService;

        public async Task<Result<int, Error>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.ChangeStatus(request.Id, StatusType.Completed);
        }
    }
}