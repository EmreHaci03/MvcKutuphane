using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MvcKutuphane.Controllers
{
    public class NotificationController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult NotificationList()
        {
            var Notifications = db.TBL_BILDIRIM.Include(x=>x.TBL_UYELER).OrderByDescending(x=>x.ID).ToList();
            return View(Notifications);
        }

        [HttpGet]
        public ActionResult CreateNotification()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNotification(TBL_BILDIRIM p)
        {
            return View();
        }


        [HttpPost]
        public ActionResult DeleteNotification(int id)
        {
            var Notification = db.TBL_BILDIRIM.Find(id);
            if (Notification == null)
            {
                TempData["Error"] = "Bildirim bulunamadı.";
                return RedirectToAction("NotificationList");
            }

            try
            {
                db.TBL_BILDIRIM.Remove(Notification);
                db.SaveChanges();
                TempData["Message"] = "Bildirim silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Bildirim silinirken bir hata oluştu.";
            }

            return RedirectToAction("NotificationList");
        }
    }
}