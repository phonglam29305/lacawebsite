using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using laca.Models;
using PagedList;

namespace laca.Controllers
{
    public class ImportController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        //
        // GET: /Import/
        [Authorize]
        public ActionResult Index( string fDate="", string tDate="", int page = 1)
        {
            var result = from b in db.tbl_Imports select b;
            DateTime fromDate = DateTime.Now.Date;
            string[] temp=fDate.Split('/');
            if(temp.Length==3)
            {
                fromDate = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[0]));
                result = result.Where(a => a.ImportDate >= fromDate);
            }
            temp = tDate.Split('/');
            if (temp.Length == 3)
            {
                fromDate = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[0]));
                result = result.Where(a => a.ImportDate <= fromDate);
            }
            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;
            result = result.OrderBy("ImportDate");
            int maxRecords = Convert.ToInt32(ConfigurationManager.AppSettings["ListItemCount"]);
            int currentPage = page;
            ViewBag.CurrentPage = page;

            return View(result.ToPagedList(currentPage, maxRecords));
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
        public ActionResult AddDetail(int ImportID = 0, int ItemID = 0, int Qty = 0, decimal Price = 0)
        {
            var import = db.tbl_Imports.Find(ImportID);
            tbl_ImportDetail detail = import.tbl_ImportDetail.Where(a => a.ItemID == ItemID).FirstOrDefault();
            if (detail != null)
            {
                detail.Qty += Qty;
                db.Entry(detail).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                detail = new tbl_ImportDetail();
                detail.ItemID = ItemID;
                detail.ImportID = ImportID;
                detail.Qty = Qty;
                detail.Price = Price;

                db.tbl_ImportDetail.Add(detail);
                db.SaveChanges();
            }
            tbl_Items item = db.tbl_Items.Find(ItemID);
            item.ItemCount += Qty;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = ImportID });
        }
        [Authorize]
        public RedirectToRouteResult RemoveDetail(int ImportID, int ItemID)
        {
            var import = db.tbl_Imports.Find(ImportID);
            tbl_ImportDetail detail = import.tbl_ImportDetail.Where(a => a.ItemID == ItemID).FirstOrDefault();
            if (detail != null)
            {
                db.tbl_ImportDetail.Remove(detail);
                tbl_Items item = db.tbl_Items.Find(ItemID);
                item.ItemCount -= detail.Qty.Value;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = ImportID });
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
            List<tbl_ImportDetail> arr = tbl_imports.tbl_ImportDetail.ToList();
            foreach (var item in arr)
                {
                    tbl_Items sp = db.tbl_Items.Find(item.ItemID);
                    if (sp != null)
                    {
                        sp.ItemCount -= item.Qty.Value;
                        db.Entry(sp).State = EntityState.Modified;
                    }
                }
            

            foreach (var item in arr)
                db.tbl_ImportDetail.Remove(item);

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