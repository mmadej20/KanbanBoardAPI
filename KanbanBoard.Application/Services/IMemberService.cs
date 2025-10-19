using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.Services;

public interface IMemberService
{
    Task<Result<Member, Error>> GetMemberById(Guid memberId);

    Task<Result<Member, Error>> GetMemberByEmail(string email);

    Task<UnitResult<Error>> AddMember(string memberName, string email);

    Task<UnitResult<Error>> UpdateMember(Guid memberId, string? memberName = null, string? email = null);

    Task<UnitResult<Error>> DeleteMember(Guid memberId);
}
