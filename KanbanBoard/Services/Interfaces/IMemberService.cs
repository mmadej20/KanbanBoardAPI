using DataAccess.Models;
using KanbanBoard.Domain;
using System.Threading.Tasks;

namespace KanbanBoard.Services.Interfaces;

public interface IMemberService
{
    Task<Member> GetMemberById(int memberId);

    Task<OperationResult> AddMember(string memberName, string email);

    Task<OperationResult> UpdateMember(int memberId,string memberName, string email);

    Task<OperationResult> DeleteMember(int memberId);


}
