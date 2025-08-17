using AutoMapper;
using CSharpFunctionalExtensions;
using KanbanBoard.Application.Members.Errors;
using KanbanBoard.Application.Models;
using KanbanBoard.Application.Services;
using KanbanBoard.DataAccess;
using KanbanBoard.DataAccess.Entities;
using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Services;

public class MemberService(KanbanContext kanbanContext, IMapper mapper) : IMemberService
{
    private readonly KanbanContext _kanbanContext = kanbanContext;
    private readonly IMapper _mapper = mapper;

    public async Task<UnitResult<Error>> AddMember(string memberName, string email)
    {
        var existing = await _kanbanContext.Members
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        if (existing is not null)
        {
            return MemberServiceErrors.EmailAlreadyInUse(email);
        }

        var newMember = await _kanbanContext.Members.AddAsync(new MemberEntity { MemberName = memberName, Email = email });

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
        var memberEntity = await _kanbanContext.Members.AsNoTracking().FirstOrDefaultAsync(i => i.Id == memberId);

        if (memberEntity is null)
        {
            return MemberServiceErrors.MemberNotFound(memberId);
        }

        var member = _mapper.Map<Member>(memberEntity);

        return member;
    }

    public async Task<UnitResult<Error>> UpdateMember(int memberId, string? memberName = null, string? email = null)
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
            var emailExists = await _kanbanContext.Members
                .AnyAsync(m => m.Email.ToLower() == email.ToLower() && m.Id != memberId);
            if (emailExists)
            {
                return MemberServiceErrors.EmailAlreadyInUse(email);
            }

            memberToUpdate.Email = email;
        }

        _kanbanContext.Members.Update(memberToUpdate); //TODO: Check if needed, as it might not be necessary if the entity is already being tracked by the context.

        var affectedEntries = await _kanbanContext.SaveChangesAsync();

        if (affectedEntries > 0)
        {
            return UnitResult.Success<Error>();
        }

        return MemberServiceErrors.GenericError;
    }
}
