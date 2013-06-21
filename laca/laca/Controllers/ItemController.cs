using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using laca.Models;
using System.Configuration;
using laca.Utils;
using System.IO;
using PagedList;

namespace laca.Controllers
{
    public class ItemController : Controller
    {
        private lacashop_dbEntities db = new lacashop_dbEntities();

        //
        // GET: /Item/
        [Authorize]
        public ActionResult Index(string sortOrder, string Keyword, int? ItemTypeID, int? ItemGroupID, bool? IsHot, bool? IsDiscount, bool? IsNew, bool? IsHotDeal, int page = 1)
        {
            ViewBag.ItemName = (sortOrder == "ItemName") ? "ItemName desc" : "ItemName";
            ViewBag.ItemTypeName = (sortOrder == "ItemTypeName") ? "ItemTypeName desc" : "ItemTypeName";
            ViewBag.ItemGroupName = (sortOrder == "ItemGroupName") ? "ItemGroupName desc" : "ItemGroupName";
            ViewBag.ViewCount = (sortOrder == "ViewCount") ? "ViewCount desc" : "ViewCount";
            ViewBag.ItemCount = (sortOrder == "ItemCount") ? "ItemCount desc" : "ItemCount";

            if (string.IsNullOrWhiteSpace(sortOrder))
                sortOrder = "OrderID";
            ViewBag.CurrentSortOrder = sortOrder;

            var result = from b in db.tbl_Items select b;
            if (!String.IsNullOrEmpty(Keyword))
            {
                result = result.Where(b => b.IsShow && b.ItemName.ToUpper().Contains(Keyword.ToUpper()));
            }
            ViewBag.CurrentKeyword = String.IsNullOrEmpty(Keyword) ? "" : Keyword;

            if (ItemTypeID != null)
                result = result.Where(a => a.tbl_ItemGroup.ItemTypeID == ItemTypeID.Value);

            if (ItemGroupID != null)
                result = result.Where(a => a.ItemGroupID == ItemGroupID.Value);

            ViewBag.ItemTypeID = db.tbl_ItemType.ToSelectList("ItemTypeID", "ItemTypeName", ItemTypeID);
            ViewBag.ItemGroupID = db.tbl_ItemGroup.ToSelectList("ItemGroupID", "ItemGroupName", ItemGroupID);

            ViewBag.TypeID = ItemTypeID;
            ViewBag.GroupID = ItemGroupID;

            if (IsHot != null && IsHot.Value)
                result = result.Where(a => a.IsHot);

            if (IsNew != null && IsNew.Value)
                result = result.Where(a => a.IsHot);

            if (IsHotDeal != null && IsHotDeal.Value)
                result = result.Where(a => a.IsHot);

            if (IsDiscount != null && IsDiscount.Value)
                result = result.Where(a => a.DiscountPrice > 0);

            ViewBag.IsHot = IsHot;
            ViewBag.IsDiscount = IsDiscount;
            ViewBag.IsNew = IsNew;
            ViewBag.IsHotDeal = IsHotDeal;

            result = result.OrderBy(sortOrder);
            int maxRecords = Convert.ToInt32(ConfigurationManager.AppSettings["ListItemCount"]);
            int currentPage = page;
            ViewBag.CurrentPage = page;

            return View(result.ToPagedList(currentPage, maxRecords));
        }

        public ActionResult ItemByItemType(int id = 0, int page = 1)
        {
            var tbl_items = db.tbl_Items.Where(a => a.IsShow && a.tbl_ItemGroup.ItemTypeID == id);
            int maxRecords = Convert.ToInt32(ConfigurationManager.AppSettings["PageItemCount"]);
            int currentPage = page;
            ViewBag.CurrentPage = page;
            tbl_items = tbl_items.OrderBy("OrderID");
            ViewBag.CurrentID = id;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentSortOrder = "OrderID";
            return View(tbl_items.ToPagedList(currentPage, maxRecords));
        }

        public ActionResult ItemByItemGroup(int id = 0, int page = 1)
        {
            var tbl_items = db.tbl_Items.Where(a => a.IsShow && a.ItemGroupID == id);
            int maxRecords = Convert.ToInt32(ConfigurationManager.AppSettings["PageItemCount"]);
            tbl_items = tbl_items.OrderBy("OrderID");
            int currentPage = page;
            ViewBag.CurrentID = id;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentSortOrder = "OrderID";
            return View(tbl_items.ToPagedList(currentPage, maxRecords));
        }

        public ActionResult ItemByProperties(string key, int ItemGroupID, int m0_s1_c2_new3_km4_hot5 = 0, int page = 1)
        {
            IEnumerable<tbl_Items> tbl_items = db.tbl_Items.Where(a => a.IsShow && a.Material.Contains(key));
            if (m0_s1_c2_new3_km4_hot5 == 1)
                tbl_items = db.tbl_Items.Where(a => a.IsShow && a.ItemGroupID == ItemGroupID && a.Style.Contains(key));
            else
                if (m0_s1_c2_new3_km4_hot5 == 2)
                    tbl_items = db.tbl_Items.Where(a => a.IsShow && a.Color.Contains(key));
                else
                    if (m0_s1_c2_new3_km4_hot5 == 3)
                        tbl_items = db.tbl_Items.Where(a => a.IsShow && a.IsNew);
                    else
                        if (m0_s1_c2_new3_km4_hot5 == 4)
                            tbl_items = db.tbl_Items.Where(a => a.IsShow && a.DiscountPrice > 0);
                        else
                            if (m0_s1_c2_new3_km4_hot5 == 5)
                                tbl_items = db.tbl_Items.Where(a => a.IsShow && a.IsHotDeal);
            ViewBag.CurrentKey = key;
            ViewBag.CurrentItemGroupID = ItemGroupID;
            ViewBag.Currentm0_s1_c2_new3_km4_hot5 = m0_s1_c2_new3_km4_hot5;
            ViewBag.CurrentSortOrder = "OrderID";
            int maxRecords = Convert.ToInt32(ConfigurationManager.AppSettings["PageItemCount"]);
            tbl_items = tbl_items.OrderBy(a => a.OrderID);
            int currentPage = page;
            ViewBag.CurrentPage = page;
            switch (m0_s1_c2_new3_km4_hot5)
            {
                case 0: ViewBag.Title = "Sản phẩm theo chất liệu: " + key; break;
                case 1: ViewBag.Title = "Sản phẩm theo kiểu dáng: " + key; break;
                case 2: ViewBag.Title = "Sản phẩm theo màu sắc: " + key; break;
                case 3: ViewBag.Title = "Sản phẩm mới"; break;
                case 4: ViewBag.Title = "Sản phẩm khuyến mãi"; break;
                case 5: ViewBag.Title = "Sản phẩm bán chạy nhất"; break;
            }
            return View(tbl_items.ToPagedList(currentPage, maxRecords));
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
            tbl_items.ViewCount += 1;
            db.Entry(tbl_items).State = EntityState.Modified;
            db.SaveChanges();
            return View(tbl_items);
        }

        //
        // GET: /Item/Create

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName");
            return View();
        }

        //
        // POST: /Item/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Items tbl_items, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string filesPath = "", full_path = "";
                if (file != null)
                {
                    char DirSeparator = System.IO.Path.DirectorySeparatorChar;
                    filesPath = ConfigurationManager.AppSettings["ItemImages"];
                    full_path = Server.MapPath(filesPath).Replace("Item", "");
                    tbl_items.Images = FileUpload.UploadFile(file, full_path);
                }

                db.tbl_Items.Add(tbl_items);
                db.SaveChanges();

                if (file != null)
                {
                    string filename = tbl_items.ItemID + "_" + file.FileName.Replace(" ", "_").Replace("-", "_");
                    tbl_items.Images = FileUpload.UploadFile(file, filename, full_path);
                    db.Entry(tbl_items).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName", tbl_items.ItemGroupID);
            return View(tbl_items);
        }

        //
        // GET: /Item/Edit/5

        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Items tbl_items, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    char DirSeparator = System.IO.Path.DirectorySeparatorChar;
                    string FilesPath = ConfigurationManager.AppSettings["ItemImages"];
                    string full_path = Server.MapPath(FilesPath).Replace("Item", "").Replace("Edit", "");
                    if (tbl_items.Images + "" != "")
                        FileUpload.DeleteFile(tbl_items.Images, full_path);

                    tbl_items.Images = FileUpload.UploadFile(file, full_path);
                }

                db.Entry(tbl_items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemGroupID = new SelectList(db.tbl_ItemGroup, "ItemGroupID", "ItemGroupName", tbl_items.ItemGroupID);
            return View(tbl_items);
        }

        //
        // GET: /Item/Delete/5

        [Authorize]
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

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Items tbl_items = db.tbl_Items.Find(id);
            db.tbl_Items.Remove(tbl_items);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ViewResult ImageList(int id)
        {
            tbl_Items item = db.tbl_Items.Find(id);
            string path = "~/Images/uploads/" + item.ItemID;
            path = Server.MapPath(path);
            List<string> list = new List<string>();
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string s in files)
                {
                    string filename = Path.GetFileName(s);
                    System.IO.File.Move(s, s.Replace(" ", "_").Replace("-", "_"));
                    if (filename.ToLower().IndexOf(".jpg") >= 0 || filename.ToLower().IndexOf(".png") >= 0 || filename.ToLower().IndexOf(".gif") >= 0)
                        list.Add("../../images/uploads/" + item.ItemID + "/" + filename.Replace(" ", "_").Replace("-", "_"));

                }
            }
            ViewBag.ImageList = list;
            return View(item);
        }
        //[Authorize]
        public PartialViewResult AddImages(int id)
        {
            var item = db.tbl_Items.Find(id);
            ViewBag.IDHangHoa = id;
            return PartialView(item);
        }
        [HttpPost]
        //[Authorize]
        public ActionResult AddImages(int id, HttpPostedFileBase file)
        {
            var item = db.tbl_Items.Find(id);
            if (ModelState.IsValid)
            {
                //HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;

                if (file != null)
                {
                    char DirSeparator = System.IO.Path.DirectorySeparatorChar;
                    string FilesPath = ConfigurationManager.AppSettings["ItemImages"];
                    string full_path = Server.MapPath(FilesPath).Replace("Item", "").Replace("AddImages", "").Replace(" ", "_").Replace("-", "_") + "\\" + item.ItemID;
                    FileUpload.UploadFile(file, full_path);
                }
                return RedirectToAction("ImageList", new { id = item.ItemID });
            }
            return View(item);
        }
        //[Authorize]
        public ActionResult DeleteImage(int id, string image)
        {
            string file = Server.MapPath(image.Replace("-", " "));
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
            var item = db.tbl_Items.Find(id);
            return RedirectToAction("ImageList", new { id = item.ItemID });

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}