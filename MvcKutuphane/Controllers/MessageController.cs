using MvcKutuphane.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    public class MessageController : Controller
    {
        DbKutuphaneEntities2 db = new DbKutuphaneEntities2();

        [HttpGet]
        public ActionResult MessageList()
        {
            var Messages = db.TBL_MESAJ.OrderByDescending(x => x.ID).ToList();
            return View(Messages);
        }

        [HttpGet]
        public ActionResult MessageDetail(int id)
        {
            var Message=db.TBL_MESAJ.Find(id);

            if (Message == null)
            {
                TempData["Error"] = "Mesaj bulunamadı.";
                return RedirectToAction("MessageList");
            }

            return View(Message);
        }


        [HttpPost]
        public ActionResult DeleteMessage(int id)
        {
            var Message = db.TBL_MESAJ.Find(id);
            if (Message == null)
            {
                TempData["Error"] = "Mesaj bulunamadı.";
                return RedirectToAction("MessageList");
            }

            try
            {
                db.TBL_MESAJ.Remove(Message);
                db.SaveChanges();
                TempData["Message"] = "Mesaj silindi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Mesaj silinirken bir hata oluştu.";
            }

            return RedirectToAction("MessageList");
        } 
    }
}