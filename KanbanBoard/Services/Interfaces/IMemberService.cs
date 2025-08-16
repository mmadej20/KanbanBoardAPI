using CSharpFunctionalExtensions;
using DataAccess.Models;
using KanbanBoard.Models;
using System.Threading.Tasks;

namespace KanbanBoard.Services.Interfaces;

public interface IMemberService
{
    Task<Result<Member, Error>> GetMemberById(int memberId);

    Task<UnitResult<Error>> AddMember(string memberName, string email);

    Task<UnitResult<Error>> UpdateMember(int memberId, string memberName, string email);

    Task<UnitResult<Error>> DeleteMember(int memberId);
}
