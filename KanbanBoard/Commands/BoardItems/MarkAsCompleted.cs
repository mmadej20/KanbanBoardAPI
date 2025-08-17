using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Api.Commands.BoardItems;

public class MarkAsCompleted
{
    public record Command(int Id) : IRequest<Result<int, Error>>;

    public class Handler : IRequestHandler<Command, Result<int, Error>>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<Result<int, Error>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.ChangeStatus(request.Id, StatusType.Completed);
        }
    }
}