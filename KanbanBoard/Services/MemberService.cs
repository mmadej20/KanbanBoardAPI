using DataAccess;
using DataAccess.Models;
using KanbanBoard.Domain;
using KanbanBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KanbanBoard.Services;

public class MemberService : IMemberService
{
    private readonly KanbanContext _kanbanContext;

    public MemberService(KanbanContext kanbanContext)
    {
        _kanbanContext = kanbanContext;
    }
    public async Task<OperationResult> AddMember(string memberName, string email)
    {
        var existing = await _kanbanContext.Members.FirstOrDefaultAsync(x => x.Email == email);

        if (existing is not null)
            return new OperationResult { IsSuccesfull = false, Message = $"Email '{email}' is already in use!" };

        var newMember = await _kanbanContext.Members.AddAsync(new Member { MemberName = memberName, Email = email });

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"New member has been created with ID:{newMember.Entity.Id}" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<OperationResult> DeleteMember(int memberId)
    {
        var memberToDelete = await _kanbanContext.Members.FindAsync(memberId);

        if (memberToDelete is null)
            return new OperationResult { IsSuccesfull = false, Message = $"Member with ID '{memberId}' does not exists", IsNotFound = true };

        _kanbanContext.Remove(memberToDelete);
        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Member '{memberToDelete.MemberName}' has been removed!" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };
    }

    public async Task<Member> GetMemberById(int memberId) => await _kanbanContext.Members.FirstOrDefaultAsync(i => i.Id == memberId);

    public async Task<OperationResult> UpdateMember(int memberId, string memberName = null, string email = null)
    {
        if (string.IsNullOrEmpty(memberName) && string.IsNullOrEmpty(email))
            return new OperationResult { IsSuccesfull = false, Message = $"There is nothing to update" };

        var memberToUpdate = await _kanbanContext.Members.FindAsync(memberId);

        if (memberToUpdate is null)
            return new OperationResult { IsSuccesfull = false, Message = $"Member with ID '{memberId}' does not exists", IsNotFound = true };

        if (!string.IsNullOrWhiteSpace(memberName))
            memberToUpdate.MemberName = memberName;

        if (!string.IsNullOrEmpty(email))
            memberToUpdate.Email = email;

        _kanbanContext.Members.Update(memberToUpdate);

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
            return new OperationResult { IsSuccesfull = true, Message = $"Member data has been updated!" };

        return new OperationResult { IsSuccesfull = false, Message = "There is a problem with your request" };


    }
}
