using DataAccess.Enums;
using DataAccess.Models;
using KanbanBoard.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KanbanBoard.Services.Interfaces;

public interface IBoardItemService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID of item in database</param>
    /// <returns>Kanban item of corressponding ID</returns>
    Task<ToDo> GetToDoById(int id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>List of Kanban items</returns>
    Task<IList<ToDo>> GetAllTasks();

    /// <summary>
    /// Adds new item to Kanban Board
    /// </summary>
    /// <param name="name">Description of item</param>
    /// <returns>ID of added task</returns>
    Task<int> AddToDo(string name);

    /// <summary>
    /// Change status of item 
    /// </summary>
    /// <param name="id">Item ID</param>
    /// <param name="status">Type of status</param>
    /// <returns>Number of affected entries</returns>
    Task<OperationResult> ChangeStatus(int id, StatusType status);
}