using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetShop.ViewModels;

namespace PetShop.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();
        public ActionResult Profile()
        {
            var khSession = (KhachHang)Session["User"];
            int maKH = khSession.MaKH;

            // Thay vì FindAll, ta dùng Where
            var dsLichHen = db.DatLiches.Where(x => x.MaKH == maKH).ToList();
            var dsHoaDon = db.HoaDons.Where(x => x.MaKH == maKH).ToList();

            ViewBag.LichHen = dsLichHen;
            ViewBag.HoaDon = dsHoaDon;

            return View(db.KhachHangs.Find(maKH));
        }
        // Giao diện chỉnh sửa
        [HttpGet]
        public ActionResult ChinhSuaHoSo(int id)
        {
            var kh = db.KhachHangs.Find(id);
            if (kh == null) return HttpNotFound();
            return View(kh);
        }

        // Xử lý lưu hồ sơ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaHoSo(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                var item = db.KhachHangs.Find(kh.MaKH);
                if (item != null)
                {
                    item.HoTen = kh.HoTen;
                    item.Email = kh.Email;
                    item.SDT = kh.SDT;
                    item.DiaChi = kh.DiaChi;
               

                    db.SaveChanges();
                    // Cập nhật lại Session nếu cần hiển thị tên mới trên header
                    Session["User"] = item;

                    return RedirectToAction("Profile", new { id = item.MaKH });
                }
            }
            return View(kh);
        }
    }
}