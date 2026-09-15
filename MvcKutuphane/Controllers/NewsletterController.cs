using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class NewsletterController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpPost]
        public ActionResult Index(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                TempData["Error"] = "Lütfen e-posta adresinizi giriniz.";
                return RedirectToAction("Index","Default");
            }

            if (!Email.Contains("@"))
            {
                TempData["Error"] = "Lütfen geçerli bir e-posta adresi giriniz.";
                return RedirectToAction("Index", "Default");
            }
            try
            {
                var Newsletter = new TBL_BULTEN
                {
                    MAIL = Email
                };
                db.TBL_BULTEN.Add(Newsletter);
                db.SaveChanges();
                TempData["Message"] = "Bültenimize başarıyla abone oldunuz.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Abonelik sırasında bir hata oluştu, lütfen tekrar deneyiniz.";
            }
            return RedirectToAction("Index", "Default");
        }
    }
}