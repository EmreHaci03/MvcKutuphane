using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
            db.TBL_YAZAR.Add(p);
            db.SaveChanges();
            return RedirectToAction("AuthorList");
        }

        [HttpPost]
        public ActionResult DeleteAuthor(int id)
        {
            var Author = db.TBL_YAZAR.Find(id);
            db.TBL_YAZAR.Remove(Author);
            db.SaveChanges();
            return RedirectToAction("AuthorList");
        }

        [HttpGet]
        public ActionResult UpdateAuthor(int id)
        {
            var Author = db.TBL_YAZAR.Find(id);
            return View(Author);
        }

        [HttpPost]
        public ActionResult UpdateAuthor(TBL_YAZAR p)
        {
            var Author = db.TBL_YAZAR.Find(p.ID);
            Author.AD = p.AD;
            Author.SOYAD= p.SOYAD;
            Author.DETAY= p.DETAY;
            db.SaveChanges();
            return RedirectToAction("AuthorList");
        }
    }
}