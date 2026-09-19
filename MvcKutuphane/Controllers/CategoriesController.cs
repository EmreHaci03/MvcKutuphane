using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class CategoriesController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult Index()
        {
            var Categories = db.TBL_KATEGORI.Include(x => x.TBL_KITAP).ToList();
            return View(Categories);
        }

        public ActionResult BookListWithCategory(int id)
        {
            var Book = db.TBL_KITAP
                .Include(x => x.TBL_KATEGORI)
                .Include(x => x.TBL_YAZAR)
                .Where(x => x.KATEGORI == id)
                .ToList();

            if (!Book.Any())
            {
                TempData["Error"] = "Bu kategoriye ait kitap bulunmamaktadır.";
                return RedirectToAction("Index");
            }

            return View(Book);
        }
    }
}