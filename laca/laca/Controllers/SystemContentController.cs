using laca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laca.Controllers
{
    public class SystemContentController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();
        //
        // GET: /SystemContent/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(laca.Models.SytemContent id = 0)
        {
            tbl_SystemContent tbl_SystemContent = db.tbl_SystemContent.Find(id);
            if (tbl_SystemContent == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SystemContent);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost,ValidateInput(false)]
        public ActionResult Edit(tbl_SystemContent content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(content);
        }
    }
}
