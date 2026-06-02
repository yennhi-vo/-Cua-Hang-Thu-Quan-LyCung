using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetShop.Models;

namespace PetShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();
        //public ActionResult Index()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
        //    }
        //    return View(cart);
        //}

        public ActionResult Index()
        {
            // Lấy giỏ hàng từ Session
            Cart cart = (Cart)Session["Cart"];

            // Nếu chưa có giỏ hàng trong Session, phải khởi tạo mới hoàn toàn
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart; // Quan trọng: Phải gán lại vào Session
            }

            return View(cart); // Truyền đối tượng cart (không được null) sang View
        }

        //public ActionResult AddToCart(int id) 
        //{
        //    if (Session["User"] == null)
        //    {
        //        return RedirectToAction("Login", "User");
        //    }
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart(); //khởi tạo lại giỏ hàng nếu chưa có 
        //    }
        //    int result = cart.Them(id);
        //    if (result == 1)
        //    { //thêm thành công
        //        Session["Cart"] = cart;
        //        return RedirectToAction("Index", "Cart");
        //    }
        //    ViewBag.Error = "Lỗi không thể thêm sản phẩm vào giỏ hàng. Vui lòng thử lại!";
        //    return RedirectToAction("Detail", "Home", new { id = id });
        //}

        // Sửa tham số truyền vào: thêm 'loai'
        public ActionResult AddToCart(int id, string loai)
        {
            if (Session["User"] == null) return RedirectToAction("Login", "User");

            Cart cart = (Cart)Session["Cart"] ?? new Cart();

            // Truyền cả ID và Loại (ví dụ: "PhuKien")
            int result = cart.Them(id, loai);

            if (result == 1)
            {
                Session["Cart"] = cart;
                return RedirectToAction("Index", "Cart");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult RemoveFromCart(int id, string loai)
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                int result = cart.Xoa(id, loai);
                if (result == 1)
                {
                    Session["Cart"] = cart;
                }
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult UpdateSL(int id, string loai, int type) // 1: tăng, 2: giảm
        {
            Cart cart = (Cart)Session["Cart"] ?? new Cart();
            int result = -1;

            if (type == 1) // Tăng
            {
                result = cart.Them(id, loai);
            }
            else // Giảm
            {
                result = cart.Giam(id, loai);
            }

            if (result == 1)
            {
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult CheckOut()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null || cart.SoLuongMH() == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // Lấy thông tin mặc định của khách hàng từ Session
            KhachHang user = (KhachHang)Session["User"];
            return View(user);
        }

        [HttpPost]
        public ActionResult ProcessOrder(string DiaChiGiaoHang, string SDT, int ship = 35000)
        {
            Cart cart = (Cart)Session["Cart"];
            KhachHang user = (KhachHang)Session["User"];

            if (user == null) return RedirectToAction("Login", "User");
            if (cart == null || cart.cart.Count == 0) return RedirectToAction("Index", "Product");

            // 1. Tạo hóa đơn mới
            HoaDon hd = new HoaDon()
            {
                MaKH = user.MaKH,
                MaNV = 1,
                NgayLap = DateTime.Now,
                TongTien = (decimal)cart.TongPhaiThanhToan() + ship,
                TinhTrang = 1,
                DiaChiGiaoHang = DiaChiGiaoHang,
                DaThanhToan = false
            };

            db.HoaDons.Add(hd);
            db.SaveChanges(); // Lưu để lấy MaHD tự động tăng

            // 2. Lưu chi tiết hóa đơn (QUAN TRỌNG: Phân loại bảng để lưu đúng cột)
            foreach (var item in cart.cart)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon()
                {
                    MaHD = hd.MaHD,
                    SoLuong = item.SoLuong,
                    DonGia = item.Gia,
   
            };
                ct.MaThuCung = item.MaSP;
                ct.MaPhuKien = item.MaSP;
              

                db.ChiTietHoaDons.Add(ct);
            }

            db.SaveChanges();

            // 3. Dọn dẹp
            Session["Cart"] = new Cart();
            TempData["MaHD"] = hd.MaHD;
            return RedirectToAction("ConfirmPayment");
        }
        public ActionResult ConfirmPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApDungMaKM(string maCode)
        {
            // Thay x.MaKM bằng x.CodeKM
            var km = db.KhuyenMais.FirstOrDefault(x => x.CodeKM == maCode && x.NgayKetThuc >= DateTime.Now);

            if (km != null)
            {
                Cart cart = (Cart)Session["Cart"];
                if (cart != null)
                {
                   
                    cart.PhanTramGiam = (double)km.PhanTramGiam;
                    Session["Cart"] = cart;
                    TempData["Success"] = "Áp dụng mã giảm giá thành công!";
                }
            }
            else
            {
                TempData["Error"] = "Mã giảm giá không hợp lệ hoặc đã hết hạn!";
            }
            return RedirectToAction("Index");
        }
    }
}