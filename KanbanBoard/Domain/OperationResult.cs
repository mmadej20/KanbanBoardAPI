namespace KanbanBoard.Domain;

public class OperationResult
{
    public bool IsSuccesfull { get; set; }

    public string Message { get; set; }

    public bool IsNotFound { get; set; }
}
