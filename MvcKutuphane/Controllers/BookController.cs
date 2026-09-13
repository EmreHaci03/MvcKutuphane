using MvcKutuphane.Models.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace MvcKutuphane.Controllers
{
    public class BookController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult BookList()
        {
            var book = db.TBL_KITAP.Include(x => x.TBL_KATEGORI).Include(x => x.TBL_YAZAR).ToList();
            return View(book);
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            List<SelectListItem> Category = (from x in db.TBL_KATEGORI
                                             select new SelectListItem
                                             {
                                                 Value=x.ID.ToString(),
                                                 Text=x.CategoryName
                                             }).ToList();

            ViewBag.Category = Category;

            List<SelectListItem> Author = (from x in db.TBL_YAZAR
                                             select new SelectListItem
                                             {
                                                 Value = x.ID.ToString(),
                                                 Text = x.AD + " " + x.SOYAD
                                             }).ToList();

            ViewBag.Author = Author;

            return View();
        }

        [HttpPost]
        public ActionResult CreateBook(TBL_KITAP p)
        {
            var Author = db.TBL_YAZAR.Where(x => x.ID == p.YAZAR).Select(x=>x.ID).FirstOrDefault();
            var Category = db.TBL_KATEGORI.Where(x => x.ID == p.KATEGORI).Select(x => x.ID).FirstOrDefault();
            p.KATEGORI = Category;
            p.YAZAR = Author;
            p.DURUM = true;
            db.TBL_KITAP.Add(p);
            db.SaveChanges();
            return RedirectToAction("BookList");
        }
        [HttpPost]
        public ActionResult DeleteBook(int id)
        {
            var Book = db.TBL_KITAP.Find(id);
            if (Book == null)
            {
                TempData["Error"] = "Silmek İstediğiniz Kitap Bulunamadı";
                return RedirectToAction("BookList");
            }
            db.TBL_KITAP.Remove(Book);
            db.SaveChanges();
            return RedirectToAction("BookList");

        }

        [HttpGet]
        public ActionResult UpdateBook(int id)
        {
            var Book = db.TBL_KITAP.Find(id);
            if (Book == null)
            {
                TempData["Error"] = "Silmek İstediğiniz Kitap Bulunamadı";
                return RedirectToAction("BookList");
            }

            List<SelectListItem> Category = (from x in db.TBL_KATEGORI
                                             select new SelectListItem
                                             {
                                                 Value = x.ID.ToString(),
                                                 Text = x.CategoryName
                                             }).ToList();

            ViewBag.Category = Category;

            List<SelectListItem> Author = (from x in db.TBL_YAZAR
                                           select new SelectListItem
                                           {
                                               Value = x.ID.ToString(),
                                               Text = x.AD + " " + x.SOYAD
                                           }).ToList();

            ViewBag.Author = Author;
            return View(Book);

        }
        [HttpPost]
        public ActionResult UpdateBook(TBL_KITAP p)
        {
            var kitap = db.TBL_KITAP.Find(p.ID);
            kitap.AD = p.AD;
            kitap.KATEGORI = p.KATEGORI;
            kitap.YAZAR = p.YAZAR;
            kitap.BASIMYIL = p.BASIMYIL;
            kitap.YAYINEVI = p.YAYINEVI;
            kitap.SAYFA = p.SAYFA;
            kitap.DURUM = p.DURUM;
            db.SaveChanges();
            return RedirectToAction("BookList");
        }
    }
}