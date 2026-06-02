
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetShop.Controllers
{
    public class UserController : Controller
    {
        SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();

        // GET: User
        public ActionResult Login()
        {
            if (Session["User"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]  
        
        public ActionResult LoginSubmit(FormCollection collect)
        {
            var email = collect["Email"];
            var pass = collect["MatKhau"];
            KhachHang kh = db.KhachHangs.FirstOrDefault(x => x.Email == email && x.MatKhau == pass);
            QuanLy ql = db.QuanLies.FirstOrDefault(x => x.Email == email && x.MatKhau == pass);
            if (ql != null)
            {
                Session["Admin"] = ql;
                return RedirectToAction("Index", "Admin");
            }
            if (kh != null)
            {
                Session["User"] = kh;
                return RedirectToAction("Index", "Home");

            }
            ViewBag.Error = "Thông tin đăng nhập không chính xác";
            return View("Login");
        }

        public ActionResult Register()
        {
            if (Session["User"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public ActionResult RegisterSubmit(FormCollection collect)
        {
            var hoten = collect["HoTen"];
            var sdt = collect["SDT"];
            var email = collect["Email"];
            var pass = collect["MatKhau"];
            var repass = collect["ReMatKhau"];
            var diachi = collect["DiaChi"];

            // Lưu lại dữ liệu đã nhập
            ViewBag.HoTen = hoten;
            ViewBag.SDT = sdt;
            ViewBag.Email = email;
            ViewBag.DiaChi = diachi;

            // Kiểm tra mật khẩu nhập lại
            if (pass != repass)
            {
                ViewBag.Error = "Mật khẩu nhập lại không trùng khớp!";
                return View("Register");
            }

            // Kiểm tra email tồn tại
            if (db.KhachHangs.Any(x => x.Email == email))
            {
                ViewBag.Error = "Email đã tồn tại!";
                return View("Register");
            }

            // Kiểm tra SĐT tồn tại
            if (db.KhachHangs.Any(x => x.SDT == sdt))
            {
                ViewBag.Error = "Số điện thoại đã tồn tại!";
                return View("Register");
            }

            KhachHang kh = new KhachHang()
            {
                HoTen = hoten,
                SDT = sdt,
                Email = email,
                MatKhau = pass,
                DiaChi = diachi
            };

            db.KhachHangs.Add(kh);
            db.SaveChanges();

            Session["User"] = kh;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            Session["Admin"] = null;
            return RedirectToAction("Login", "User");
        }
    }
}