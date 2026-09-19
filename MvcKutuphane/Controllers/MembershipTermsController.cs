using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKutuphane.Controllers
{
    [AllowAnonymous]
    public class MembershipTermsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}