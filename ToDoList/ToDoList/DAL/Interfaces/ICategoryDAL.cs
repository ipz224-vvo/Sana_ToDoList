using ToDoList.DAL.Models;

namespace ToDoList.DAL.Interfaces
{
	public interface ICategoryDAL
	{
		private static StorageType _storageType;
		public static void ChangeStorageType(string storageType) { }
		public static async Task<IQueryable<Category>> GetCategoriesAsync() { return null; }
		public static async Task<Category> GetCategoryByIdAsync(int id) { return null; }
		public static async void AddCategoryAsync(Category category) { }
		public static async void DeleteCategoryAsync(int id) { }
	}
}
