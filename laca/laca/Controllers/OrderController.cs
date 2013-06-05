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
    public class OrderController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        
        //
        // GET: /Order/

        public ActionResult Index()
        {
            var tbl_orders = db.tbl_Orders.Include(t => t.tbl_Customers);
            return View(tbl_orders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            if (tbl_orders == null)
            {
                return HttpNotFound();
            }
            return View(tbl_orders);
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.tbl_Customers, "CustomerID", "CustomerName");
            return View();
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Orders tbl_orders)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Orders.Add(tbl_orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.tbl_Customers, "CustomerID", "CustomerName", tbl_orders.CustomerID);
            return View(tbl_orders);
        }

        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            if (tbl_orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.tbl_Customers, "CustomerID", "CustomerName", tbl_orders.CustomerID);
            return View(tbl_orders);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Orders tbl_orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.tbl_Customers, "CustomerID", "CustomerName", tbl_orders.CustomerID);
            return View(tbl_orders);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            if (tbl_orders == null)
            {
                return HttpNotFound();
            }
            return View(tbl_orders);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            db.tbl_Orders.Remove(tbl_orders);
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