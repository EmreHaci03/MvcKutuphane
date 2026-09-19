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
    public class StatisticController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();
        public ActionResult Index()
        {
            var MostBookAuthor = db.TBL_KITAP
                .Where(x=>x.TBL_YAZAR!=null)
                .GroupBy(x => x.TBL_YAZAR.AD + " " +  x.TBL_YAZAR.SOYAD)
                .Select(g => new
                {
                    Author=g.Key,
                    Count=g.Count()
                }).OrderByDescending(x=>x.Count).FirstOrDefault();

            ViewBag.MostBookAuthorName = MostBookAuthor!=null ? MostBookAuthor.Author : " - ";
            ViewBag.MostBookCount = MostBookAuthor != null ? MostBookAuthor.Count : 0;


            var MostBookPublishingHouse = db.TBL_KITAP
                .Where(x => x.YAYINEVI != null)
                .GroupBy(x => x.YAYINEVI)
                .Select(g => new
                {
                    Publishing = g.Key,
                    BookCount = g.Count(),
                }).OrderByDescending(x => x.BookCount).FirstOrDefault();

            ViewBag.MostBookPublishing = MostBookPublishingHouse.Publishing;
            ViewBag.MostBookPublishingCount = MostBookPublishingHouse.BookCount;

            var mostPopularCategory=db.TBL_KITAP
                .Where(x=>x.TBL_KATEGORI != null)
                .GroupBy(y=>y.TBL_KATEGORI.CategoryName)
                .Select(g => new
                {
                    KATEGORI = g.Key,   
                    BookCount = g.Count(),
                }).OrderByDescending(x=>x.BookCount).FirstOrDefault();


            ViewBag.CategoryName = mostPopularCategory.KATEGORI;
            ViewBag.BookCountMostCategory = mostPopularCategory.BookCount;

            var mostActiveMember=db.TBL_HAREKET
                .Where(x=>x.UYE!=null)
                .GroupBy(x=>x.TBL_UYELER.AD + " " + x.TBL_UYELER.SOYAD)
                .Select(g => new
                {
                    NameSurname=g.Key,
                    Count=g.Count()
                }).OrderByDescending(x=>x.Count).FirstOrDefault();

            ViewBag.MostActiveMemberName = mostActiveMember.NameSurname;
            ViewBag.CountTransactions=mostActiveMember.Count;


            var mostTransactionEmployee = db.TBL_HAREKET
                .Where(x => x.PERSONEL != null)
                .GroupBy(x => x.TBL_PERSONEL.PERSONEL)
                .Select(g => new
                {
                    NameSurname = g.Key,
                    Count = g.Count()
                }).OrderByDescending(x => x.Count).FirstOrDefault();

            ViewBag.EmployeeName = mostTransactionEmployee.NameSurname;
            ViewBag.Count = mostTransactionEmployee.Count;

            var mostLendBook=db.TBL_HAREKET
                .Where(x=>x.KITAP!=null)
                .GroupBy(x=>x.TBL_KITAP.AD)
                .Select(g => new
                {
                    Ad=g.Key,
                    Count=g.Count()
                }).OrderByDescending(x => x.Count).FirstOrDefault();

            ViewBag.LendBookName = mostLendBook.Ad;
            ViewBag.LendBookCount = mostLendBook.Count;



            var startDate = DateTime.Now.AddDays(-30);

            var last30DaysReturns = db.TBL_HAREKET
                .Where(x => x.UYEGETIRDIGITARIH.HasValue &&
                            x.UYEGETIRDIGITARIH.Value >= startDate)
                .ToList();

            int totalReturns = last30DaysReturns.Count;
            int onTimeReturns = last30DaysReturns.Count(x =>
                x.IADETARIH.HasValue &&
                x.UYEGETIRDIGITARIH.Value <= x.IADETARIH.Value);

            decimal onTimeReturnRate = totalReturns > 0
                ? Math.Round((decimal)onTimeReturns / totalReturns * 100, 0)
                : 0;

            ViewBag.OnTimeReturnRate = onTimeReturnRate;


            // Ortalama İade Ödünç Gün Sayısı
           double averageLoanDays=last30DaysReturns.Any()
                ? last30DaysReturns.Average(x=>(x.UYEGETIRDIGITARIH.Value -x.ALISTARIH.Value).TotalDays) : 0;

            ViewBag.AverageLoanDays=Math.Round(averageLoanDays, 0);







            ViewBag.TotalPenaltySum = db.TBL_CEZALAR.Where(x=>x.CEZABASLANGIC.HasValue && x.CEZABASLANGIC.Value.Month==DateTime.Now.Month  && x.CEZABASLANGIC.Value.Year==DateTime.Now.Year).Sum(x=>x.CEZA);
            return View();
        }
    }
}