
using PetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PetShop;


namespace PetShop.Controllers
{
    public class ProductController : Controller
    {
        SHOPBANTHUCUNGEntities data = new SHOPBANTHUCUNGEntities();


        public ActionResult Index(int? id, string sort)
        {
            // 1. Lấy toàn bộ thú cưng
            var list = data.ThuCungs.AsQueryable();

            // 2. Nếu có id (mã loại), lọc theo loại trước
            if (id.HasValue)
            {
                list = list.Where(x => x.MaLoai == id.Value);
            }

            // 3. Sau đó mới thực hiện sắp xếp trên danh sách đã lọc (hoặc toàn bộ)
            if (!String.IsNullOrEmpty(sort))
            {
                if (sort == "0") // Tăng dần
                {
                    list = list.OrderBy(x => x.Gia);
                }
                else if (sort == "1") // Giảm dần
                {
                    list = list.OrderByDescending(x => x.Gia);
                }
            }
            else
            {
                list = list.OrderByDescending(x => x.MaThuCung); // Mặc định
            }

            // 4. Lưu lại ID loại và kiểu Sort để giữ trạng thái trên giao diện
            ViewBag.CurrentLoai = id;
            ViewBag.CurrentSort = sort;

            return View(list.ToList());
        }

        public ActionResult DichVu(int? id, string sort)
        {
            var list = data.DichVus.AsQueryable();
            if (id.HasValue) list = list.Where(x => x.MaLoai == id);

            if (sort == "0") list = list.OrderBy(x => x.Gia);
            else if (sort == "1") list = list.OrderByDescending(x => x.Gia);

            ViewBag.CurrentLoai = id;
            ViewBag.CurrentSort = sort;
            ViewBag.Title = "Danh sách dịch vụ";
            return View(list.ToList()); // Trả về View DichVu.cshtml
        }
        // Chi tiết thú cưng
        public ActionResult DetailDV(int id)
        {
            var dichVu = data.DichVus.FirstOrDefault(x => x.MaDichVu == id);
            if (dichVu == null)
                return HttpNotFound();

            // Lấy danh sách DICH VU cùng loại
            var dsLienQuanDV = data.DichVus
                .Where(x => x.MaLoai == dichVu.MaLoai && x.MaDichVu != dichVu.MaDichVu)
                .ToList();

            ViewBag.SPLienQuanDV = dsLienQuanDV;
            return View(dichVu);
        }
        public ActionResult PhuKien(int? id, string sort)
        {
            var list = data.PhuKiens.AsQueryable();

            // Sửa logic lọc: Đảm bảo so sánh đúng kiểu dữ liệu
            if (id.HasValue && id > 0)
            {
                list = list.Where(x => x.MaLoai == id.Value);
            }

            // Logic sắp xếp giữ nguyên
            if (sort == "0") list = list.OrderBy(x => x.Gia);
            else if (sort == "1") list = list.OrderByDescending(x => x.Gia);

            ViewBag.CurrentLoai = id; // Lưu lại để giữ trạng thái cho Hidden Input
            ViewBag.CurrentSort = sort;
            ViewBag.Title = "Danh sách phụ kiện";

            return View(list.ToList());
        }
        public ActionResult DetailPK(int id)
        {
            var phuKien = data.PhuKiens
          .Include("HinhAnhPhus") // Giả sử tên bảng ảnh phụ của PK là HinhAnhPhuPKs
          .Include("BinhLuans.KhachHang") // Giả sử tên bảng bình luận của PK là BinhLuanPKs
          .FirstOrDefault(x => x.MaPhuKien == id);

            if (phuKien == null)
                return HttpNotFound();

            // Lấy danh sách thú cùng loại
            var dsLienQuanPK = data.PhuKiens
                .Where(x => x.MaLoai == phuKien.MaLoai && x.MaPhuKien != phuKien.MaPhuKien)
                .ToList();

            ViewBag.SPLienQuanPK = dsLienQuanPK;
            return View(phuKien);
        }
        // Chi tiết thú cưng
        public ActionResult Detail(int id)
        {
            var thuCung = data.ThuCungs
                    .Include(p => p.BinhLuans.Select(b => b.KhachHang))
                    .Include(p => p.HinhAnhPhus)
                    .FirstOrDefault(p => p.MaThuCung == id);
           
            if (thuCung == null)
                return HttpNotFound();

            // Lấy danh sách thú cùng loại
            var dsLienQuanTC = data.ThuCungs
                .Where(x => x.MaLoai == thuCung.MaLoai && x.MaThuCung != thuCung.MaThuCung)
                .ToList();

            ViewBag.SPLienQuanTC = dsLienQuanTC;
            return View(thuCung);
        }
        public ActionResult _ThuCung()
        {
            return PartialView(data.LoaiThuCungs.ToList());
        }
        public ActionResult _Dichvu()
        {
            return PartialView(data.LoaiDichVus.ToList());
        }
        public ActionResult _PhuKien()
        {
            return PartialView(data.LoaiPhuKiens.ToList());
        }
        // Lọc theo loại thú cưng 
        public ActionResult TimTheoLoai(int id)
            {
                var ds = data.ThuCungs.Where(x => x.MaLoai == id).ToList();
                return View("Index", ds);
            }

            // Lọc theo loại dịch vụ 
            public ActionResult TimTheoLoaiDichVu(int id)
            {
                var ds = data.DichVus.Where(x => x.MaLoai == id).ToList();
                return View("DichVu", ds);
            }

            // Lọc theo loại phụ kiện
            public ActionResult TimTheoLoaiPhuKien(int id)
            {
                var ds = data.PhuKiens.Where(x => x.MaLoai == id).ToList();
                return View("PhuKien", ds); 
            }




        // Tìm theo từ khóa cho thú cưng
        public ActionResult TimTheoTuKhoa(string keyword)
        {
            var ds = data.ThuCungs
                          .Where(x => x.TenThuCung.Contains(keyword))
                          .ToList();
            return View("Index", ds);
        }

        // Tìm theo từ khóa cho dịch vụ
        public ActionResult TimTheoTuKhoaDichVu(string keyword)
        {
            var ds = data.DichVus
                          .Where(x => x.TenDichVu.Contains(keyword))
                          .ToList();
            return View("DichVu", ds);
        }

        // Tìm theo từ khóa cho phụ kiện
        public ActionResult TimTheoTuKhoaPhuKien(string keyword)
        {
            var ds = data.PhuKiens
                          .Where(x => x.TenPhuKien.Contains(keyword))
                          .ToList();
            return View("PhuKien", ds);
        }


        public ActionResult TimKiem(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = "";
                return View("KetQuaTimKiem");
            }

            keyword = keyword.ToLower();

            var thuCung = data.ThuCungs
                              .Where(x => x.TenThuCung.ToLower().Contains(keyword))
                              .ToList();

            var phuKien = data.PhuKiens
                              .Where(x => x.TenPhuKien.ToLower().Contains(keyword))
                              .ToList();

            var dichVu = data.DichVus
                              .Where(x => x.TenDichVu.ToLower().Contains(keyword))
                              .ToList();

            ViewBag.Keyword = keyword;

            var model = new KetQuaTimKiemVM
            {
                ThuCungs = thuCung,
                PhuKiens = phuKien,
                DichVus = dichVu
            };

            return View("KetQuaTimKiem", model);
        }

        public ActionResult KetQuaTimKiem()
        {

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GuiBinhLuan(int id, string loai, string noiDung)
        {
            // Ép kiểu rõ ràng về class KhachHang từ file bạn vừa gửi
            var kh = Session["User"] as KhachHang;

            if (kh == null)
            {
                // Nếu chưa đăng nhập, trả về trang đăng nhập
                return RedirectToAction("DangNhap", "User");
            }

            if (!string.IsNullOrEmpty(noiDung))
            {
                if (loai == "ThuCung")
                {
                    var bl = new BinhLuan();
                    bl.MaThuCung = id;
                    bl.MaKH = kh.MaKH; // Bây giờ mã sẽ hết lỗi đỏ
                    bl.NoiDung = noiDung;
                    bl.NgayDang = DateTime.Now;

                    data.BinhLuans.Add(bl);
                }
                else if (loai == "PhuKien")
                {
                    // Trong EF của bạn, nếu bảng bình luận phụ kiện là BinhLuanPK
                    // Hãy đảm bảo bạn khởi tạo đúng lớp đó
                    var blpk = new BinhLuan(); // Hoặc BinhLuanPK tùy DB của bạn
                    blpk.MaPhuKien = id;
                    blpk.MaKH = kh.MaKH;
                    blpk.NoiDung = noiDung;
                    blpk.NgayDang = DateTime.Now;

                    data.BinhLuans.Add(blpk);
                }

                data.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

    }


}
