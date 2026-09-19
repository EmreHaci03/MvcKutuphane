using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class CatalogController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        public ActionResult BookList()
        {
            var BookList = db.TBL_KITAP
                .Include(x => x.TBL_YAZAR)
                .Include(x => x.TBL_KATEGORI)
                .ToList();
            return View(BookList);
        }

        public ActionResult BookDetail(int id)
        {
            var book = db.TBL_KITAP
                .Include(x => x.TBL_YAZAR)
                .Include(x => x.TBL_KATEGORI)
                .FirstOrDefault(x => x.ID == id);

            if (book == null)
            {
                TempData["Error"] = "Kitap bulunamadı.";
                return RedirectToAction("BookList");
            }

            var kitapDetay = db.TBL_KITAPDETAY.FirstOrDefault(x => x.KITAP == id);
            ViewBag.KitapDetay = kitapDetay != null ? kitapDetay.DETAY : null;

            return View(book);
        }

        public ActionResult BookListWithCategory(int id)
        {
            var kategori = db.TBL_KATEGORI.Find(id);
            if (kategori == null)
            {
                TempData["Error"] = "Kategori bulunamadı.";
                return RedirectToAction("BookList");
            }

            var Book = db.TBL_KITAP
                .Include(x => x.TBL_KATEGORI)
                .Include(x => x.TBL_YAZAR)
                .Where(x => x.KATEGORI == id)
                .ToList();

            ViewBag.CategoryName = kategori.CategoryName;

            if (!Book.Any())
            {
                TempData["Message"] = "Bu kategoriye ait henüz kitap bulunmuyor.";
            }

            return View(Book);
        }
    }
}