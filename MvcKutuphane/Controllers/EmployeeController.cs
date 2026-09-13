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
            db.TBL_PERSONEL.Add(p);
            db.SaveChanges();
            return RedirectToAction("EmployeeList");
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
            return View(Employee);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(TBL_PERSONEL p)
        {
            var Employee = db.TBL_PERSONEL.Find(p.ID);
            Employee.PERSONEL = p.PERSONEL;
            db.SaveChanges();
            return RedirectToAction("EmployeeList");
        }

    }
}