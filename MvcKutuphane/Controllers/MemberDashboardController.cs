using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberDashboardController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        public ActionResult Index()
        {
            string mail = Session["Mail"] as string;
            if (string.IsNullOrEmpty(mail))
            {
                TempData["Error"] = "Panelinizi görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var Member = db.TBL_UYELER.FirstOrDefault(x => x.MAIL == mail);
            if (Member == null)
            {
                TempData["Error"] = "Üye bilgileri bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            var MemberActiveBooks = db.TBL_HAREKET
                .Where(x => x.UYEGETIRDIGITARIH == null && x.UYE == Member.ID)
                .ToList();

            ViewBag.ActiveLendCount = MemberActiveBooks.Count;
            ViewBag.ActivelendBook = MemberActiveBooks;
            ViewBag.SumPenalty = db.TBL_CEZALAR.Where(x => x.UYE == Member.ID && x.ODENDI == false).Sum(x => x.CEZA);
            ViewBag.TotalBookCount = db.TBL_HAREKET.Where(x => x.UYE == Member.ID).Count();

            // ---- Haftalık aktivite grafiği ----
            var StartDate = DateTime.Now.AddDays(-6).Date;

            var WeekActivity = db.TBL_HAREKET
                .Where(x => x.UYE == Member.ID && x.ALISTARIH.HasValue && x.ALISTARIH >= StartDate)
                .ToList()
                .GroupBy(x => x.ALISTARIH.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(),
                })
                .OrderBy(x => x.Date)
                .ToList();

            ViewBag.WeekLabels = Newtonsoft.Json.JsonConvert.SerializeObject(WeekActivity.Select(x => x.Date.ToString("dd MMM")));
            ViewBag.WeekData = Newtonsoft.Json.JsonConvert.SerializeObject(WeekActivity.Select(x => x.Count));

            // ---- Kategori dağılımı ----
            var MyCategoryDistribution = db.TBL_HAREKET
                .Include(x => x.TBL_KITAP.TBL_KATEGORI)
                .Where(x => x.UYE == Member.ID && x.KITAP != null && x.TBL_KITAP.TBL_KATEGORI != null)
                .ToList()
                .GroupBy(x => x.TBL_KITAP.TBL_KATEGORI.CategoryName)
                .Select(x => new
                {
                    Category = x.Key,
                    Count = x.Count(),
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            ViewBag.CategoryLabels = Newtonsoft.Json.JsonConvert.SerializeObject(MyCategoryDistribution.Select(x => x.Category));
            ViewBag.CategoryData = Newtonsoft.Json.JsonConvert.SerializeObject(MyCategoryDistribution.Select(x => x.Count));

            var MostReadBookCategory = MyCategoryDistribution.FirstOrDefault();
            ViewBag.TopCategoryName = MostReadBookCategory != null ? MostReadBookCategory.Category : "-";

            return View();
        }
    }
}