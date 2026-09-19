using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MvcKutuphane.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
       
        public ActionResult Index()
        {
            ViewBag.TotalBook = db.TBL_KITAP.Count();
            ViewBag.TotalMember = db.TBL_UYELER.Count();
            ViewBag.ActiveLend = db.TBL_HAREKET.Count(x=>x.UYEGETIRDIGITARIH==null);
            ViewBag.ActivePenalty = db.TBL_CEZALAR.Count();

            ViewBag.Last5Lend = db.TBL_HAREKET.Include(x=>x.TBL_KITAP).Include(x=>x.TBL_UYELER).Include(x=>x.TBL_PERSONEL).OrderByDescending(x => x.ID).Take(5).ToList();

            var BookStatus = db.TBL_KITAP
                .GroupBy(x => x.DURUM)
                .Select(g => new
                {
                    Durum = g.Key,
                    Count = g.Count()
                }).ToList();

            ViewBag.ShelfBook = BookStatus.Where(x => x.Durum == true).Select(x => x.Count).FirstOrDefault();
            ViewBag.BorrowedBook = BookStatus.Where(x => x.Durum == false).Select(x => x.Count).FirstOrDefault();



            var startDate = DateTime.Now.AddDays(-6).Date;


            var DayGroup = db.TBL_HAREKET
                .Where(x => x.ALISTARIH.HasValue && x.ALISTARIH > startDate)
                .ToList()
                .GroupBy(x => x.ALISTARIH.Value.Date)
               .Select(g => new
               {
                   Tarih = g.Key,
                   Count = g.Count()
               }).OrderBy(x =>x.Tarih)
               .ToList();

            ViewBag.DayTags = JsonConvert.SerializeObject(DayGroup.Select(x => x.Tarih.ToString("dd MMM yyyy")));
            ViewBag.DayData = JsonConvert.SerializeObject(DayGroup.Select(x => x.Count));


            var MostActiveMember = db.TBL_HAREKET
                .GroupBy(x => x.UYE)
                .Select(g => new
                {
                    MemberId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList()
                .Select(x =>
                {
                    var uye = db.TBL_UYELER.FirstOrDefault(u => u.ID == x.MemberId);
                    return new
                    {
                        Ad = uye != null ? uye.AD + " " + uye.SOYAD : "-",
                        Sayi = x.Count
                    };
                }).ToList();

            ViewBag.MemberTag = JsonConvert.SerializeObject(MostActiveMember.Select(x => x.Ad));
            ViewBag.MemberData = JsonConvert.SerializeObject(MostActiveMember.Select(x => x.Sayi));

            var categoryDistribution = db.TBL_KITAP
                .Where(x => x.TBL_KATEGORI != null)
                .GroupBy(x => x.TBL_KATEGORI.CategoryName)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                }).OrderByDescending(x => x.Count).ToList();


            ViewBag.CategoryTags = JsonConvert.SerializeObject(categoryDistribution.Select(x => x.Category));
            ViewBag.CategoryData = JsonConvert.SerializeObject(categoryDistribution.Select(x => x.Count));
            return View();
        }
    }
}