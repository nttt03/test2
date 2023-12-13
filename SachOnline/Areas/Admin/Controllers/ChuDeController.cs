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
    public class ChuDeController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/ChuDe
        public ActionResult Index(int ? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Hiển thị S ds CD và S NXB  đồng thời chọn chủ đề và nxb của cuốn hiện tại
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", chude.MaCD);      
            return View(chude);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == int.Parse(f["iMaChuDe"]));
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", chude.MaCD);        

            if (ModelState.IsValid)
            {
                //Lưu vào CSDL
                //chude.MaCD = int.Parse(f["MaCD"]);
                chude.TenChuDe = f["sTenChuDe"];            
                db.SubmitChanges();
                //Về lại trang Quản lý sách
                return RedirectToAction("Index");
            }
            return View(chude);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var cd = db.CHUDEs.Where(ct => ct.MaCD == id);
            if (cd.Count() > 0)
            {
                ViewBag.ThongBao = "Chủ đề này đang có trong bảng Sách <br>" +
                    "Nếu muốn xoá thì phải xoá hết mã chủ đề này trong bảng Sách";
                return View(chude);
            }
            
            // Xoá 
            db.CHUDEs.DeleteOnSubmit(chude);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CHUDE cd, FormCollection f)
        {

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            if (ModelState.IsValid)
            {
                //Lưu vào CSDL
                cd.TenChuDe = f["sTenCD"];
                db.CHUDEs.InsertOnSubmit(cd);
                db.SubmitChanges();
                
                // Về lại trang Quản lý          
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}