using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace MvcKutuphane.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberMyPenaltiesController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult Index()
        {
            string Mail = Session["Mail"] as string;
            if (string.IsNullOrEmpty(Mail))
            {
                TempData["Error"] = "Cezalarınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == Mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var MyPenalties = db.TBL_CEZALAR
                .Include(x => x.TBL_HAREKET)
                .Where(x => x.UYE == Member.ID && x.ODENDI == false)   // <-- sadece ödenmemişler
                .OrderByDescending(x => x.ID)
                .ToList();
            return View(MyPenalties);
        }

        [HttpGet]
        public ActionResult MyOldPenalties()
        {
            string Mail = Session["Mail"] as string;
            if (string.IsNullOrEmpty(Mail))
            {
                TempData["Error"] = "Cezalarınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == Mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var MyOldPenalties = db.TBL_CEZALAR
                .Include(x => x.TBL_HAREKET)
                .Where(x => x.UYE == Member.ID && x.ODENDI == true)   // <-- sadece ödenmişler
                .OrderByDescending(x => x.ID)
                .ToList();
            return View(MyOldPenalties);
        }


        [HttpPost]
        public ActionResult PayPenalty(int id)
        {
            string Mail = Session["Mail"] as string;

            if (string.IsNullOrEmpty(Mail))
            {
                TempData["Error"] = "Mesajlarınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == Mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var myPenalty = db.TBL_CEZALAR.FirstOrDefault(x => x.ID == id && x.UYE == Member.ID);

            if (myPenalty == null)
            {
                TempData["Error"] = "Ceza kaydı bulunamadı.";
                return RedirectToAction("Index");
            }


            if (myPenalty.ODENDI == true)
            {
                TempData["Error"] = "Bu Ceza Zaten Ödenmiş.";
                return RedirectToAction("Index");
            }
            try
            {
                myPenalty.ODENDI = true;
                db.SaveChanges();
                TempData["Message"] = "Cezanız başarıyla ödendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ödeme işlemi sırasında bir hata oluştu.";
            }

            return RedirectToAction("Index"); ;
        }

      
    }
}