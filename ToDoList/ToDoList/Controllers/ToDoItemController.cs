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
            
            var temp = ToDoItemDAL.GetToDoItems();

            var sorted = from item in temp
                         orderby item.Due_Date ascending
                         select item;
            var sorted_list = sorted.ToList();
            for (int i = 0; i < sorted_list.Count(); i++)
            {
                var item = sorted_list[i];
                if (item.Due_Date == null)
                {
                    sorted_list.RemoveAt(i);
                    sorted_list.Add(item);
                }
            }
            temp = (from item in sorted_list
                    orderby item.Is_Finished
                    select item).ToList<ToDoItem>();
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: TaskController/Details/5
        public ActionResult ChangeStatusToDoItem(int id)
        {
            ToDoItem item = ToDoItemDAL.GetToDoItemById(id);
            if (item.Is_Finished)
                item.Is_Finished = false;
            else
                item.Is_Finished = true;
            ToDoItemDAL.EditToDoItem(item);
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
        public ActionResult CreateItem(IFormCollection collection)
        {

            var categoryes = CategoryDAL.GetCategories();
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
                    item.CategoryId = categoryes[i].Id;
                    break;
                }
            }

            ToDoItemDAL.AddToDoItem(item);
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
            var categoryes = CategoryDAL.GetCategories();
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
            ToDoItemDAL.EditToDoItem(item);
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

            ToDoItemDAL.DeleteToDoItem(id);

            return RedirectToAction(nameof(Index));
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id, IFormCollection collection)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
