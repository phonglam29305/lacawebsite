using laca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laca.Controllers
{
    public class HomeController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult systemcontent(laca.Models.SytemContent id = 0)
        {
            tbl_SystemContent tbl_SystemContent = db.tbl_SystemContent.Find(id);
            if (tbl_SystemContent == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SystemContent);
        }
        
    }
}
