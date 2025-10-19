using KanbanBoard.Api.Contracts.Board.Responses;
using KanbanBoard.Api.Contracts.BoardItem.Responses;
using KanbanBoard.Api.Contracts.Member.Responses;
using KanbanBoard.Domain.Entities;
using System.Linq;

namespace KanbanBoard.Api.Mapping
{
    public static class ResponseMappingProfiles
    {
        public static BoardResponse ToBoardResponse(this Board dto)
            => new()
            {
                Id = dto.Id,
                Name = dto.Name,
                ToDoItems = (dto.BoardItems ?? Enumerable.Empty<BoardItem>())
                              .Select(t => t.ToBoardItemResponse())
                              .ToList()
                              .AsReadOnly(),
                Members = (dto.Members ?? Enumerable.Empty<Member>())
                              .Select(m => m.ToMemberResponse())
                              .ToList()
                              .AsReadOnly()
            };

        public static BoardItemResponse ToBoardItemResponse(this BoardItem dto)
            => new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Status = dto.Status,
                AssignedMemberId = dto.AssignedMemberId,
                BoardId = dto.BoardId
            };

        public static MemberResponse ToMemberResponse(this Member dto)
            => new()
            {
                Id = dto.Id,
                MemberName = dto.MemberName,
                Email = dto.Email
            };
    }
}
