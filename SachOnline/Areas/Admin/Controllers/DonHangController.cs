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
    public class DonHangController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/DonHang
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Details(int id)
        {
            var dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }

        public ActionResult Edit(int id)
        {
            var dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
     
            return View(dh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == int.Parse(f["iMaDH"]));

            if (ModelState.IsValid)
            {

                dh.DaThanhToan = Convert.ToBoolean(f["bDaThanhToan"]);
                dh.TinhTrangGiaoHang = int.Parse(f["sTinhTrang"]);
                dh.NgayDat = Convert.ToDateTime(f["dNgayDat"]);
                dh.NgayGiao = Convert.ToDateTime(f["dNgayGiao"]);
                dh.MaKH = int.Parse(f["sMaKH"]);
                db.SubmitChanges();
                //Về lại trang Quản lý 
                return RedirectToAction("Index");
            }
            return View(dh);
        }

    }
}