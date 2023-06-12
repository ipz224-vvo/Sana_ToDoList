using Microsoft.AspNetCore.Mvc;
using ToDoList.DAL.Implementations;
using ToDoList.DAL.Models;

namespace ToDoList.Controllers
{
	public class CategoryController : Controller
	{
		// GET: CategoryController
		public async Task<ActionResult> Index()
		{
			var listOfCategoryes = await CategoryDAL.GetCategoriesAsync();
			return View(listOfCategoryes);
		}

		// GET: CategoryController/Create
		public ActionResult CreateCategory()
		{
			return View();
		}

		// POST: CategoryController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateCategory(IFormCollection collection)
		{

			var item = new Category();
			item.Name = collection["Name"].ToString();
			CategoryDAL.AddCategoryAsync(item);
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CategoryController/Delete/5
		public async Task<ActionResult> DeleteCategory(int id)
		{
			var tasks = await ToDoItemDAL.GetToDoItemsAsync();
			foreach (var task in tasks)
			{
				if (task.CategoryId == id)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			CategoryDAL.DeleteCategoryAsync(id);
			return RedirectToAction(nameof(Index));

		}

		// POST: CategoryController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteCategory(int id, IFormCollection collection)
		{
			return RedirectToAction(nameof(Index));
		}
	}
}
