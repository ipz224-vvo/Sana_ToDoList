using ToDoList.DAL.Models;

namespace ToDoList.DAL.Interfaces
{
    public interface IToDoItem
    {
        ToDoItem GetToDoItemById(int id);
        List<ToDoItem> GetToDoItems();
    }
}
