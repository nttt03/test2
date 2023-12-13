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
    public class KhachHangController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }

        public ActionResult Details(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Hiển thị S ds CD và S NXB  đồng thời chọn chủ đề và nxb của cuốn hiện tại
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen", kh.MaKH);
            return View(kh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == int.Parse(f["iMaKH"]));
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen", kh.MaKH);

            if (ModelState.IsValid)
            {
                //Lưu vào CSDL
                //chude.MaCD = int.Parse(f["MaCD"]);
                kh.HoTen = f["sTenKH"];
                kh.TaiKhoan = f["sTaiKhoan"];
                kh.Email = f["sEmail"];              
                kh.DienThoai = f["sDienThoai"];
                kh.DiaChi = f["sDiaChi"];
                kh.NgaySinh = Convert.ToDateTime(f["dNgaySinh"]);
                db.SubmitChanges();
                //Về lại trang Quản lý 
                return RedirectToAction("Index");
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHACHHANG kh, FormCollection f)
        {

            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            if (ModelState.IsValid)
            {
                //Lưu vào CSDL
                kh.HoTen = f["sTenKH"];
                kh.TaiKhoan = f["sTaiKhoan"];
                kh.MatKhau = f["sMatKhau"];
                kh.Email = f["sEmail"];
                kh.DienThoai = f["sDienThoai"];
                kh.DiaChi = f["sDiaChi"];
                kh.NgaySinh = Convert.ToDateTime(f["dNgaySinh"]);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                    
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Xoá 
            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}