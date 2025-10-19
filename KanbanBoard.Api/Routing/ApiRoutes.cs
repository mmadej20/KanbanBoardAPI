namespace KanbanBoard.Api.Routing
{
    public class ApiRoutes
    {
        public const string Base = "api";

        public static class V1
        {
            public static class Boards
            {
                public const string Base = $"{ApiRoutes.Base}/v1/boards";
                public const string GetById = "{id:guid}";
                public const string Create = "create";
                public const string Delete = "{id:guid}";
                public const string AddMember = "addMember";
                public const string RemoveMember = "removeMember";
            }

            public static class BoardItems
            {
                public const string Base = $"{ApiRoutes.Base}/v1/boardItems";
                public const string GetById = "{id:guid}";
                public const string GetAll = "all";
                public const string Create = "createItem";
                public const string AssignMember = "assignMember";
                public const string MarkAsNew = "{id:guid}/new";
                public const string MarkAsCompleted = "{id:guid}/complete";
                public const string MarkAsInProgress = "{id:guid}/inProgress";
                public const string MarkAsCancelled = "{id:guid}/cancel";
                public const string MarkAsOnHold = "{id:guid}/onHold";
            }

            public static class Members
            {
                public const string Base = $"{ApiRoutes.Base}/v1/members";
                public const string GetById = "{id:guid}";
                public const string Add = "add";
                public const string Update = "update";
            }
        }
    }
}
