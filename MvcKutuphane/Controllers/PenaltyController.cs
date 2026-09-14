using MvcKutuphane.Models.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class PenaltyController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult PenaltyList()
        {
            var Penalty = db.TBL_CEZALAR.Include(x=>x.TBL_UYELER).OrderByDescending(x=>x.ID).ToList();
            return View(Penalty);
        }
    }
}