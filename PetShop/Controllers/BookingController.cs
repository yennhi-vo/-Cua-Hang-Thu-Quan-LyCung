using System;
using System.Linq;
using System.Web.Mvc;
using PetShop.Models;

namespace PetShop.Controllers
{
    public class BookingController : Controller
    {
        SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();

        // GET: Hiển thị trang đặt lịch
        public ActionResult Index()
        {
            // Nếu khách đã đăng nhập, tự điền thông tin vào Form
            if (Session["User"] != null)
            {
                var user = (KhachHang)Session["User"];
                ViewBag.HoTen = user.HoTen;
                ViewBag.SDT = user.SDT;
            }
            return View();
        }

        // POST: Xử lý khi nhấn nút Đặt lịch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DatLich booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (Session["User"] != null)
                    {
                        var user = (KhachHang)Session["User"];
                        booking.MaKH = user.MaKH;
                    }

                    booking.NgayHen = booking.NgayHen; 
                    booking.TinhTrang = "Chờ xác nhận";

                    db.DatLiches.Add(booking);
                    db.SaveChanges();

                    TempData["Success"] = "Đặt lịch thành công! Chúng tôi sẽ liên hệ sớm nhất.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            return View("Index", booking);
        }
    }
}