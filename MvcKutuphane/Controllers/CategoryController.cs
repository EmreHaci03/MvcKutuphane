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
            if (string.IsNullOrWhiteSpace(p.CategoryName))
            {
                TempData["Error"] = "Kategori adı boş olamaz.";
                return RedirectToAction("CreateCategory");
            }
            bool AnyCategory = db.TBL_KATEGORI.Any(x => x.CategoryName.Trim().ToLower() == p.CategoryName.Trim().ToLower());
            if (AnyCategory)
            {
                TempData["Error"] = "Bu Kategori Adına Sahip Kayıt Zaten Mevcut.";
                return RedirectToAction("CreateCategory");
            }

            try
            {
                p.CategoryName = p.CategoryName.Trim();
                db.TBL_KATEGORI.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Bu isimde bir kategori zaten mevcut olabilir veya kategori eklenirken bir hata oluştu.";
                return RedirectToAction("CreateCategory");
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            var Category = db.TBL_KATEGORI.Find(id);
            if (Category == null)
            {
                TempData["Error"] = "Kategori bulunamadı.";
                return RedirectToAction("Index");
            }

            try
            {
                db.TBL_KATEGORI.Remove(Category);
                db.SaveChanges();
                TempData["Message"] = "Kategori silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Bu kategoriye bağlı kitaplar olduğu için silinemiyor.";
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            var Category = db.TBL_KATEGORI.Find(id);
            if (Category == null)
            {
                  TempData["Error"] = "Kategori bulunamadı.";
                return RedirectToAction("Index");
            }
            return View(Category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(TBL_KATEGORI p)
        {
            if (string.IsNullOrWhiteSpace(p.CategoryName))
            {
                TempData["Error"] = "Kategori Adı Boş Bırakılamaz";
                return RedirectToAction("UpdateCategory", new {id=p.ID});
            }
            var Category = db.TBL_KATEGORI.Find(p.ID);
            if(Category == null)
            {
                TempData["Error"] = "Güncellenmek İstenen Kategori Bulunamadı";
                return RedirectToAction("Index");
            }
            try
            {
                Category.CategoryName = p.CategoryName.Trim(); // Trim Baştaki Ve Sondaki Boşlukları Siler Ortadaki Boşuklara Dokunmaz.
                db.SaveChanges(); 
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Kategori güncellenirken bir hata oluştu.";
                return RedirectToAction("UpdateCategory", new { id = p.ID });
            }          
        }




    }
}