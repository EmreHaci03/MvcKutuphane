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
    [AllowAnonymous]
    public class AdminLoginController : Controller
    {
        DbKutuphaneEntities2 db=new DbKutuphaneEntities2();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(AdminLoginViewModel p)
        {
            if (!ModelState.IsValid)
                return View(p);

            var Admin = db.TBL_ADMIN.FirstOrDefault(x => x.KULLANICI == p.Username && x.SIFRE == p.Password);

            if (Admin == null)
            {
                TempData["Error"] = "Bilgiler Alınamadı Lütfen Tekrar Deneyiniz";
                return View();
            }

            var ticket = new FormsAuthenticationTicket(
                1,
                Admin.KULLANICI,
                DateTime.Now,
                DateTime.Now.AddMinutes(60),
                false,
                "Admin"
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);

            Session["User"] = Admin.KULLANICI;

            return RedirectToAction("Index", "Dashboard");
        }
    }
}