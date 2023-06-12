using Microsoft.Data.SqlClient;

namespace ToDoList.DAL.Implementations
{
	public class DBConnection
	{
		public static SqlConnection CreateConnection()
		{
			return new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
		}
		public static string GetXMLToDoItemsPath()
		{
			return "Storage/ToDoItemsRepo.xml";
		}
		public static string GetXMLCategoriesPath()
		{
			return "Storage/CategoriesRepo.xml";
		}
	}
}
