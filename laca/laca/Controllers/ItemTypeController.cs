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

        [Authorize]
        public ActionResult Index()
        {
            return View(db.tbl_ItemType.ToList());
        }
        

        //
        // GET: /ItemType/Details/5

        [Authorize]
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

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ItemType/Create

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_ItemType tbl_itemtype = db.tbl_ItemType.Find(id);
            tbl_ItemGroup tbl_itemgroup = db.tbl_ItemGroup.Find(id);
            if (db.tbl_Items.Where(a => a.ItemGroupID == id).Count() > 0)
            {
                ModelState.AddModelError("ItemTypeName", "Đã tồn tại nhóm sản phẩm thuộc loại sản phẩm này, bạn không thể xóa!");
                return View(tbl_itemtype);
            }
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