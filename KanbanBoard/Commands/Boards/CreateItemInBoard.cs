using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace KanbanBoard.Commands.Boards
{
    public class CreateItemInBoard
    {
        public record Command(int BoardId, string Name) : IRequest<OperationResult>;

        //Handler

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IBoardService _boardService;
            private readonly IBoardItemService _boardItemService;

            public Handler(IBoardService boardService, IBoardItemService boardItemService)
            {
                _boardService = boardService;
                _boardItemService = boardItemService;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var newItemId = await _boardItemService.AddToDo(request.Name);
                if (newItemId == -1)
                    return new OperationResult { IsSuccesfull = false, Message = "Failed to create task"};

                return await _boardService.AddItemToBoard(request.BoardId, newItemId);
            }
        }
    }
}
