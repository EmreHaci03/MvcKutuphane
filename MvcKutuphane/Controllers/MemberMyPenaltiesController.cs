using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [Authorize]
    public class MemberMyPenaltiesController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult Index()
        {
            string Mail = Session["Mail"] as string;

            if (string.IsNullOrEmpty(Mail))
            {
                TempData["Error"] = "Mesajlarınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member=db .TBL_UYELER.FirstOrDefault(x=>x.MAIL== Mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var MyPenalties = db.TBL_CEZALAR.Where(x => x.UYE == Member.ID).OrderByDescending(x => x.ID).ToList(); 
            return View(MyPenalties);
        }
    }
}