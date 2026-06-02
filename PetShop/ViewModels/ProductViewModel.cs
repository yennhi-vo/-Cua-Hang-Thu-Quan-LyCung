using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PetShop.ViewModels
{
    public class ProductViewModel
    {
        public int MaThuCung { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên thú cưng")]
        [Display(Name = "Tên thú cưng")]
        public string TenThuCung { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn loại")]
        [Display(Name = "Loại")]
        public int MaLoai { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }
        
        [Display(Name = "Màu sắc")]
        public string MauSac { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Display(Name = "Giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Gia { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }
        
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; }
        
        [Display(Name = "Hình ảnh")]
        public string Hinh { get; set; }
        
        [Display(Name = "Upload hình")]
        public HttpPostedFileBase ImageUpload { get; set; }
        
        // For display purposes
        public string TenLoai { get; set; }
    }
}
