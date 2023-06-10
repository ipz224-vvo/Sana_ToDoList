using Dapper;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Implementations
{
    public class CategoryDAL 
    {
        public static List<Category> GetCategories()
        {
            using (var connection = DBConnection.CreateConnection())
            {
                return connection.Query<Category>("SELECT * FROM [Categories]").ToList();
            }
        }
        public static Category GetCategoryById(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                return connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@Id", new { Id = id });
            }
        }
        public static void AddCategory(Category category)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<Category>("INSERT INTO [Categories] (Name, Description) " +
                    "VALUES (@Name, @Description)",
                    new
                    {
                        Name = category.Name,
                        Description = category.Description
                    });
            }
        }
        public static void EditCategory(Category category)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<Category>("UPDATE [Categories] " +
                    "SET Name = @Name, Description = @Description " +
                    "Where Id = @Id",
                    new
                    {
                        Name = category.Name,
                        Description = category.Description,
                        Id = category.Id
                    });
            }
        }
        public static void DeleteCategory(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var ent = connection.QueryFirstOrDefault<ToDoItem>("DELETE FROM [Categories] WHERE Id=@Id", new { Id = id });
            }
        }

    }
}
