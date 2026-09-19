using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult Index()
        {
            ViewBag.BookCount = db.TBL_KITAP.Count();
            ViewBag.ActiveMember = db.TBL_UYELER.Count();
            ViewBag.Category = db.TBL_KATEGORI.Count();
            return View();
        }
    }
}