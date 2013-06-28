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
    public class ItemGroupController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        //
        // GET: /ItemGroup/

        [Authorize]
        public ActionResult Index()
        {
            var tbl_itemgroup = db.tbl_ItemGroup.Include(t => t.tbl_ItemType);
            return View(tbl_itemgroup.ToList());
        }

        //
        // GET: /ItemGroup/Details/5

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            tbl_ItemGroup tbl_itemgroup = db.tbl_ItemGroup.Find(id);
            if (tbl_itemgroup == null)
            {
                return HttpNotFound();
            }
            return View(tbl_itemgroup);
        }

        //
        // GET: /ItemGroup/Create

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ItemTypeID = new SelectList(db.tbl_ItemType, "ItemTypeID", "ItemTypeName");
            return View();
        }

        //
        // POST: /ItemGroup/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_ItemGroup tbl_itemgroup)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ItemGroup.Add(tbl_itemgroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemTypeID = new SelectList(db.tbl_ItemType, "ItemTypeID", "ItemTypeName", tbl_itemgroup.ItemTypeID);
            return View(tbl_itemgroup);
        }

        //
        // GET: /ItemGroup/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            tbl_ItemGroup tbl_itemgroup = db.tbl_ItemGroup.Find(id);
            if (tbl_itemgroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemTypeID = new SelectList(db.tbl_ItemType, "ItemTypeID", "ItemTypeName", tbl_itemgroup.ItemTypeID);
            return View(tbl_itemgroup);
        }

        //
        // POST: /ItemGroup/Edit/5

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_ItemGroup tbl_itemgroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_itemgroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeID = new SelectList(db.tbl_ItemType, "ItemTypeID", "ItemTypeName", tbl_itemgroup.ItemTypeID);
            return View(tbl_itemgroup);
        }

        //
        // GET: /ItemGroup/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            tbl_ItemGroup tbl_itemgroup = db.tbl_ItemGroup.Find(id);
            if (tbl_itemgroup == null)
            {
                return HttpNotFound();
            }
            return View(tbl_itemgroup);
        }

        //
        // POST: /ItemGroup/Delete/5

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_ItemGroup tbl_itemgroup = db.tbl_ItemGroup.Find(id);
            if (db.tbl_Items.Where(a => a.ItemGroupID == id).Count() > 0)
            {
                ModelState.AddModelError("ItemGroupName", "Đã tồn tại sản phẩm thuộc nhóm sản phẩm này, bạn không thể xóa!");
                return View(tbl_itemgroup);
            }
            db.tbl_ItemGroup.Remove(tbl_itemgroup);
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