using PetShop.ViewModels;
using PetShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace PetShop.Controllers
{
    public class AdminController : Controller
    {
        private SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();

        //--- 
        // GET: Admin Dashboard
        public ActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalProducts = db.ThuCungs.Count(),
                TotalOrders = db.HoaDons.Count(),
                TotalCustomers = db.KhachHangs.Count(),
                NewCustomersThisMonth = db.KhachHangs.Count(), // Can be refined with date filter
                MonthlyRevenue = db.HoaDons
                .Where(h => h.TinhTrang == 4 && // Chỉ lấy đơn hoàn thành
                h.NgayLap.Value.Month == DateTime.Now.Month &&
                h.NgayLap.Value.Year == DateTime.Now.Year)
                 .Sum(h => (decimal?)h.TongTien) ?? 0,
                TotalRevenue = db.HoaDons
                .Where(h => h.TinhTrang == 4) 
                .Sum(h => (decimal?)h.TongTien) ?? 0
            };

            // Recent orders
            model.RecentOrders = db.HoaDons
                .OrderByDescending(h => h.NgayLap)
                .Take(5)
                .Select(h => new RecentOrderViewModel
                {
                    MaHD = h.MaHD,
                    NgayLap = h.NgayLap ?? DateTime.Now,
                    KhachHang = h.KhachHang.HoTen,
                    TongTien = h.TongTien ?? 0,
                    TrangThai = "Hoàn thành" // Will use actual status when table updated
                })
                .ToList();

            // Top products
            model.TopProducts = db.ChiTietHoaDons
                .GroupBy(ct => new { ct.MaThuCung, ct.ThuCung.TenThuCung })
                .Select(g => new TopProductViewModel
                {
                    MaThuCung = g.Key.MaThuCung,
                    TenThuCung = g.Key.TenThuCung,
                    SoLuongBan = g.Sum(x => x.SoLuong ?? 0),
                    DoanhThu = g.Sum(x => (x.SoLuong ?? 0) * (x.DonGia ?? 0))
                })
                .OrderByDescending(p => p.DoanhThu)
                .Take(5)
                .ToList();

            // Monthly revenue for last 6 months
            var sixMonthsAgo = DateTime.Now.AddMonths(-5);
            var rawRevenueData = db.HoaDons
     .Where(h => h.NgayLap >= sixMonthsAgo && h.TinhTrang == 4) 
     .GroupBy(h => new { h.NgayLap.Value.Year, h.NgayLap.Value.Month })
                 
                 .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(h => h.TongTien ?? 0)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            model.MonthlyRevenueChart = rawRevenueData.Select(x => new MonthlyRevenueData
            {
                Month = x.Month + "/" + x.Year,
                Revenue = x.Revenue
            }).ToList();

            return View(model);
        }

        #region Product Management

        // GET: Admin/Products
        public ActionResult Products(string search, int? category)
        {
            var products = db.ThuCungs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.TenThuCung.Contains(search));
            }

            if (category.HasValue)
            {
                products = products.Where(p => p.MaLoai == category.Value);
            }

            ViewBag.Categories = db.LoaiThuCungs.ToList();
            ViewBag.Search = search;
            ViewBag.Category = category;

            return View(products.Include(p => p.LoaiThuCung).OrderByDescending(p => p.MaThuCung).ToList());
        }

        // GET: Admin/CreateProduct
        public ActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(db.LoaiThuCungs, "MaLoai", "TenLoai");
            return View(new ProductViewModel { TrangThai = "Còn hàng" });
        }

        // POST: Admin/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new ThuCung
                {
                    TenThuCung = model.TenThuCung,
                    MaLoai = model.MaLoai,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh,
                    MauSac = model.MauSac,
                    Gia = model.Gia,
                    MoTa = model.MoTa,
                    TrangThai = model.TrangThai
                };

                // Handle image upload
                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.ImageUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Products"), fileName);

                    // Create directory if not exists
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Products"));

                    model.ImageUpload.SaveAs(path);
                    product.Hinh = fileName;
                }

                db.ThuCungs.Add(product);
                db.SaveChanges();

                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Products");
            }

            ViewBag.Categories = new SelectList(db.LoaiThuCungs, "MaLoai", "TenLoai", model.MaLoai);
            return View(model);
        }

        // GET: Admin/EditProduct/5
        public ActionResult EditProduct(int id)
        {
            var product = db.ThuCungs.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = new ProductViewModel
            {
                MaThuCung = product.MaThuCung,
                TenThuCung = product.TenThuCung,
                MaLoai = product.MaLoai ?? 0,
                GioiTinh = product.GioiTinh,
                NgaySinh = product.NgaySinh,
                MauSac = product.MauSac,
                Gia = product.Gia ?? 0,
                MoTa = product.MoTa,
                TrangThai = product.TrangThai,
                Hinh = product.Hinh
            };

            ViewBag.Categories = new SelectList(db.LoaiThuCungs, "MaLoai", "TenLoai", model.MaLoai);
            return View(model);
        }

        // POST: Admin/EditProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = db.ThuCungs.Find(model.MaThuCung);
                if (product == null)
                {
                    return HttpNotFound();
                }

                product.TenThuCung = model.TenThuCung;
                product.MaLoai = model.MaLoai;
                product.GioiTinh = model.GioiTinh;
                product.NgaySinh = model.NgaySinh;
                product.MauSac = model.MauSac;
                product.Gia = model.Gia;
                product.MoTa = model.MoTa;
                product.TrangThai = model.TrangThai;

                // Handle image upload
                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.ImageUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Products"), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Products"));

                    model.ImageUpload.SaveAs(path);
                    product.Hinh = fileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Products");
            }

            ViewBag.Categories = new SelectList(db.LoaiThuCungs, "MaLoai", "TenLoai", model.MaLoai);
            return View(model);
        }

        // POST: Admin/DeleteProduct/5
        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var product = db.ThuCungs.Find(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Check if product is in any order
                var hasOrders = db.ChiTietHoaDons.Any(ct => ct.MaThuCung == id);
                if (hasOrders)
                {
                    return Json(new { success = false, message = "Không thể xóa sản phẩm đã có trong đơn hàng" });
                }

                db.ThuCungs.Remove(product);
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        #endregion

        #region Order Management

        // GET: Admin/Orders
        public ActionResult Orders(string status)
        {
            var orders = db.HoaDons.Include(h => h.KhachHang).Include(h => h.NhanVien).AsQueryable();

            // Note: TrangThai column needs to be added to HoaDon table
            // if (!string.IsNullOrEmpty(status))
            // {
            //     orders = orders.Where(o => o.TrangThai == status);
            // }

            ViewBag.Status = status;
            return View(orders.OrderByDescending(o => o.NgayLap).ToList());
        }

        // GET: Admin/OrderDetail/5
        public ActionResult OrderDetail(int id)
        {
            var order = db.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .Include(h => h.ChiTietHoaDons.Select(ct => ct.ThuCung))
                .FirstOrDefault(h => h.MaHD == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        public ActionResult UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                var order = db.HoaDons.Find(orderId);
                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                // Note: This requires TrangThai column in HoaDon table
                // order.TrangThai = status;
                // db.Entry(order).State = EntityState.Modified;
                // db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        #endregion

        #region Revenue & Reports

        // GET: Admin/Revenue
        public ActionResult Revenue(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddMonths(-1);

            if (!endDate.HasValue)
                endDate = DateTime.Now;

            var orders = db.HoaDons
          .Where(h => h.NgayLap >= startDate && h.NgayLap <= endDate && h.TinhTrang == 4) // Thêm điều kiện ở đây
          .Include(h => h.ChiTietHoaDons.Select(ct => ct.ThuCung.LoaiThuCung))
          .ToList();

            var model = new RevenueReportViewModel
            {
                StartDate = startDate.Value,
                EndDate = endDate.Value,
                TotalRevenue = orders.Sum(o => o.TongTien ?? 0),
                TotalOrders = orders.Count(),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TongTien ?? 0) : 0
            };

            // Revenue by category
            model.RevenueByCategory = orders
                .SelectMany(h => h.ChiTietHoaDons)
                .GroupBy(ct => ct.ThuCung.LoaiThuCung.TenLoai)
                .Select(g => new CategoryRevenueData
                {
                    TenLoai = g.Key,
                    DoanhThu = g.Sum(ct => (ct.SoLuong ?? 0) * (ct.DonGia ?? 0)),
                    SoLuong = g.Sum(ct => ct.SoLuong ?? 0)
                })
                .OrderByDescending(c => c.DoanhThu)
                .ToList();

            // Daily revenue
            model.DailyRevenue = orders
                .GroupBy(h => h.NgayLap.Value.Date)
                .Select(g => new DailyRevenueData
                {
                    Ngay = g.Key,
                    DoanhThu = g.Sum(h => h.TongTien ?? 0),
                    SoDonHang = g.Count()
                })
                .OrderBy(d => d.Ngay)
                .ToList();

            // Top products
            model.TopProducts = orders
                .SelectMany(h => h.ChiTietHoaDons)
                .GroupBy(ct => new { ct.MaThuCung, ct.ThuCung.TenThuCung })
                .Select(g => new TopProductViewModel
                {
                    MaThuCung = g.Key.MaThuCung,
                    TenThuCung = g.Key.TenThuCung,
                    SoLuongBan = g.Sum(ct => ct.SoLuong ?? 0),
                    DoanhThu = g.Sum(ct => (ct.SoLuong ?? 0) * (ct.DonGia ?? 0))
                })
                .OrderByDescending(p => p.DoanhThu)
                .Take(10)
                .ToList();

            return View(model);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Cập nhập tình trạng dơnd hàng
        // Trong Controllers/AdminController.cs

        [HttpPost]
        public ActionResult CapNhatTrangThai(int maHD, int maTinhTrang)
        {
            // 1. Tìm hóa đơn trong DB
            var hoadon = db.HoaDons.Find(maHD);

            if (hoadon != null)
            {
                // 2. Cập nhật mã tình trạng mới
                hoadon.TinhTrang = maTinhTrang;

                // 3. Lưu thay đổi
                db.SaveChanges();

                // 4. Tạo thông báo thành công (hiển thị sau khi load lại trang)
                TempData["Message"] = "Cập nhật đơn hàng #" + maHD + " thành công!";
            }

            // 5. Quay trở về trang danh sách đơn hàng
            return RedirectToAction("Orders");
        }
        public ActionResult DanhSachTinTuc()
        {
            var news = db.TinTucs.OrderByDescending(x => x.NgayDang).ToList();
            return View(news);
        }
        public ActionResult ThemTinTuc()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)] // Quan trọng: Cho phép nhận mã HTML từ CKEditor
        public ActionResult ThemTinTuc(TinTuc news, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // Xử lý lưu hình ảnh
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(uploadImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/news/"), fileName);
                    uploadImage.SaveAs(path);
                    news.HinhTinTuc = fileName;
                }

                news.NgayDang = DateTime.Now;
                db.TinTucs.Add(news);
                db.SaveChanges();
                return RedirectToAction("DanhSachTinTuc");
            }
            return View(news);
        }

        // Xóa tin tức
        public ActionResult XoaTinTuc(int id)
        {
            var item = db.TinTucs.Find(id);
            if (item != null)
            {
                db.TinTucs.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachTinTuc");
        }
        // 1. Hiển thị danh sách các mã giảm giá
        public ActionResult DanhSachKhuyenMai()
        {
            var list = db.KhuyenMais.OrderByDescending(x => x.NgayKetThuc).ToList();
            return View(list);
        }

        // 2. Giao diện thêm mới mã giảm giá
        public ActionResult ThemKhuyenMai()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemKhuyenMai(KhuyenMai km)
        {
            if (ModelState.IsValid)
            {
                // Bạn có thể thêm mô tả hoặc hình ảnh nếu bảng có các cột đó
                db.KhuyenMais.Add(km);
                db.SaveChanges();
                TempData["Message"] = "Thêm mã khuyến mãi thành công!";
                return RedirectToAction("DanhSachKhuyenMai");
            }
            return View(km);
        }

        // 3. Xóa mã khuyến mãi
        public ActionResult XoaKhuyenMai(int id)
        {
            var km = db.KhuyenMais.Find(id);
            if (km != null)
            {
                db.KhuyenMais.Remove(km);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachKhuyenMai");
        }

		// --- QUẢN LÝ PHỤ KIỆN ---
		// --- QUẢN LÝ PHỤ KIỆN ---
		public ActionResult DanhSachPhuKien(string search, int? category)
		{
			// Sử dụng Include để tải kèm thông tin Loại Phụ Kiện (Eager Loading)
			var list = db.PhuKiens.Include(p => p.LoaiPhuKien).AsQueryable();

			// 1. Lọc theo từ khóa tìm kiếm
			if (!string.IsNullOrEmpty(search))
			{
				list = list.Where(p => p.TenPhuKien.Contains(search));
			}

			// 2. Lọc theo danh mục (Loại phụ kiện)
			if (category.HasValue)
			{
				list = list.Where(p => p.MaLoai == category.Value);
			}

			// Đổ dữ liệu ra ViewBag để giữ trạng thái trên giao diện
			ViewBag.Categories = db.LoaiPhuKiens.ToList();
			ViewBag.Search = search;
			ViewBag.Category = category;

			return View(list.OrderByDescending(p => p.MaPhuKien).ToList());
		}

		public ActionResult XoaPhuKien(int id)
        {
            var pk = db.PhuKiens.Find(id);
            if (pk != null)
            {
                db.PhuKiens.Remove(pk);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachPhuKien");
        }

        // --- THÊM PHỤ KIỆN ---
        [HttpGet]
        public ActionResult ThemPhuKien()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
      
        public ActionResult ThemPhuKien(PhuKien pk, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // 1. Xử lý upload ảnh
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    // Lấy tên file gốc
                    string fileName = Path.GetFileName(uploadImage.FileName);

                    // Đường dẫn thư mục lưu (Đảm bảo có dấu gạch chéo ở cuối)
                    string folderPath = Server.MapPath("~/Content/Images/Products/");

                    // Nếu thư mục chưa tồn tại thì tạo mới để tránh lỗi
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string path = Path.Combine(folderPath, fileName);

                    // Lưu file vật lý vào server
                    uploadImage.SaveAs(path);

                    // QUAN TRỌNG: Gán tên file vào cột Hinh của Model
                    pk.Hinh = fileName;
                }

                // 2. Lưu vào Database
                db.PhuKiens.Add(pk);
                db.SaveChanges();

                TempData["Message"] = "Thêm phụ kiện mới thành công!";
                return RedirectToAction("DanhSachPhuKien");
            }
            return View(pk);
        }
		// --- QUẢN LÝ DỊCH VỤ ---
		// --- QUẢN LÝ DỊCH VỤ ---
		public ActionResult DanhSachDichVu(string search, int? category)
		{
			// Bắt buộc dùng .Include(d => d.LoaiDichVu) để lấy tên loại dịch vụ, tránh lỗi NullReferenceException
			var list = db.DichVus.Include(d => d.LoaiDichVu).AsQueryable();

			// 1. Lọc theo tên dịch vụ
			if (!string.IsNullOrEmpty(search))
			{
				list = list.Where(d => d.TenDichVu.Contains(search));
			}

			// 2. Lọc theo loại dịch vụ
			if (category.HasValue)
			{
				list = list.Where(d => d.MaLoai == category.Value);
			}

			// Gửi dữ liệu qua ViewBag để hiển thị trên Form tìm kiếm
			ViewBag.Categories = db.LoaiDichVus.ToList();
			ViewBag.Search = search;
			ViewBag.Category = category;

			return View(list.OrderByDescending(d => d.MaDichVu).ToList());
		}
		[HttpGet]
        public ActionResult ThemDichVu()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemDichVu(DichVu dv, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // Nếu dịch vụ có hình ảnh minh họa
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(uploadImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/dichvu/"), fileName);
                    uploadImage.SaveAs(path);
                    dv.Hinh = fileName;
                }

                db.DichVus.Add(dv);
                db.SaveChanges();
                TempData["Message"] = "Thêm dịch vụ mới thành công!";
                return RedirectToAction("DanhSachDichVu");
            }
            return View(dv);
        }
        public ActionResult XoaDichVu(int id)
        {
            var dv = db.DichVus.Find(id);
            if (dv != null)
            {
                db.DichVus.Remove(dv);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachDichVu");
        }
        // --- SỬA PHỤ KIỆN ---
        [HttpGet]
        public ActionResult SuaPhuKien(int id)
        {
            var pk = db.PhuKiens.Find(id);
            if (pk == null) return HttpNotFound();
            return View(pk);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaPhuKien(PhuKien pk, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var item = db.PhuKiens.Find(pk.MaPhuKien);
                if (item != null)
                {
                    item.TenPhuKien = pk.TenPhuKien;
                    item.Gia = pk.Gia;
                    item.GiaKhuyenMai = pk.GiaKhuyenMai;
                    item.MoTa = pk.MoTa;

                    if (uploadImage != null && uploadImage.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(uploadImage.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName);
                        uploadImage.SaveAs(path);
                        item.Hinh = fileName;
                    }
                    db.SaveChanges();
                    return RedirectToAction("DanhSachPhuKien");
                }
            }
            return View(pk);
        }

        // --- SỬA DỊCH VỤ ---
        [HttpGet]
        public ActionResult SuaDichVu(int id)
        {
            var dv = db.DichVus.Find(id);
            if (dv == null) return HttpNotFound();
            return View(dv);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaDichVu(DichVu dv, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var item = db.DichVus.Find(dv.MaDichVu); // Kiểm tra lại MaDV hay MaDichVu trong DB của bạn
                if (item != null)
                {
                    item.TenDichVu = dv.TenDichVu;
                    item.Gia = dv.Gia;
                    item.GiaKhuyenMai = dv.GiaKhuyenMai;
                    item.MoTa = dv.MoTa;

                    if (uploadImage != null && uploadImage.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(uploadImage.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/Images/dichvu/"), fileName);
                        uploadImage.SaveAs(path);
                        item.Hinh = fileName;
                    }
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDichVu");
                }
            }
            return View(dv);
        }

        //======================QUAN LY LICH HEN ===========================



        // Action xem danh sách lịch hẹn
        public ActionResult QuanLyLichHen()
        {
            var list = db.DatLiches.OrderByDescending(x => x.NgayHen).ToList();
            return View(list);
        }
        // 2. Giao diện thêm mới lịch hẹn
        [HttpGet]
        public ActionResult ThemMoilichHen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoilichHen(DatLich lh)
        {
            if (ModelState.IsValid)
            {
                // Mặc định tình trạng khi thêm mới
                if (string.IsNullOrEmpty(lh.TinhTrang)) lh.TinhTrang = "Xác nhận";

                db.DatLiches.Add(lh);
                db.SaveChanges();
                TempData["Message"] = "Thêm lịch hẹn thành công!";
                return RedirectToAction("QuanLyLichHen");
            }
            return View(lh);
        }

        // 3. Giao diện sửa lịch hẹn
        [HttpGet]
        public ActionResult SuaLichHen(int id)
        {
            var lh = db.DatLiches.Find(id);
            if (lh == null) return HttpNotFound();
            return View(lh);
        }

        [HttpPost]
        public ActionResult SuaLichHen(DatLich lh)
        {
            if (ModelState.IsValid)
            {
                var item = db.DatLiches.Find(lh.MaDatLich);
                if (item != null)
                {
                    item.HoTenKhach = lh.HoTenKhach;
                    item.SDTKhach = lh.SDTKhach;
                    item.NgayHen = lh.NgayHen;
                    item.GhiChu = lh.GhiChu;
                    item.TinhTrang = lh.TinhTrang; // Thay đổi tình trạng ở đây

                    db.SaveChanges();
                    TempData["Message"] = "Cập nhật lịch hẹn thành công!";
                    return RedirectToAction("QuanLyLichHen");
                }
            }
            return View(lh);
        }

        // 4. Xóa lịch hẹn
        public ActionResult XoaLichHen(int id)
        {
            var lh = db.DatLiches.Find(id);
            if (lh != null)
            {
                db.DatLiches.Remove(lh);
                db.SaveChanges();
                TempData["Message"] = "Xóa lịch hẹn thành công!";
            }
            return RedirectToAction("QuanLyLichHen");
        }
    }
}