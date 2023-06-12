using Microsoft.AspNetCore.Mvc;
using ToDoList.DAL.Implementations;
using ToDoList.DAL.Models;

namespace ToDoList.Controllers
{
	public class ToDoItemController : Controller
	{

		public ToDoItemController() { }
		// GET: TaskController
		public async Task<ActionResult> Index()
		{

			var temp = await ToDoItemDAL.GetToDoItemsAsync();

			if (temp == null) return View(new List<ToDoItem>()); ;
			var sorted = from item in temp
						 orderby item.EndDate ascending
						 select item;
			var sorted_list = sorted.ToList();
			for (int i = 0; i < sorted_list.Count(); i++)
			{
				var item = sorted_list[i];
				if (item.EndDate == null)
				{
					sorted_list.RemoveAt(i);
					sorted_list.Add(item);
				}
			}
			temp = (from item in sorted_list
					orderby item.IsFinished
					select item).ToList<ToDoItem>();
			return View(temp);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		// POST: TaskController/Details/5
		public async Task<ActionResult> ChangeStatusToDoItem(int id)
		{
			ToDoItem item = await ToDoItemDAL.GetToDoItemByIdAsync(id);
			if (item.IsFinished)
				item.IsFinished = false;
			else
				item.IsFinished = true;
			ToDoItemDAL.EditToDoItemAsync(item);
			return RedirectToAction(nameof(Index));
		}

		// GET: TaskController/Create
		public ActionResult CreateItem()
		{
			return View();
		}

		// POST: TaskController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateItem(IFormCollection collection)
		{

			var categoryes = await CategoryDAL.GetCategoriesAsync();
			var item = new ToDoItem();
			item.Text = collection["Text"].ToString();

			item.IsFinished = collection["IsFinished"].ToString().Split(',').First() == "true" ? true : false;

			try
			{
				item.EndDate = Convert.ToDateTime(collection["EndDate"]);
			}
			catch (FormatException ex)
			{
				item.EndDate = null;
			}

			for (int i = 0; i < categoryes.Count; i++)
			{
				if (collection["Category"].ToString() == categoryes[i].Name.ToString())
				{
					item.Category = categoryes[i];
					item.CategoryId = categoryes[i].Id;
					break;
				}
			}

			ToDoItemDAL.AddToDoItemAsync(item);
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: TaskController/Edit/5
		public ActionResult EditItem(int id)
		{
			return View();
		}

		// POST: TaskController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditItem(int id, IFormCollection collection)
		{
			var categoryes = await CategoryDAL.GetCategoriesAsync();
			var item = new ToDoItem();
			item.Id = id;
			item.Text = collection["Text"].ToString();
			item.IsFinished = collection["IsFinished"].ToString().Split(',').First() == "true" ? true : false;
			try
			{
				item.EndDate = Convert.ToDateTime(collection["EndDate"]);
			}
			catch (FormatException ex)
			{
				item.EndDate = null;
			}



			for (int i = 0; i < categoryes.Count; i++)
			{
				if (collection["Category"].ToString() == categoryes[i].Name.ToString())
				{
					item.Category = categoryes[i];
					item.CategoryId = i + 1;
					break;
				}
			}
			ToDoItemDAL.EditToDoItemAsync(item);
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: TaskController/Delete/5
		public ActionResult DeleteItem(int id)
		{
			try
			{
				ToDoItemDAL.DeleteToDoItemAsync(id);
			}
			catch (Exception ex)
			{

			}

			return RedirectToAction(nameof(Index));
		}

		// POST: TaskController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(int id, IFormCollection collection)
		{
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public ActionResult ChangeStorageType(string selectType)
		{
			if (selectType == null) return null;
			else if (selectType == "SQL" || selectType == "XML")
			{
				ToDoItemDAL.ChangeStorageType(selectType);
				CategoryDAL.ChangeStorageType(selectType);
			}
			/*if (selectType == "SQL")
			{
				ToDoItemDAL.StorageType = StorageType.SQL;
				CategoryDAL.StorageType = StorageType.SQL;
			}
			if (selectType == "XML")
			{
				ToDoItemDAL.StorageType = StorageType.XML;
				CategoryDAL.StorageType = StorageType.XML;
			}*/
			return RedirectToAction(nameof(Index));
		}
	}
}
