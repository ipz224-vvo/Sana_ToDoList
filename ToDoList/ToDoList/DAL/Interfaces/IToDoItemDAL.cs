using ToDoList.DAL.Models;

namespace ToDoList.DAL.Interfaces
{
	public interface IToDoItemDAL
	{
		public static StorageType _storageType;
		public static StorageType GetStorageType() { return _storageType; }
		public static void ChangeStorageType(string storageType) { }
		public static async Task<ToDoItem> GetToDoItemById(int id) { return null; }
		public static async Task<List<ToDoItem>> GetToDoItemsAsync() { return null; }
		public static async void AddToDoItemAsync(ToDoItem toDoItem) { }
		public static async void EditToDoItemAsync(ToDoItem toDoItem) { }
		public static async void DeleteToDoItemAsync(int id) { }

	}
}
