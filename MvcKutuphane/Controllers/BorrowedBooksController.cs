using MvcKutuphane.Models.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class BorrowedBooksController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult MoveBookList()
        {
            var MoveBook = db.TBL_HAREKET
                         .Include(x => x.TBL_KITAP)
                         .Include(x => x.TBL_PERSONEL)
                         .Include(x => x.TBL_UYELER)
                         .Where(x => x.UYEGETIRDIGITARIH == null ) 
                         .ToList();

            return View(MoveBook);
        }

        [HttpGet]
        public ActionResult CreateLend()
        {
            List<SelectListItem> Book = (from x in db.TBL_KITAP
                                         .Where(x=>x.DURUM==true)
                                         select new SelectListItem
                                         {
                                             Value = x.ID.ToString(),
                                             Text = x.AD
                                         }).ToList();

            ViewBag.Kitaplar = Book;



            List<SelectListItem> Member = (from x in db.TBL_UYELER
                                         select new SelectListItem
                                         {
                                             Value = x.ID.ToString(),
                                             Text = x.AD + " " + x.SOYAD
                                         }).ToList();

            ViewBag.Uyeler = Member;

            List<SelectListItem> Staff = (from x in db.TBL_PERSONEL
                                         select new SelectListItem
                                         {
                                             Value = x.ID.ToString(),
                                             Text = x.PERSONEL
                                         }).ToList();

            ViewBag.Personeller = Staff;

            return View();
        }

        [HttpPost]
        public ActionResult CreateLend(TBL_HAREKET p)
        {
            if (p.ALISTARIH.HasValue && p.IADETARIH.HasValue && p.ALISTARIH.Value > p.IADETARIH.Value)
            {
                TempData["Error"] = "Kitap alış tarihi, iade tarihinden geç olamaz.";
                return RedirectToAction("CreateLend");
            }

            var bookControl = db.TBL_KITAP.Where(x => x.ID == p.KITAP).FirstOrDefault();

            if (bookControl == null)
            {
                TempData["Error"] = "Geçersiz kitap seçimi.";
                return RedirectToAction("CreateLend");
            }

            if (bookControl.DURUM == false)
            {
                TempData["Error"] = "Ödünçte olan kitap tekrar ödünç verilemez.";
                return RedirectToAction("MoveBookList");
            }

            bool existMember = db.TBL_UYELER.Any(x => x.ID == p.UYE);
            bool existEmployee = db.TBL_PERSONEL.Any(x => x.ID == p.PERSONEL);
            if (!existMember || !existEmployee)
            {
                TempData["Error"] = "Geçerli bir üye ve personel seçiniz.";
                return RedirectToAction("CreateLend");
            }

            bool unpaidMemberFine = db.TBL_CEZALAR.Any(x => x.UYE == p.UYE && x.ODENDI == false);
            if (unpaidMemberFine)
            {
                var memberInfo = db.TBL_UYELER.Find(p.UYE);
                var memberNameSurname = memberInfo.AD + " " + memberInfo.SOYAD;
                TempData["Error"] = $"{memberNameSurname} isimli üyenin ödenmemiş cezası bulunuyor. Ceza ödenmeden yeni kitap ödünç verilemez.";
                return RedirectToAction("CreateLend");
            }

            try
            {
                db.TBL_HAREKET.Add(p);
                db.SaveChanges();
                TempData["Message"] = "Kitap ödünç verildi.";
                return RedirectToAction("MoveBookList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Ekleme sırasında hata oluştu, lütfen tekrar deneyiniz.";
                return RedirectToAction("CreateLend");
            }
        }

        [HttpPost]
        public ActionResult DeleteLend(int id)
        {
            var hareket = db.TBL_HAREKET.Find(id);
            if (hareket == null)
            {
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction("MoveBookList");
            }

            try
            {
                db.TBL_HAREKET.Remove(hareket);
                db.SaveChanges();
                TempData["Message"] = "Ödünç kaydı silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Bu kayıt silinemedi.";
            }

            return RedirectToAction("MoveBookList");
        }

        [HttpGet]
        public ActionResult UpdateLend(int id)
        {
            var lendBook = db.TBL_HAREKET.Find(id);

            List<SelectListItem> Book = (from x in db.TBL_KITAP
                                         select new SelectListItem
                                         {
                                             Value = x.ID.ToString(),
                                             Text = x.AD
                                         }).ToList();

            ViewBag.Kitaplar = Book;



            List<SelectListItem> Member = (from x in db.TBL_UYELER
                                           select new SelectListItem
                                           {
                                               Value = x.ID.ToString(),
                                               Text = x.AD + " " + x.SOYAD
                                           }).ToList();

            ViewBag.Uyeler = Member;

            List<SelectListItem> Staff = (from x in db.TBL_PERSONEL
                                          select new SelectListItem
                                          {
                                              Value = x.ID.ToString(),
                                              Text = x.PERSONEL
                                          }).ToList();

            ViewBag.Personeller = Staff;
            return View(lendBook);
        }

        [HttpPost]
        public ActionResult UpdateLend(TBL_HAREKET p)
        {

            var lendBook = db.TBL_HAREKET.Find(p.ID);
            if (lendBook == null)
            {
                TempData["Error"] = "Güncellenecek Kayıt bulunamadı.";
                return RedirectToAction("MoveBookList");
            }
            bool ExistEmployee = db.TBL_PERSONEL.Any(x => x.ID == p.PERSONEL);
            bool ExistMember = db.TBL_UYELER.Any(x => x.ID == p.UYE);
            if(!ExistEmployee  || !ExistMember )
            {
                TempData["Error"] = "Geçerli bir personel ve üye seçiniz.";
                return RedirectToAction("UpdateLend", new { id = p.ID });
            }
            try
            {
                lendBook.KITAP = p.KITAP;
                lendBook.UYE = p.UYE;
                lendBook.PERSONEL = p.PERSONEL;
                lendBook.ALISTARIH = p.ALISTARIH;
                lendBook.IADETARIH = p.IADETARIH;
                db.SaveChanges();
                TempData["Message"] = "Ödünç kaydı güncellendi.";
                return RedirectToAction("MoveBookList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Güncelleme sırasında hata oluştu, lütfen tekrar deneyiniz.";
                return RedirectToAction("UpdateLend", new { id = p.ID });
            }
          
        }

        [HttpPost]
        public ActionResult ReturnBook(int id)
        {
            var hareket = db.TBL_HAREKET.Find(id);
            if (hareket == null)
            {
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction("MoveBookList");
            }

            var kitap = db.TBL_KITAP.Find(hareket.KITAP);
            if (kitap != null)
            {
                kitap.DURUM = true;
            }

            hareket.UYEGETIRDIGITARIH = DateTime.Now;

            if (hareket.IADETARIH.HasValue && hareket.IADETARIH.Value < hareket.UYEGETIRDIGITARIH.Value)
            {
                var gecGunSayisi = (hareket.UYEGETIRDIGITARIH.Value - hareket.IADETARIH.Value).Days;
                decimal cezaTutari = gecGunSayisi * 10;
                var Member = hareket.TBL_UYELER.AD + " " + hareket.TBL_UYELER.SOYAD;
                var bookName = hareket.TBL_KITAP.AD;

                var ceza = new TBL_CEZALAR
                {
                    UYE = hareket.UYE,
                    CEZABASLANGIC = hareket.UYEGETIRDIGITARIH.Value,
                    CEZABITIS = hareket.UYEGETIRDIGITARIH.Value.AddDays(7),
                    CEZA = cezaTutari,
                    HAREKET = hareket.ID
                };
                db.TBL_CEZALAR.Add(ceza);

                TempData["Message"] = $"\"{bookName}\" adlı kitap iade alındı. {Member} isimli üye {gecGunSayisi} gün geç teslim ettiği için {cezaTutari:0.00} TL ceza uygulandı.";
            }
            else
            {
                TempData["Message"] = "Kitap zamanında iade alındı.";
            }

            db.SaveChanges();
            return RedirectToAction("MoveBookList");
        }
    }

}