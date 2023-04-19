using Microsoft.AspNetCore.Mvc;
using ToDoList.DAL.Implementations;
using ToDoList.DAL.Models;

namespace ToDoList.Controllers
{
	public class ToDoItemController : Controller
	{

		public ToDoItemController() { }
		// GET: TaskController
		public ActionResult Index()
		{
			ToDoItemDAL toDoItemDAL = new ToDoItemDAL();
			var temp = toDoItemDAL.GetToDoItems();
			return View(temp);
		}

		// GET: TaskController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: TaskController/Create
		public ActionResult CreateItem()
		{

			return View();
		}

		// POST: TaskController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateItem(IFormCollection collection)
		{
			foreach (var coll_item in collection)
			{
				Console.WriteLine(coll_item.ToString());
			}
			var categoryes = (new CategoryDAL()).GetCategories();
			Console.WriteLine(collection["Text"].ToString());
			Console.WriteLine(collection["Is_Finished"].ToString());
			Convert.ToDateTime(null);
			var item = new ToDoItem();
			item.Text = collection["Text"].ToString();

			item.Is_Finished = collection["Is_Finished"].ToString().Split(',').First() == "true" ? true : false;

			try
			{
				item.Due_Date = Convert.ToDateTime(collection["Due_Date"]);
			}
			catch (FormatException ex)
			{
				item.Due_Date = null;
			}


			try
			{
				item.Set_Date = Convert.ToDateTime(collection["Set_Date"]);
			}
			catch (FormatException ex)
			{
				item.Set_Date = null;
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

			ToDoItemDAL toDoItemDAL = new ToDoItemDAL();
			toDoItemDAL.AddToDoItem(item);
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
		public ActionResult EditItem(int id, IFormCollection collection)
		{
			var categoryes = (new CategoryDAL()).GetCategories();
			var item = new ToDoItem();
			item.Id = id;
			item.Text = collection["Text"].ToString();
			item.Is_Finished = collection["Is_Finished"].ToString().Split(',').First() == "true" ? true : false;
			try
			{
				item.Due_Date = Convert.ToDateTime(collection["Due_Date"]);
			}
			catch (FormatException ex)
			{
				item.Due_Date = null;
			}

			try
			{
				item.Set_Date = Convert.ToDateTime(collection["Set_Date"]);
			}
			catch (FormatException ex)
			{
				item.Set_Date = null;
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

			ToDoItemDAL toDoItemDAL = new ToDoItemDAL();
			toDoItemDAL.EditToDoItem(item);
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
			(new ToDoItemDAL()).DeleteToDoItem(id);
			return RedirectToAction(nameof(Index));
		}

		// POST: TaskController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
