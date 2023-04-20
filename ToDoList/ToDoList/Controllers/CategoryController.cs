﻿using Microsoft.AspNetCore.Mvc;
using ToDoList.DAL.Implementations;
using ToDoList.DAL.Models;

namespace ToDoList.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController
        public ActionResult Index()
        {
            var listOfCategoryes = (new CategoryDAL()).GetCategories();
            return View(listOfCategoryes);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            item.Description = collection["Description"].ToString();
            (new CategoryDAL()).AddCategory(item);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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