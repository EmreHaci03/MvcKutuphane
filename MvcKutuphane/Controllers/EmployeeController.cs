using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class EmployeeController : Controller
    {
        DbKutuphaneEntities2 db=new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult EmployeeList()
        {
            var Employee = db.TBL_PERSONEL.ToList();
            return View(Employee);
        }

        [HttpGet]
        public ActionResult CreateEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateEmployee(TBL_PERSONEL p)
        {
            if (string.IsNullOrWhiteSpace(p.PERSONEL))
            {
                TempData["Error"] = "Personel Adı Boş Bırakılamaz";
                return RedirectToAction("CreateEmployee");
            }


            bool anyEmployee = db.TBL_PERSONEL.Any(x => x.PERSONEL.Trim().ToLower() == p.PERSONEL.Trim().ToLower());
            if (anyEmployee)
            {
                TempData["Error"] = "Bu Personel Adı Zaten kullanılıyor.";
                return RedirectToAction("CreateEmployee");
            }
            try
            {
                p.PERSONEL=p.PERSONEL.Trim();   
                db.TBL_PERSONEL.Add(p);
                db.SaveChanges();
                return RedirectToAction("EmployeeList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Personel Eklenirken Bir Hata Oluştu.";
                return RedirectToAction("CreateEmployee");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(int id)
        {
            var Employee = db.TBL_PERSONEL.Find(id);

            if (Employee == null)
            {
                TempData["Error"] = "Personel bulunamadı.";
                return RedirectToAction("EmployeeList");
            }

            try
            {
                db.TBL_PERSONEL.Remove(Employee);
                db.SaveChanges();
                TempData["Message"] = "Personel silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Bu personel, ödünç işlem kayıtlarında (TBL_HAREKET) kullanıldığı için silinemiyor.";
            }

            return RedirectToAction("EmployeeList");
        }

        [HttpGet]
        public ActionResult UpdateEmployee(int id)
        {
            var Employee = db.TBL_PERSONEL.Find(id);
            if (Employee == null)
            {
                TempData["Error"] = "Güncellenmek İstenen Personel Bilgileri Alınamadı.";
                return RedirectToAction("EmployeeList");
            }
            return View(Employee);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(TBL_PERSONEL p)
        {
            if (string.IsNullOrWhiteSpace(p.PERSONEL))
            {
                TempData["Error"] = "Personel Adı Boş Bırakılamaz.";
                return RedirectToAction("UpdateEmployee", new { id = p.ID });
            }

            var Employee = db.TBL_PERSONEL.Find(p.ID);
            if (Employee == null)
            {
                TempData["Error"] = "Güncellenecek Personel bulunamadı.";
                return RedirectToAction("EmployeeList");
            }
            try
            {
                Employee.PERSONEL = p.PERSONEL.Trim();
                db.SaveChanges();
                return RedirectToAction("EmployeeList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Personel Güncellenirken Bir Hata Oluştu.";
                return RedirectToAction("UpdateEmployee", new { id = p.ID });
            }          
        }

    }
}
