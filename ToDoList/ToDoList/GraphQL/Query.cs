using ToDoList.DAL.Implementations;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.GraphQL
{
	public class Query
	{
		private readonly IToDoItemDAL _toDoItemDAL;
		private readonly ICategoryDAL _categoryDAL;

		public Query(IToDoItemDAL toDoItemDAL, ICategoryDAL categoryDAL)
		{
			_toDoItemDAL = toDoItemDAL;
			_categoryDAL = categoryDAL;
		}

		[UsePaging(SchemaType = typeof(ToDoItemType))]
		[UseFiltering]
		public IQueryable<ToDoItem> ToDoItems => ToDoItemDAL.GetToDoItemsAsync().Result;

		[UsePaging(SchemaType = typeof(CategoryType))]
		[UseFiltering]
		public IQueryable<Category> Categories => CategoryDAL.GetCategoriesAsync().Result;

	}
}
