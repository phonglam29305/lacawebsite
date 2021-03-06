﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using laca.Models;
using PagedList;

namespace laca.Controllers
{
    public class OrderController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();


        //
        // GET: /Order/

        [Authorize]
        public ActionResult Index(string fDate = "", string tDate = "", OrderStatus? status = null, int page = 1)
        {
            var result = from b in db.tbl_Orders select b;
            DateTime fromDate = DateTime.Now.Date;
            string[] temp = fDate.Split('/');
            if (temp.Length == 3)
            {
                fromDate = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[0]));
                result = result.Where(a => a.DeliveryDate >= fromDate);
            }
            temp = tDate.Split('/');
            if (temp.Length == 3)
            {
                fromDate = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[0]));
                result = result.Where(a => a.DeliveryDate <= fromDate);
            }
            if (status != null)
                result = result.Where(a=>a.Status == status);
            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;
            ViewBag.OrderStatus = status;
            result = result.OrderBy("DeliveryDate");
            int maxRecords = Convert.ToInt32(ConfigurationManager.AppSettings["ListItemCount"]);
            int currentPage = page;
            ViewBag.CurrentPage = page;

            return View(result.ToPagedList(currentPage, maxRecords));
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            if (tbl_orders == null)
            {
                if (Session[Session.SessionID] == null)
                    Session[Session.SessionID] = new List<tbl_OrderDetail>();

                List<tbl_OrderDetail> list = Session[Session.SessionID] as List<tbl_OrderDetail>;
                return View(list);
            }

            ViewBag.Style = "visibility: visible";
            if (id > 0) { ViewBag.Style = "visibility: hidden;"; }
            return View(tbl_orders.tbl_OrderDetail.ToList());
        }


        //[HttpPost]
        public RedirectToRouteResult AddItem(int ItemID, decimal Price, int Qtty = 1)
        {
            //int Qtty = 0;
            //int.TryParse(Request.Form["quantity"] + "", out Qtty);
            if (Session[Session.SessionID] == null)
                Session[Session.SessionID] = new List<tbl_OrderDetail>();

            List<tbl_OrderDetail> list = Session[Session.SessionID] as List<tbl_OrderDetail>;
            if (list.Find(a => a.ItemID == ItemID) != null)
            {
                var item = list.Find(a => a.ItemID == ItemID);
                item.Qty += Qtty;
                item.Amount = item.Qty * item.Price;
            }
            else
            {
                tbl_OrderDetail ct = new tbl_OrderDetail();
                ct.ItemID = ItemID;
                ct.tbl_Items = db.tbl_Items.Find(ItemID);
                ct.Qty = Qtty;
                ct.Price = Price;
                ct.Amount = Qtty * Price;
                list.Add(ct);

            }
            TempData[Session.SessionID] = list;
            return RedirectToAction("Details", "Order");
        }
        public RedirectToRouteResult RemoveItem(int ItemID)
        {
            //int Qtty = 0;
            //int.TryParse(Request.Form["quantity"] + "", out Qtty);
            if (Session[Session.SessionID] == null)
                Session[Session.SessionID] = new List<tbl_OrderDetail>();

            List<tbl_OrderDetail> list = Session[Session.SessionID] as List<tbl_OrderDetail>;
            if (list.Find(a => a.ItemID == ItemID) != null)
                list.Remove(list.Find(a => a.ItemID == ItemID));

            TempData[Session.SessionID] = list;
            return RedirectToAction("Details", "Order");
        }

        [HttpPost]
        public ActionResult OrderComplete(string name, string address, string email, string phone, string cardid)
        {
            tbl_Customers cus = db.tbl_Customers.Where(a => a.Email == email || a.Phone == phone).FirstOrDefault();
            if (cus == null)
            {
                cus = new tbl_Customers();
                cus.CustomerName = name;
                cus.Address = address;
                cus.Email = email;
                cus.Phone = phone;
                cus.CardID = cardid;

                db.tbl_Customers.Add(cus);
                db.SaveChanges();
            }

            tbl_Orders order = new tbl_Orders();
            order.CustomerID = cus.CustomerID;
            order.CreateDate = DateTime.Now;
            order.Status = laca.Models.OrderStatus.Order;
            db.tbl_Orders.Add(order);
            db.SaveChanges();

            List<tbl_OrderDetail> list = Session[Session.SessionID] as List<tbl_OrderDetail>;
            foreach (var item in list)
            {
                item.tbl_Items = null;
                item.OrderID = order.OrderID;
                db.tbl_OrderDetail.Add(item);
                db.SaveChanges();
            }

            return Json(new { url = Url.Action("ThankYou", new { id = order.OrderID }) });
            //return View();
        }
        public ActionResult ThankYou(int id = 0)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            if (tbl_orders == null)
            {
                if (tbl_orders == null)
                {
                    return HttpNotFound();
                }
            }
            var cus = new laca.Models.lacashop_dbEntities().tbl_Customers.Find(tbl_orders.CustomerID);
            ViewBag.cusName = cus.CustomerName;
            Session[Session.SessionID] = null;

            //return View(tbl_orders);
            return View();
        }
        //
        // GET: /Order/Create

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.tbl_Customers, "CustomerID", "CustomerName");
            return View();
        }



        //
        // POST: /Order/Create

        [Authorize]
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

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);

            if (tbl_orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeliveryDate = tbl_orders.DeliveryDate == null ? "" : tbl_orders.DeliveryDate.Value.ToString("dd/MM/yyyy");
            ViewBag.Customer = db.tbl_Customers.Find(tbl_orders.CustomerID).CustomerName;
            ViewBag.Amount = tbl_orders.tbl_OrderDetail.Sum(a => a.Amount).Value.ToString("#,###");
            //ViewBag.currStatus = tbl_orders.Status;
            return View(tbl_orders);
        }

        //
        // POST: /Order/Edit/5

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Orders tbl_orders, OrderStatus currStatus)
        {
            var date = DateTime.Now;
            tbl_orders.DeliveryDate = null;
            string[] s = (Request.Form["datepicker"] + "").Split('/');
            try
            {
                date = new DateTime(Convert.ToInt16(s[2]), Convert.ToInt16(s[1]), Convert.ToInt16(s[0]));
                tbl_orders.DeliveryDate = date;
            }
            catch (Exception e) { if (tbl_orders.Status == OrderStatus.Delivery)ModelState.AddModelError("DeliveryDate", "Ngày giao hàng chưa đúng"); }
            if (ModelState.IsValid)
            {
                //tbl_Orders currOrder = db.tbl_Orders.Find(tbl_orders.OrderID);
                if (tbl_orders.Status == OrderStatus.Delivery && currStatus == OrderStatus.Order)
                {
                    foreach (var item in db.tbl_OrderDetail.Where(a=>a.OrderID == tbl_orders.OrderID))
                    {
                        tbl_Items sp = db.tbl_Items.Find(item.ItemID);
                        sp.ItemCount -= item.Qty.Value;
                        db.Entry(sp).State = EntityState.Modified;
                    }
                }
                if (tbl_orders.Status != OrderStatus.Delivery && currStatus == OrderStatus.Delivery)
                {
                    foreach (var item in db.tbl_OrderDetail.Where(a => a.OrderID == tbl_orders.OrderID))
                    {
                        tbl_Items sp = db.tbl_Items.Find(item.ItemID);
                        sp.ItemCount += item.Qty.Value;
                        db.Entry(sp).State = EntityState.Modified;
                    }
                }
                db.Entry(tbl_orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            tbl_orders = db.tbl_Orders.Find(tbl_orders.OrderID);
            ViewBag.Customer = db.tbl_Customers.Find(tbl_orders.CustomerID).CustomerName;
            ViewBag.DeliveryDate = tbl_orders.DeliveryDate == null ? "" : tbl_orders.DeliveryDate.Value.ToString("dd/MM/yyyy");
            ViewBag.Amount = tbl_orders.tbl_OrderDetail.Sum(a => a.Amount).Value.ToString("#,###");
            return View(tbl_orders);
        }

        //
        // GET: /Order/Delete/5

        [Authorize]
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

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Orders tbl_orders = db.tbl_Orders.Find(id);
            List<tbl_OrderDetail> arr = tbl_orders.tbl_OrderDetail.ToList();
            if (tbl_orders.Status == OrderStatus.Delivery)
            {
                foreach (var item in arr)
                {
                    tbl_Items sp = db.tbl_Items.Find(item.ItemID);
                    if (sp != null)
                    {
                        sp.ItemCount += item.Qty.Value;
                        db.Entry(sp).State = EntityState.Modified;
                    }
                }
            }
            
            foreach(var item in arr)
                db.tbl_OrderDetail.Remove(item);
            
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