using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [Authorize]
    public class MemberAnnouncementController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult Index()
        {
            var AnnouncementList = db.TBL_DUYURU.ToList();
            return View(AnnouncementList);
        }
    }
}