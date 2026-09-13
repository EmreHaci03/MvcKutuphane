using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class MemberController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult MemberList()
        {
            var Member = db.TBL_UYELER.ToList();
            return View(Member);
        }

        [HttpGet]
        public ActionResult CreateMember()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(TBL_UYELER p)
        {
            db.TBL_UYELER.Add(p);
            db.SaveChanges();
            return RedirectToAction("MemberList");
        }

        [HttpPost]
        public ActionResult DeleteMember(int id)
        {
            var Member = db.TBL_UYELER.Find(id);
            db.TBL_UYELER.Remove(Member);
            db.SaveChanges();
            return RedirectToAction("MemberList");
        }

        [HttpGet]
        public ActionResult UpdateMember(int id)
        {
            var Member = db.TBL_UYELER.Find(id);
            return View(Member);
        }

        [HttpPost]
        public ActionResult UpdateMember(TBL_UYELER p)
        {
            var Member = db.TBL_UYELER.Find(p.ID);
            if (Member == null)
            {
                TempData["Error"] = "Üye Bilgisi Bulunamadı";
                return RedirectToAction("MemberList");
            }
            Member.AD = p.AD;
            Member.SOYAD = p.SOYAD;
            Member.MAIL = p.MAIL;
            Member.KULLANICIADI = p.KULLANICIADI;
            Member.SIFRE = p.SIFRE;
            Member.FOTOGRAF = p.FOTOGRAF;
            Member.TELEFON=p.TELEFON;
            Member.OKUL = p.OKUL;
            db.SaveChanges();
            return RedirectToAction("MemberList");
        }

    }
}