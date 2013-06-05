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
    public class ItemController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        //
        // GET: /Item/

        public ActionResult Index()
        {
            var tbl_items = db.tbl_Items.Include(t => t.tbl_ItemGroup);
            return View(tbl_items.ToList());
        }

        //
        // GET: /Item/Details/5

        public ActionResult Details(int id = 0)
        {
            tbl_Items tbl_items = db.tbl_Items.Find(id);
            if (tbl_items == null)
            {
                return HttpNotFound();
            }
            return View(tbl_items);
        }

        //
        // GET: /Item/Create

        public ActionResult Create()
        {
            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName");
            return View();
        }

        //
        // POST: /Item/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Items tbl_items)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Items.Add(tbl_items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName", tbl_items.ItemGroupID);
            return View(tbl_items);
        }

        //
        // GET: /Item/Edit/5

        public ActionResult Edit(int id = 0)
        {
            tbl_Items tbl_items = db.tbl_Items.Find(id);
            if (tbl_items == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName", tbl_items.ItemGroupID);
            return View(tbl_items);
        }

        //
        // POST: /Item/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Items tbl_items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName", tbl_items.ItemGroupID);
            return View(tbl_items);
        }

        //
        // GET: /Item/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tbl_Items tbl_items = db.tbl_Items.Find(id);
            if (tbl_items == null)
            {
                return HttpNotFound();
            }
            return View(tbl_items);
        }

        //
        // POST: /Item/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Items tbl_items = db.tbl_Items.Find(id);
            db.tbl_Items.Remove(tbl_items);
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