using CSharpFunctionalExtensions;
using KanbanBoard.Application.Models;
using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.Services;

public interface IMemberService
{
    Task<Result<Member, Error>> GetMemberById(int memberId);

    Task<UnitResult<Error>> AddMember(string memberName, string email);

    Task<UnitResult<Error>> UpdateMember(int memberId, string? memberName = null, string? email = null);

    Task<UnitResult<Error>> DeleteMember(int memberId);
}
