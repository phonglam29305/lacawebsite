using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using laca.Models;

namespace laca.Controllers
{
    public class ImportController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        //
        // GET: /Import/
        [Authorize]
        public ActionResult Index()
        {
            return View(db.tbl_Imports.ToList());
        }

        //
        // GET: /Import/Details/5

        public ActionResult Details(int id = 0)
        {
            tbl_Imports tbl_imports = db.tbl_Imports.Find(id);
            if (tbl_imports == null)
            {
                return HttpNotFound();
            }
            return View(tbl_imports);
        }

        //
        // GET: /Import/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ImportDate = DateTime.Now.ToString("dd/MM/yyyy");
            return View();
        }

        //
        // POST: /Import/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Imports tbl_imports)
        {
            var date = DateTime.Now;
            tbl_imports.ImportDate = null;
            string[] s = (Request.Form["datepicker"] + "").Split('/');
            try
            {
                date = new DateTime(Convert.ToInt16(s[2]), Convert.ToInt16(s[1]), Convert.ToInt16(s[0]));
                tbl_imports.ImportDate = date;
            }
            catch (Exception e) { ModelState.AddModelError("DeliveryDate", "Ngày nhập hàng chưa đúng"); }

            if (ModelState.IsValid)
            {
                tbl_imports.ImportUser = User.Identity.Name;
                db.tbl_Imports.Add(tbl_imports);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_imports);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetail(int id = 0, int ItemID = 0)
        {
            var date = DateTime.Now;

            tbl_Imports tbl_imports = db.tbl_Imports.Find(id);

            tbl_imports.ImportUser = User.Identity.Name;
            db.tbl_Imports.Add(tbl_imports);
            db.SaveChanges();
            return View("Edit");
        }

        //
        // GET: /Import/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            tbl_Imports tbl_imports = db.tbl_Imports.Find(id);
            if (tbl_imports == null)
            {
                return HttpNotFound();
            }
            if (tbl_imports.ImportDate != null)
                ViewBag.ImportDate = tbl_imports.ImportDate.Value.ToString("dd/MM/yyyy");
            ViewBag.ItemID = new SelectList(db.tbl_Items, "ItemID", "ItemName");
            return View(tbl_imports);
        }

        //
        // POST: /Import/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Imports tbl_imports)
        {
            var date = DateTime.Now;
            tbl_imports.ImportDate = null;
            string[] s = (Request.Form["datepicker"] + "").Split('/');
            try
            {
                date = new DateTime(Convert.ToInt16(s[2]), Convert.ToInt16(s[1]), Convert.ToInt16(s[0]));
                tbl_imports.ImportDate = date;
            }
            catch (Exception e) { ModelState.AddModelError("DeliveryDate", "Ngày nhập hàng chưa đúng"); }

            if (ModelState.IsValid)
            {
                db.Entry(tbl_imports).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.tbl_Items, "ItemID", "ItemName");
            return View(tbl_imports);
        }

        //
        // GET: /Import/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            tbl_Imports tbl_imports = db.tbl_Imports.Find(id);
            if (tbl_imports == null)
            {
                return HttpNotFound();
            }
            return View(tbl_imports);
        }

        //
        // POST: /Import/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Imports tbl_imports = db.tbl_Imports.Find(id);
            db.tbl_Imports.Remove(tbl_imports);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}