using KanbanBoard.Application.Models;

namespace KanbanBoard.Application.Boards.Errors
{
    public static class BoardServiceErrors
    {
        public static Error GenericError => new Error("kanbanboard.genericerror", "An error occurred while processing your request");

        public static Error BoardCreateFailure => new Error("kanbanboard.failedtocreateboard", $"Failed to create board");

        public static Error InvalidBoardName => new("kanbanboard.invalidboardname", "Board name cannot be empty or whitespace");

        public static Error BoardNotFound(int boardId)
        {
            return new Error("kanbanboard.boardnotfound", $"There is no board with id [{boardId}]");
        }

        public static Error BoardAlreadyExists(string name)
        {
            return new Error("kanbanboard.boardalreadyexists", $"Board with name [{name}] already exists");
        }

        public static Error MemberNotFound(int memberId)
        {
            return new Error("kanbanboard.membernotfound", $"There is no member with id [{memberId}]");
        }

        public static Error MemberAlreadyExists(int memberId)
        {
            return new Error("kanbanboard.memberalreadyexists", $"Member with id [{memberId}] already exists in this board");
        }

        public static Error MemberIsNotAssignedToBoard(int memberId)
        {
            return new Error("kanbanboard.membernotassigned", $"Member with id [{memberId}] is not yet assigned to this board");
        }

        public static Error ItemNotFound(int itemId)
        {
            return new Error("kanbanboard.itemnotfound", $"There is no item with id [{itemId}]");
        }

        public static Error NoTasksFound => new Error("kanbanboard.notasksfound", $"There are no tasks available");
    }
}
