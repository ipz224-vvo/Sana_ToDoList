using Dapper;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Implementations
{
    public class CategoryDAL : ICategory
    {
        public List<Category> GetCategories()
        {
            using (var connection = DBConnection.CreateConnection())
            {
                return connection.Query<Category>("SELECT * FROM [Categories]").ToList();
            }
        }
        public Category GetCategoryById(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                return connection.QueryFirstOrDefault<Category>("SELECT * FROM [Categories] WHERE Id=@Id", new { Id = id });
            }
        }
        public void AddCategory(Category category)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<ToDoItem>("INSERT INTO [Categories] (Name, Description) " +
                    "VALUES (@Name, @Description)",
                    new
                    {
                        Name = category.Name,
                        Description = category.Description
                    });
            }
        }
        public void EditCategory(Category category)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                connection.Query<ToDoItem>("UPDATE [Categories] " +
                    "SET Name = @Name, Description = @Description" +
                    "Where Id = @Id",
                    new
                    {
                        Name = category.Name,
                        Description = category.Description,
                        Id = category.Id
                    });
            }
        }
        public void DeleteCategory(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                var ent = connection.QueryFirstOrDefault<ToDoItem>("DELETE FROM [Categories] WHERE Id=@Id", new { Id = id });
            }
        }

    }
}
