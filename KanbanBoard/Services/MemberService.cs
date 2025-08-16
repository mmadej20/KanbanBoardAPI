using CSharpFunctionalExtensions;
using DataAccess;
using DataAccess.Models;
using KanbanBoard.Domain.Errors;
using KanbanBoard.Models;
using KanbanBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KanbanBoard.Services;

public class MemberService(KanbanContext kanbanContext) : IMemberService
{
    private readonly KanbanContext _kanbanContext = kanbanContext;

    public async Task<UnitResult<Error>> AddMember(string memberName, string email)
    {
        var existing = await _kanbanContext.Members.FirstOrDefaultAsync(x => x.Email == email);

        if (existing is not null)
        {
            return MemberServiceErrors.EmailAlreadyInUse(email);
        }

        var newMember = await _kanbanContext.Members.AddAsync(new Member { MemberName = memberName, Email = email });

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return MemberServiceErrors.GenericError;
    }

    public async Task<UnitResult<Error>> DeleteMember(int memberId)
    {
        var memberToDelete = await _kanbanContext.Members.FindAsync(memberId);

        if (memberToDelete is null)
        {
            return MemberServiceErrors.MemberNotFound(memberId);
        }

        _kanbanContext.Remove(memberToDelete);
        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return MemberServiceErrors.GenericError;
    }

    public async Task<Result<Member, Error>> GetMemberById(int memberId)
    {
        var member = await _kanbanContext.Members.FirstOrDefaultAsync(i => i.Id == memberId);

        if (member is null)
        {
            return MemberServiceErrors.MemberNotFound(memberId);
        }

        return member;
    }

    public async Task<UnitResult<Error>> UpdateMember(int memberId, string memberName = null, string email = null)
    {
        if (string.IsNullOrEmpty(memberName) && string.IsNullOrEmpty(email))
        {
            return MemberServiceErrors.NothingToUpdate;
        }

        var memberToUpdate = await _kanbanContext.Members.FindAsync(memberId);

        if (memberToUpdate is null)
        {
            return MemberServiceErrors.MemberNotFound(memberId);
        }

        if (!string.IsNullOrWhiteSpace(memberName))
        {
            memberToUpdate.MemberName = memberName;
        }

        if (!string.IsNullOrEmpty(email))
        {
            memberToUpdate.Email = email;
        }

        _kanbanContext.Members.Update(memberToUpdate);

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return MemberServiceErrors.GenericError;
    }
}
