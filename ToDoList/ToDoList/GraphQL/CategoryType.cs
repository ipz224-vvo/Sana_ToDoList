using HotChocolate.Resolvers;
using ToDoList.DAL.Implementations;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.GraphQL
{
	public class CategoryType : ObjectType<Category>
	{
		protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
		{
			descriptor.Field(x => x.Id).Type<IdType>();
			descriptor.Field(x => x.Name).Type<StringType>();
			descriptor.Field<ToDoItemResolver>(x => x.GetToDoItems(default, default));
		}
	}
	public class ToDoItemResolver
	{
		private readonly IToDoItemDAL _toDoItemDAL;
		public ToDoItemResolver([Service] IToDoItemDAL toDoItemDAL)
		{
			_toDoItemDAL = toDoItemDAL;
		}
		public IEnumerable<ToDoItem> GetToDoItems(Category category, IResolverContext ctx)
		{
			return ToDoItemDAL.GetToDoItemsAsync().Result.
				Where(x => x.CategoryId == category.Id);
		}
	}
}
