using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcKutuphane.Controllers
{
    public class AccountController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TBL_UYELER p)
        {
            if(string.IsNullOrWhiteSpace(p.MAIL) || string.IsNullOrWhiteSpace(p.SIFRE))
            {
                TempData["Error"] = "E-posta ve şifre boş bırakılamaz.";
                return RedirectToAction("Login");
            }

            var member=db.TBL_UYELER.FirstOrDefault(x=>x.MAIL==p.MAIL && x.SIFRE==p.SIFRE); 

            if(member==null)
            {
                TempData["Error"] = "E-posta veya şifre hatalı.";
                return RedirectToAction("Login");
            }
            FormsAuthentication.SetAuthCookie(member.MAIL, false);


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