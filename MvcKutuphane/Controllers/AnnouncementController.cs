using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class AnnouncementController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult AnnouncementList()
        {
            var duyurular = db.TBL_DUYURU.OrderByDescending(x => x.ID).ToList();
            return View(duyurular);
        }

        [HttpGet]
        public ActionResult CreateAnnouncement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAnnouncement(TBL_DUYURU p)
        {
            if (string.IsNullOrWhiteSpace(p.BASLIK) || string.IsNullOrWhiteSpace(p.ICERIK))
            {
                TempData["Error"] = "Başlık ve içerik boş bırakılamaz.";
                return RedirectToAction("CreateAnnouncement");
            }

            try
            {
                p.BASLIK = p.BASLIK.Trim();
                p.ICERIK = p.ICERIK.Trim();
                p.YAYINTARIHI = DateTime.Now;

                db.TBL_DUYURU.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Duyuru yayınlandı.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Duyuru eklenirken bir hata oluştu.";
                return RedirectToAction("CreateAnnouncement");
            }

            return RedirectToAction("AnnouncementList");
        }

        [HttpPost]
        public ActionResult DeleteAnnouncement(int id)
        {
            var duyuru = db.TBL_DUYURU.Find(id);
            if (duyuru == null)
            {
                TempData["Error"] = "Duyuru bulunamadı.";
                return RedirectToAction("Index");
            }

            try
            {
                db.TBL_DUYURU.Remove(duyuru);
                db.SaveChanges();
                TempData["Message"] = "Duyuru silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Duyuru silinirken bir hata oluştu.";
            }

            return RedirectToAction("AnnouncementList");
        }

        [HttpGet]
        public ActionResult UpdateAnnouncement(int id)
        {
            var Announcement = db.TBL_DUYURU.Find(id);
            if (Announcement==null)
            {
                TempData["Error"] = "Duyuru Detayına Ulaşılamadı";
                return View();
            }
            return View(Announcement);
        }
        [HttpPost]
        public ActionResult UpdateAnnouncement(TBL_DUYURU p)
        {
            if (string.IsNullOrWhiteSpace(p.BASLIK) ||
                string.IsNullOrWhiteSpace(p.ICERIK))
            {
                TempData["Error"] = "Başlık ve içerik boş bırakılamaz.";
                return RedirectToAction("UpdateAnnouncement", new { id = p.ID });
            }

            try
            {
                var duyuru = db.TBL_DUYURU.Find(p.ID);

                if (duyuru == null)
                {
                    TempData["Error"] = "Duyuru bulunamadı.";
                    return RedirectToAction("AnnouncementList");
                }

                duyuru.BASLIK = p.BASLIK.Trim();
                duyuru.ICERIK = p.ICERIK.Trim();

                db.SaveChanges();

                TempData["Message"] = "Duyuru başarıyla güncellendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Duyuru güncellenirken bir hata oluştu.";
            }

            return RedirectToAction("AnnouncementList");
        }

    }
}