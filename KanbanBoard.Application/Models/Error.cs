namespace KanbanBoard.Application.Models
{
    public class Error(string errorCode, string message)
    {
        public string ErrorCode { get; set; } = errorCode;

        public string Message { get; set; } = message;

        public override string ToString()
        {
            return Message;
        }
    }
}
