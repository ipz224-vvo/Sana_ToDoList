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
                return connection.Query<Category>("SELECT * FROM [Category]").ToList();
            }
        }
        public Category GetCategoryById(int id)
        {
            using (var connection = DBConnection.CreateConnection())
            {
                return connection.QueryFirstOrDefault<Category>("SELECT * FROM [Category] WHERE Id=@Id", new { Id = id });
            }
        }
    }
}
