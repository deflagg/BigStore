using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigStoreBL;

namespace BigStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SubsystemManager mgr = new SubsystemManager();
            ViewBag.SubsystemName = mgr.GetSubsystemName();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}