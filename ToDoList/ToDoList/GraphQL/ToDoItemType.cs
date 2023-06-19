using HotChocolate.Resolvers;
using ToDoList.DAL.Implementations;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.GraphQL
{
	public class ToDoItemType : ObjectType<ToDoItem>
	{
		protected override void Configure(IObjectTypeDescriptor<ToDoItem> descriptor)
		{
			descriptor.Field(x => x.Id).Type<IdType>();
			descriptor.Field(x => x.Text).Type<StringType>();
			descriptor.Field(x => x.CategoryId).Type<IntType>();
			descriptor.Field(x => x.EndDate).Type<DateType>();
			descriptor.Field(x => x.IsFinished).Type<BooleanType>();
			descriptor.Field<CategoryResolver>(x => x.GetCategory(default, default));

		}
	}
	public class CategoryResolver
	{
		private readonly ICategoryDAL _categoryDAL;
		public CategoryResolver([Service] ICategoryDAL categoryDAL)
		{
			_categoryDAL = categoryDAL;
		}
		public Category GetCategory(ToDoItem toDoItem, IResolverContext ctx)
		{
			return CategoryDAL.GetCategoriesAsync().Result
				.Where(x => x.Id == toDoItem.CategoryId).FirstOrDefault();
		}
	}
}
