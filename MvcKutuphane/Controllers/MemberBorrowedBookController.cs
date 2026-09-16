using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class MemberBorrowedBookController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();


        [HttpGet]
        public ActionResult ActiveLend()
        {
            string mail = Session["Mail"] as string;
            if (string.IsNullOrEmpty(mail))
            {
                TempData["Error"] = "Mesajlarınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var MyActiveBorrowed = db.TBL_HAREKET
                .Include(x => x.TBL_KITAP)
                .Where(x => x.UYE == Member.ID && x.UYEGETIRDIGITARIH == null)
                .OrderByDescending(x => x.ID)
                .ToList();
            return View(MyActiveBorrowed);
        }

        [HttpGet]
        public ActionResult History()
        {
            string mail = Session["Mail"] as string;
            if (string.IsNullOrEmpty(mail))
            {
                TempData["Error"] = "Mesajlarınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member=db.TBL_UYELER.FirstOrDefault(x=>x.MAIL== mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var MyOldBorrowed = db.TBL_HAREKET
                .Include(x => x.TBL_KITAP)
                .Where(x => x.UYE == Member.ID && x.UYEGETIRDIGITARIH != null)
                .OrderByDescending(x => x.ID)
                .ToList();
            return View(MyOldBorrowed);
        }
    }
}