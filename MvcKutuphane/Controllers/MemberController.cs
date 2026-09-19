using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [Authorize(Roles = "Admin")]
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
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.SOYAD))
            {
                TempData["Error"] = "Ad ve soyad alanları boş bırakılamaz.";
                return RedirectToAction("CreateMember");
            }

            bool AnyMember = db.TBL_UYELER.Any(x => x.KULLANICIADI.Trim().ToLower() == p.KULLANICIADI.Trim().ToLower());
            if (AnyMember)
            {
                TempData["Error"] = "Bu kullanıcı adı zaten kullanılıyor.";
                return RedirectToAction("CreateMember");
            }

            try
            {
                p.AD = p.AD.Trim();
                p.SOYAD = p.SOYAD.Trim();
                p.KULLANICIADI = p.KULLANICIADI.Trim();
                p.MAIL = p.MAIL?.Trim();
                p.TELEFON = p.TELEFON?.Trim();
                p.OKUL = p.OKUL?.Trim();

                db.TBL_UYELER.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Üye eklendi.";
                return RedirectToAction("MemberList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Üye eklenirken bir hata oluştu.";
                return RedirectToAction("CreateMember");
            }
        }

        [HttpPost]
        public ActionResult DeleteMember(int id)
        {
            var Member = db.TBL_UYELER.Find(id);
            if (Member == null)
            {
                TempData["Error"] = "Üye bulunamadı.";
                return RedirectToAction("MemberList");
            }
            try
            {
                db.TBL_UYELER.Remove(Member);
                db.SaveChanges();
                TempData["Message"] = "Üye silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Üye Silinirken Hata Oluştu Lütfen Tekrar Deneyiniz.";
            }
            return RedirectToAction("MemberList");
        }

        [HttpGet]
        public ActionResult UpdateMember(int id)
        {
            var Member = db.TBL_UYELER.Find(id);
            if (Member == null)
            {
                TempData["Error"] = "Güncellenmek İstenen üye Bilgileri Alınamadı.";
                return RedirectToAction("MemberList");
            }
            return View(Member);
        }

        [HttpPost]
        public ActionResult UpdateMember(TBL_UYELER p)
        {
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.SOYAD))
            {
                TempData["Error"] = "Ad ve soyad alanları boş bırakılamaz.";
                return RedirectToAction("UpdateMember", new { id = p.ID });
            }

            var Member = db.TBL_UYELER.Find(p.ID);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgisi bulunamadı.";
                return RedirectToAction("MemberList");
            }

            try
            {
                Member.AD = p.AD.Trim();
                Member.SOYAD = p.SOYAD.Trim();
                Member.MAIL = p.MAIL?.Trim();
                Member.KULLANICIADI = p.KULLANICIADI?.Trim();
                if (!string.IsNullOrWhiteSpace(p.SIFRE))
                {
                    Member.SIFRE = p.SIFRE.Trim();
                }
                Member.FOTOGRAF = p.FOTOGRAF;
                Member.TELEFON = p.TELEFON?.Trim();
                Member.OKUL = p.OKUL?.Trim();

                db.SaveChanges();
                TempData["Message"] = "Üye bilgileri güncellendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Üye güncellenirken bir hata oluştu.";
                return RedirectToAction("UpdateMember", new { id = p.ID });
            }

            return RedirectToAction("MemberList");
        }

        [HttpGet]
        public ActionResult MemberReturnedBooks(int id)
        {
            var uye = db.TBL_UYELER.Find(id);
            if (uye == null)
            {
                TempData["Error"] = "Üye Bulunamadı.";
                return RedirectToAction("MemberList");
            }

            var MemberBooks = db.TBL_HAREKET.Include(x => x.TBL_KITAP)
                .Include(x => x.TBL_PERSONEL)
                .Include(x => x.TBL_UYELER)
                .Where(x => x.UYE == id && x.UYEGETIRDIGITARIH.HasValue)
                .ToList();

            ViewBag.MemberName = uye.AD + " " + uye.SOYAD;

            return View(MemberBooks);
        }

    }
}