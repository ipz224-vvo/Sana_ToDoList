using ToDoList.DAL.Implementations;
using ToDoList.DAL.Interfaces;
using ToDoList.DAL.Models;

namespace ToDoList.GraphQL
{
	public class Mutation
	{
		private readonly IToDoItemDAL _toDoItemDAL = null;
		public string definedField;

		public Mutation(IToDoItemDAL toDoItemDAL)
		{
			definedField = "111";
			_toDoItemDAL = toDoItemDAL;
		}
		public string AddToDoItem(ToDoItem toDoItem)
		{
			ToDoItemDAL.AddToDoItemAsync(toDoItem);
			return "Таск додано!";
		}
		public string EditToDoItem(ToDoItem toDoItem)
		{
			ToDoItemDAL.EditToDoItemAsync(toDoItem);
			return "Таск змінено!";

		}
		public string DeleteToDoItem(ToDoItem toDoItem)
		{
			ToDoItemDAL.DeleteToDoItemAsync(toDoItem.Id);
			return "Таск видалено!";

		}
		public string ChangeStorageType(string storageType)
		{
			ToDoItemDAL.ChangeStorageType(storageType);
			return "Тип сховища змінено!";

		}

	}
}
