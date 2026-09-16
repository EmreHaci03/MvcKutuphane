using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class AuthorController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult AuthorList()
        {
            var Author = db.TBL_YAZAR.ToList();
            return View(Author);
        }

        [HttpGet]
        public ActionResult CreateAuthor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAuthor(TBL_YAZAR p)
        {
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.SOYAD))
            {
                TempData["Error"] = "Ad ve soyad alanları boş bırakılamaz.";
                return RedirectToAction("CreateAuthor");
            }

            bool ExistAuthor = db.TBL_YAZAR.Any(x => x.AD.Trim().ToLower() == p.AD.Trim().ToLower()
                                          && x.SOYAD.Trim().ToLower() == p.SOYAD.Trim().ToLower());

            if (ExistAuthor)
            {
                TempData["Error"] = "Bu isimde bir yazar zaten kayıtlı.";
                return RedirectToAction("CreateAuthor");
            }

            try
            {
                p.AD = p.AD.Trim();
                p.SOYAD = p.SOYAD.Trim();
                db.TBL_YAZAR.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Yazar eklendi.";
                return RedirectToAction("AuthorList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Yazar eklenirken bir hata oluştu.";
                return RedirectToAction("CreateAuthor");
            }
        }

        [HttpPost]
        public ActionResult DeleteAuthor(int id)
        {
            var Author = db.TBL_YAZAR.Find(id);
            if (Author == null)
            {
                TempData["Error"] = "Silmek istediğiniz yazar bilgisi alınamadı.";
                return RedirectToAction("AuthorList");
            }
            try
            {
                db.TBL_YAZAR.Remove(Author);
                db.SaveChanges();
                TempData["Message"] = "Yazar silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Bu yazara ait kitaplar olduğu için silinemiyor.";
            }
            return RedirectToAction("AuthorList");
        }

        [HttpGet]
        public ActionResult UpdateAuthor(int id)
        {
            var Author = db.TBL_YAZAR.Find(id);

            if (Author == null)
            {
                TempData["Error"] = "Güncellemek istediğiniz yazar bilgisi alınamadı.";
                return RedirectToAction("AuthorList");
            }
            return View(Author);
        }

        [HttpPost]
        public ActionResult UpdateAuthor(TBL_YAZAR p)
        {
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.SOYAD))
            {
                TempData["Error"] = "Ad ve soyad alanları boş bırakılamaz.";
                return RedirectToAction("UpdateAuthor", new { id = p.ID });
            }

            var Author = db.TBL_YAZAR.Find(p.ID);
            if (Author == null)
            {
                TempData["Error"] = "Güncellenecek yazar bulunamadı.";
                return RedirectToAction("AuthorList");
            }

            try
            {
                Author.AD = p.AD.Trim();
                Author.SOYAD = p.SOYAD.Trim();
                Author.DETAY = p.DETAY;
                db.SaveChanges();
                TempData["Message"] = "Yazar bilgileri güncellendi.";
                return RedirectToAction("AuthorList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Yazar güncellenirken bir hata oluştu.";
                return RedirectToAction("UpdateAuthor", new { id = p.ID });
            }
        }

        [HttpGet]
        public ActionResult AuthorBooks(int id)
        {
            var AuthorBooks = db.TBL_KITAP.Include(x=>x.TBL_KATEGORI).Include(x=>x.TBL_YAZAR).Where(x => x.YAZAR == id).ToList();
            if (!AuthorBooks.Any())
            {
                TempData["Error"] = "Bu yazara ait kitap bulunamadı.";
                return RedirectToAction("AuthorList");
            }

            return View(AuthorBooks);
        }
    }
}