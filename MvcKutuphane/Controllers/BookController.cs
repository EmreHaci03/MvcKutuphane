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
    [Authorize(Roles = "Admin")]
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
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.YAYINEVI))
            {
                TempData["Error"] = "Kitap Adı Ve Yayın Evi Boş Bırakılamaz.";
                return RedirectToAction("CreateBook");
            }

            if (string.IsNullOrWhiteSpace(p.SAYFA))
            {
                TempData["Error"] = "Sayfa sayısı boş bırakılamaz.";
                return RedirectToAction("CreateBook");
            }
            if (string.IsNullOrWhiteSpace(p.FOTOGRAF))
            {
                TempData["Error"] = "Kitap Fotoğrafı Boş Bırakılamaz.";
                return RedirectToAction("CreateBook");
            }

            int numberofPages = 0;
            if (!int.TryParse(p.SAYFA, out numberofPages) || numberofPages <= 0)
            {
                TempData["Error"] = "Sayfa sayısı geçerli bir sayı olmalı.";
                return RedirectToAction("CreateBook");
            }

            int YearOfPublication = 0;
            if (!int.TryParse(p.BASIMYIL, out YearOfPublication) || YearOfPublication < 1400 || YearOfPublication > DateTime.Now.Year)
            {
                TempData["Error"] = "Lütfen geçerli bir basım yılı giriniz.";
                return RedirectToAction("CreateBook");
            }

            bool yazarVarMi = db.TBL_YAZAR.Any(x => x.ID == p.YAZAR);
            bool kategoriVarMi = db.TBL_KATEGORI.Any(x => x.ID == p.KATEGORI);
            if (!yazarVarMi || !kategoriVarMi)
            {
                TempData["Error"] = "Lütfen geçerli bir yazar ve kategori seçiniz.";
                return RedirectToAction("CreateBook");
            }

            bool ayniKitapVarMi = db.TBL_KITAP.Any(x => x.AD.Trim().ToLower() == p.AD.Trim().ToLower());
            if (ayniKitapVarMi)
            {
                TempData["Error"] = "Bu isimde bir kitap zaten kayıtlı.";
                return RedirectToAction("CreateBook");
            }
            try
            {

                p.AD = p.AD.Trim();
                p.DURUM = true;

                db.TBL_KITAP.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Kitap eklendi.";
                return RedirectToAction("BookList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitap eklenirken bir hata oluştu.";
                return RedirectToAction("CreateBook");
            }
        }

        [HttpPost]
        public ActionResult DeleteBook(int id)
        {
            var Book = db.TBL_KITAP.Find(id);
            if (Book == null)
            {
                TempData["Error"] = "Silmek istediğiniz kitap bulunamadı.";
                return RedirectToAction("BookList");
            }

            try
            {
                db.TBL_KITAP.Remove(Book);
                db.SaveChanges();
                TempData["Message"] = "Kitap silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Bu kitaba ait ödünç kayıtları olduğu için silinemiyor.";
            }

            return RedirectToAction("BookList");
        }

        [HttpGet]
        public ActionResult UpdateBook(int id)
        {
            var Book = db.TBL_KITAP.Find(id);
            if (Book == null)
            {
                TempData["Error"] = "Güncellemek İstediğiniz Kitap Bulunamadı";
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
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.YAYINEVI))
            {
                TempData["Error"] = "Kitap adı ve yayınevi boş bırakılamaz.";
                return RedirectToAction("UpdateBook", new { id = p.ID });
            }

            if (string.IsNullOrWhiteSpace(p.FOTOGRAF))
            {
                TempData["Error"] = "Kitap Fotoğrafı Boş Bırakılamaz.";
                return RedirectToAction("UpdateBook", new { id = p.ID });
            }


            int PageCount = 0;
            if (!int.TryParse(p.SAYFA, out PageCount) || PageCount <= 0)
            {
                TempData["Error"] = "Sayfa sayısı geçerli bir sayı olmalı.";
                return RedirectToAction("UpdateBook", new { id = p.ID });
            }

            int yearOfPublicaiton = 0;
            if (!int.TryParse(p.BASIMYIL, out yearOfPublicaiton) || yearOfPublicaiton <= 0)
            {
                TempData["Error"] = "Basım yılı geçerli bir değer olmalı.";
                return RedirectToAction("UpdateBook", new { id = p.ID });
            }
            var kitap = db.TBL_KITAP.Find(p.ID);
            if (kitap == null)
            {
                TempData["Error"] = "Güncellenecek kitap bulunamadı.";
                return RedirectToAction("BookList");
            }

            try
            {
                kitap.AD = p.AD.Trim();
                kitap.KATEGORI = p.KATEGORI;
                kitap.YAZAR = p.YAZAR;
                kitap.BASIMYIL = p.BASIMYIL;
                kitap.YAYINEVI = p.YAYINEVI.Trim();
                kitap.SAYFA = p.SAYFA;
                kitap.FOTOGRAF = p.FOTOGRAF;
                kitap.DURUM = p.DURUM;
                db.SaveChanges();
                TempData["Message"] = "Kitap bilgileri güncellendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitap güncellenirken bir hata oluştu.";
                return RedirectToAction("UpdateBook", new { id = p.ID });
            }

            return RedirectToAction("BookList");
        }
    }
}