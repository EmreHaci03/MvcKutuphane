using MvcKutuphane.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        public ActionResult Index()
        {
            ViewBag.ToplamKitap = db.TBL_KITAP.Count();
            ViewBag.ToplamUye = db.TBL_UYELER.Count();
            ViewBag.ToplamKategori = db.TBL_KATEGORI.Count();

            ViewBag.SonKitaplar = db.TBL_KITAP.Include(x => x.TBL_YAZAR).OrderByDescending(x => x.ID).Take(3).ToList();
            ViewBag.OneCikanlar = db.TBL_KITAP.Include(x => x.TBL_YAZAR).OrderByDescending(x => x.ID).Take(4).ToList();
            ViewBag.Kategoriler = db.TBL_KATEGORI.Take(8).ToList();

            return View();
        }
    }
}