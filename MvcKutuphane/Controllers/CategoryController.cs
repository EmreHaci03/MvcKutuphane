using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class CategoryController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult Index()
        {
            var Kategori = db.TBL_KATEGORI.ToList();
            return View(Kategori);
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(TBL_KATEGORI p)
        {
            db.TBL_KATEGORI.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            var Category = db.TBL_KATEGORI.Find(id);
            db.TBL_KATEGORI.Remove(Category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            var Category = db.TBL_KATEGORI.Find(id);
            return View(Category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(TBL_KATEGORI p)
        {
            var Category = db.TBL_KATEGORI.Find(p.ID);
            Category.CategoryName= p.CategoryName;
            db.SaveChanges();
            return RedirectToAction("Index");
        }




    }
}