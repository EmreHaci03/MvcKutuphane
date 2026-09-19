using MvcKutuphane.Models;
using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcKutuphane.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberProfileController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult Index()
        {

            string mail = Session["Mail"] as string;


            if (string.IsNullOrEmpty(mail))
            {
                TempData["Error"] = "Oturum bilgisi bulunamadı, lütfen tekrar giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            var member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == mail);

            if (member == null)
            {
                TempData["Error"] = "Üye bilgisi bulunamadı.";
                return RedirectToAction("Login", "Account");
            }
            return View(member);
        }


        [HttpGet]
        public ActionResult EditInformation()
        {
            string mail = Session["Mail"] as string;
            var Member = db.TBL_UYELER.FirstOrDefault(x=>x.MAIL == mail);   


            if (Member == null)
            {
                TempData["Error"] = "Kullanıcı Bilgileri Alınamadı.";
                return RedirectToAction("Login","Account");
            }

            return View(Member);

        }

        [HttpPost]
        public ActionResult EditInformation(TBL_UYELER p)
        {
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.SOYAD) || string.IsNullOrWhiteSpace(p.MAIL))
            {
                TempData["Error"] = "Ad, soyad ve e-posta boş bırakılamaz.";
                return RedirectToAction("Edit");
            }

            string mail = Session["Mail"] as string;
            var Member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == mail);


            if (Member == null)
            {
                TempData["Error"] = "Kullanıcı Bilgileri Alınamadı.";
                return RedirectToAction("Login", "Account");
            }


            Member.AD= p.AD.Trim();
            Member.SOYAD=p.SOYAD.Trim();
            Member.MAIL=p.MAIL.Trim();
            Member.KULLANICIADI = p.KULLANICIADI.Trim();
            Member.TELEFON = p.TELEFON.Trim();
            Member.OKUL = p.OKUL.Trim();
            Member.FOTOGRAF = p.FOTOGRAF.Trim();

            db.SaveChanges();

            Session["Mail"] = Member.MAIL;

            TempData["Message"] = "Bilgileriniz güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            string mail = Session["Mail"] as string;

            if (string.IsNullOrEmpty(mail))
            {
                TempData["Error"] = "Oturum bilgisi bulunamadı, lütfen tekrar giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }
            return View();

        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel p)
        {
            string mail = Session["Mail"] as string;

            if (string.IsNullOrWhiteSpace(p.Password) || string.IsNullOrWhiteSpace(p.NewPassword) || string.IsNullOrWhiteSpace(p.NewPasswordAgain))
            {
                TempData["Message"] = "Lütfen Boş Alan Bırakmayınız";
                return View();
            }


            var Member=db.TBL_UYELER.FirstOrDefault(x=>x.MAIL== mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgisi bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            if (Member.SIFRE != p.Password)
            {
                TempData["Error"] = "Mevcut şifreniz yanlış.";
                return RedirectToAction("ChangePassword");
            }

            if(p.NewPassword != p.NewPasswordAgain)
            {
                TempData["Error"] = "Yeni şifreler eşleşmiyor.";
                return RedirectToAction("ChangePassword");
            }

            if (p.NewPassword == p.Password)
            {
                TempData["Error"] = "Yeni şifreniz eski şifrenizle aynı olamaz.";
                return RedirectToAction("ChangePassword");
            }

            if (p.NewPassword.Length < 6)
            {
                TempData["Error"] = "Yeni şifre en az 6 karakter olmalıdır.";
                return RedirectToAction("ChangePassword");
            }

            Member.SIFRE = p.NewPassword.Trim();
            db.SaveChanges();

            FormsAuthentication.SignOut();
            Session.Clear();

            TempData["Message"] = "Şifreniz değiştirildi. Lütfen yeni şifrenizle tekrar giriş yapın.";
            return RedirectToAction("Login", "Account");
        }
    }

} 