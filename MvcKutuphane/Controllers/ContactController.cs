using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(TBL_MESAJ p)
        {
            if (string.IsNullOrWhiteSpace(p.ADSOYAD) || string.IsNullOrWhiteSpace(p.MAIL) ||string.IsNullOrWhiteSpace(p.KONU) || string.IsNullOrWhiteSpace(p.MESAJ))
            {
                TempData["Error"] = "Lütfen Bütün Alanları Eksiksiz Doldurduğunuzdan Emin Olunuz";
                return RedirectToAction("Index");
            }
            if (p.MESAJ.Length > 250)
            {
                TempData["Error"] = "Lütfen En Fazla 250 Karakterlik Bir Mesaj Girişi Yapınız";
                return RedirectToAction("Index");
            }
            if (!p.MAIL.Contains("@"))
            {
                TempData["Error"] = "Lütfen Geçerli Bir Mail Adresi Giriniz";
                return RedirectToAction("Index");
            }
            try
            {
                p.ADSOYAD = p.ADSOYAD.Trim();
                p.MAIL = p.MAIL.Trim();
                p.KONU = p.KONU.Trim();
                p.MESAJ = p.MESAJ.Trim();
                db.TBL_MESAJ.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Mesajınız Alındı En Kısa Sürede Mail Üzerinden Dönüş Yapılacaktır";
            }
            catch (Exception)
            {
                TempData["Error"] = "Mesaj gönderilirken bir hata oluştu, lütfen tekrar deneyiniz.";
            }

            return RedirectToAction("Index");

        }
    }
}