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
    public class ItemTypeController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        //
        // GET: /ItemType/

        public ActionResult Index()
        {
            return View(db.tbl_ItemType.ToList());
        }
        

        //
        // GET: /ItemType/Details/5

        public ActionResult Details(int id = 0)
        {
            tbl_ItemType tbl_itemtype = db.tbl_ItemType.Find(id);
            if (tbl_itemtype == null)
            {
                return HttpNotFound();
            }
            return View(tbl_itemtype);
        }

        //
        // GET: /ItemType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ItemType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_ItemType tbl_itemtype)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ItemType.Add(tbl_itemtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_itemtype);
        }

        //
        // GET: /ItemType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            tbl_ItemType tbl_itemtype = db.tbl_ItemType.Find(id);
            if (tbl_itemtype == null)
            {
                return HttpNotFound();
            }
            return View(tbl_itemtype);
        }

        //
        // POST: /ItemType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_ItemType tbl_itemtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_itemtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_itemtype);
        }

        //
        // GET: /ItemType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tbl_ItemType tbl_itemtype = db.tbl_ItemType.Find(id);
            if (tbl_itemtype == null)
            {
                return HttpNotFound();
            }
            return View(tbl_itemtype);
        }

        //
        // POST: /ItemType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_ItemType tbl_itemtype = db.tbl_ItemType.Find(id);
            db.tbl_ItemType.Remove(tbl_itemtype);
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