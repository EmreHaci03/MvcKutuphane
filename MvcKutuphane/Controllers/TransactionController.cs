using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();   
        public ActionResult ReturnedBooksList()
        {
            var TransactionList = db.TBL_HAREKET.Include(x => x.TBL_PERSONEL).Include(x => x.TBL_KITAP).Include(x=>x.TBL_UYELER).Where(x => x.UYEGETIRDIGITARIH != null).OrderByDescending(x=>x.UYEGETIRDIGITARIH).ToList();
            return View(TransactionList);
        }
    }
}