using Dapper;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Implementations
{
    public class ToDoItemDAL 
    {
        public string TypeOfStorage;
        public ToDoItemDAL() { }
        public ToDoItemDAL(string typeOfStorage)
        {
            this.TypeOfStorage = typeOfStorage;
        }

        public static string DateTimeToString(DateTime? dateTime)
        {
            DateTime temp_date;
            if (dateTime != null)
            {
                temp_date = Convert.ToDateTime(dateTime);

                return temp_date.ToString("yyyy-MM-dd HH:mm");
            }
            return null;
        }
        public static ToDoItem GetToDoItemById(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var ent = connection.QueryFirstOrDefault<ToDoItem>("SELECT * FROM [Tasks] WHERE Id=@Id", new { Id = id });
                ent.Category = connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@cat_id", new { cat_id = ent.CategoryId });
                return ent;
            }
        }
        public static List<ToDoItem> GetToDoItems()
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var entyties = connection.Query<ToDoItem>("SELECT * FROM [Tasks]").ToList();
                foreach (var item in entyties)
                {
                    item.Category = connection.QueryFirstOrDefault<Category>("SELECT * FROM [dbo].[Categories] WHERE Id=@cat_id", new { cat_id = item.CategoryId });

                }

                return entyties;
            }
        }
        public static void AddToDoItem(ToDoItem toDoItem)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<ToDoItem>("INSERT INTO [Tasks] (Text, Due_date, CategoryId, Set_Date, Is_Finished) " +
                    "VALUES (@Text, @Due_Date, @CategoryId,@Set_Date,@Is_Finished)",
                    new
                    {
                        Text = toDoItem.Text,
                        Due_Date = toDoItem.Due_Date,
                        CategoryId = toDoItem.CategoryId,
                        Set_Date = toDoItem.Set_Date,
                        Is_Finished = toDoItem.Is_Finished
                    });
            }
        }
        public static void EditToDoItem(ToDoItem toDoItem)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<ToDoItem>("UPDATE [Tasks] " +
                    "SET Text = @Text, Due_Date = @Due_Date, CategoryId = @CategoryId, Set_Date = @Set_Date, Is_Finished = @Is_Finished " +
                    "Where Id = @Id",
                    new
                    {
                        Text = toDoItem.Text,
                        Due_Date = toDoItem.Due_Date,
                        CategoryId = toDoItem.CategoryId,
                        Set_Date = toDoItem.Set_Date,
                        Is_Finished = toDoItem.Is_Finished,
                        Id = toDoItem.Id
                    });
            }
        }
        public static void DeleteToDoItem(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var ent = connection.QueryFirstOrDefault<ToDoItem>("DELETE FROM [Tasks] WHERE Id=@Id", new { Id = id });
            }
        }
    }
}
