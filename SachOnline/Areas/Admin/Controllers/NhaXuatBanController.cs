using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace SachOnline.Areas.Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/NhaXuatBan
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(iPageNum, iPageSize));
        }

        public ActionResult Details(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Hiển thị S ds CD và S NXB  đồng thời chọn chủ đề và nxb của cuốn hiện tại
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", nxb.MaNXB);
            return View(nxb);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == int.Parse(f["iMaNXB"]));
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", nxb.MaNXB);

            if (ModelState.IsValid)
            {
                //Lưu vào CSDL
                //chude.MaCD = int.Parse(f["MaCD"]);
                nxb.TenNXB = f["sTenNXB"];
                nxb.DiaChi = f["sDiaChi"];
                nxb.DienThoai = f["sDienThoai"];
                db.SubmitChanges();
                //Về lại trang Quản lý 
                return RedirectToAction("Index");
            }
            return View(nxb);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NHAXUATBAN nxb, FormCollection f)
        {
            
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (ModelState.IsValid)
            {
                //Lưu vào CSDL
                nxb.TenNXB = f["sTenNXB"];
                nxb.DienThoai = f["sDienThoai"];
                nxb.DiaChi = f["sDiaChi"];
                db.NHAXUATBANs.InsertOnSubmit(nxb);
                db.SubmitChanges();
                // Về lại trang Quản lý sách             
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var nxban = db.NHAXUATBANs.Where(ct => ct.MaNXB == id);
            if (nxban.Count() > 0)
            {
                ViewBag.ThongBao = "Nhà xuất bản này đang có trong bảng Sách <br>" +
                    "Nếu muốn xoá thì phải xoá hết mã nhà xuất bản này trong bảng Sách";
                return View(nxb);
            }
            // Xoá 
            db.NHAXUATBANs.DeleteOnSubmit(nxb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}