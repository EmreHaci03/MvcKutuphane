using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TBL_UYELER p)
        {
            if (string.IsNullOrWhiteSpace(p.AD) || string.IsNullOrWhiteSpace(p.SOYAD) || string.IsNullOrWhiteSpace(p.SIFRE)
                || string.IsNullOrWhiteSpace(p.MAIL) || string.IsNullOrWhiteSpace(p.KULLANICIADI))
            {
                TempData["Error"] = "Lütfen boş alan bırakmayınız.";
                return RedirectToAction("Register");
            }
            if (!p.MAIL.Contains("@"))
            {
                TempData["Error"] = "Lütfen geçerli bir e-posta adresi giriniz.";
                return RedirectToAction("Register");
            }

            if (p.SIFRE.Length < 6)
            {
                TempData["Error"] = "Şifre en az 6 karakter olmalıdır.";
                return RedirectToAction("Register");
            }

            bool AnyMail = db.TBL_UYELER.Any(x => x.MAIL == p.MAIL);
            bool AnyUsername = db.TBL_UYELER.Any(x => x.KULLANICIADI == p.KULLANICIADI);
            if (AnyMail)
            {
                TempData["Error"] = "Bu Maile Sahip Hesap Bulunmaktadır.";
                return RedirectToAction("Register");

            }
            if (AnyUsername)
            {
                TempData["Error"] = "Bu Kullanıcı Adına Sahip Hesap Bulunmaktadır.";
                return RedirectToAction("Register");
            }
            try
            {
                p.AD = p.AD.Trim();
                p.SOYAD = p.SOYAD.Trim();
                p.KULLANICIADI = p.KULLANICIADI.Trim();
                p.SIFRE = p.SIFRE.Trim();
                p.MAIL = p.MAIL.Trim();

                db.TBL_UYELER.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Hesabınız başarıyla oluşturuldu, giriş yapabilirsiniz.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Hesap oluşturma sırasında hata oluştu, lütfen tekrar deneyiniz.";
                return RedirectToAction("Register");
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TBL_UYELER p)
        {
            if (string.IsNullOrWhiteSpace(p.MAIL) || string.IsNullOrWhiteSpace(p.SIFRE))
            {
                TempData["Error"] = "E-posta ve şifre boş bırakılamaz.";
                return RedirectToAction("Login");
            }

            var member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == p.MAIL && x.SIFRE == p.SIFRE);

            if (member == null)
            {
                TempData["Error"] = "E-posta veya şifre hatalı.";
                return RedirectToAction("Login");
            }

            var ticket = new FormsAuthenticationTicket(
                1,
                member.KULLANICIADI,
                DateTime.Now,
                DateTime.Now.AddMinutes(60),
                false,
                "Member"
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);

            Session["Mail"] = member.MAIL;
            TempData["Message"] = "Giriş başarılı, hoş geldiniz " + member.AD + "!";
            return RedirectToAction("Index", "MemberProfile");
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

   
    }
}