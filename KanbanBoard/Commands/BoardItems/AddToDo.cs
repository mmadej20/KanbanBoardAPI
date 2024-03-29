﻿using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanBoard.Commands.BoardItems;

public class AddToDo
{
    //Command
    public record Command(string Name) : IRequest<int>;

    //Handler

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IBoardService _kanbanService;

        public Handler(IBoardService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _kanbanService.AddToDo(request.Name);
        }
    }
}