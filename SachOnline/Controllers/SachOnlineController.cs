using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        dbSachOnlineDataContext data = new dbSachOnlineDataContext();
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a =>
            a.NgayCapNhat).Take(count).ToList();
        }

        private List<SACH> LaySachBanNhieu(int count)
        {
            return data.SACHes.OrderByDescending(a => a.SoLuongBan).Take(count).ToList();
        }

        // GET: SachOnline
        public ActionResult Index()
        {
            // Lay 6 quyen sach moi
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }

        public ActionResult NavPartial()
        {
            return PartialView();
        }

        // Partial
        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in data.CHUDEs select cd;
            return PartialView(listChuDe);
        }

        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(listNhaXuatBan);
        }

        public ActionResult SachBanNhieuPartial()
        {
            var listBanNhieu = LaySachBanNhieu(6);
            return PartialView(listBanNhieu);
        }


        public ActionResult SachTheoChuDe(int iMaCD, int ? page)
        {
            ViewBag.MaCD = iMaCD;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == iMaCD select s;
            return View(sach.ToPagedList(iPageNum, iSize));
        }

        public ActionResult SachTheoNhaXuatBan(int iMaNXB, int ? page)
        {
            ViewBag.MaNXB = iMaNXB;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaNXB == iMaNXB select s;
            return View(sach.ToPagedList(iPageNum, iSize));
        }

        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in data.SACHes
                       where s.MaSach == id select s;
            return View(sach.Single());
        }

        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
            //return Redirect(url);
        }
    }
}