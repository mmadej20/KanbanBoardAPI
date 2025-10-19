using KanbanBoard.Application.Models;

namespace KanbanBoard.Application.Members.Errors
{
    public static class MemberServiceErrors
    {
        public static Error EmailAlreadyInUse(string email)
        {
            return new Error("kanbanboard.emailinuse", $"Email [{email}] is already in use");
        }

        public static Error MemberNotFound(string memberEmail)
        {
            return new Error("kanbanboard.membernotfound", $"Member with email [{memberEmail}] not found");
        }

        public static Error MemberNotFound(Guid memberId)
        {
            return new Error("kanbanboard.membernotfound", $"There is no member with id [{memberId}]");
        }

        public static Error NothingToUpdate => new("kanbanboard.nothingtoupdate", "There is nothing to update");

        public static Error GenericError => new("kanbanboard.genericerror", "An error occurred while processing your request");
    }
}
