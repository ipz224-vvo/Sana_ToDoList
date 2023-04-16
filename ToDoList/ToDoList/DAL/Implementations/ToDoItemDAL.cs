using Dapper;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Implementations
{
    public class ToDoItemDAL : IToDoItem
    {

        public ToDoItem GetToDoItemById(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var ent = connection.QueryFirstOrDefault<ToDoItem>("SELECT * FROM [Tasks] WHERE Id=@Id", new { Id = id });
                ent.Category = connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@cat_id", new { cat_id = ent.CategoryId });
                return ent;
            }
        }
        public List<ToDoItem> GetToDoItems()
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var entyties = connection.Query<ToDoItem>("SELECT * FROM [Tasks]").ToList();
                foreach (var item in entyties)
                {
                    item.Category = connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@cat_id", new { cat_id = item.CategoryId });
                }
                return entyties;
            }
        }
        public void AddToDoItem(ToDoItem toDoItem)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<ToDoItem>("INSERT INTO [Tasks] (Text, Due_date, ) VALUES ");
            }

        }
    }
}
