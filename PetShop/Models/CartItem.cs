using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetShop.Models
{
    public class CartItem
    {
        public int MaSP { get; set; } 
        public string TenSP { get; set; }
        public string Hinh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string Loai { get; set; } 

        public decimal ThanhTien => SoLuong * Gia;

        SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();

        // Constructor mới nhận thêm tham số 'loai'
        public CartItem(int id, string loai)
        {
            this.Loai = loai;
            if (loai == "ThuCung")
            {
                var sp = db.ThuCungs.FirstOrDefault(x => x.MaThuCung == id);
                if (sp != null)
                {
                    MaSP = sp.MaThuCung;
                    TenSP = sp.TenThuCung;
                    Hinh = sp.Hinh;
                    Gia = sp.Gia ?? 0;
                }
            }
            else if (loai == "PhuKien")
            {
                var sp = db.PhuKiens.FirstOrDefault(x => x.MaPhuKien == id);
                if (sp != null)
                {
                    MaSP = sp.MaPhuKien;
                    TenSP = sp.TenPhuKien;
                    Hinh = sp.Hinh;
                    Gia = sp.Gia ?? 0;
                }
            }
          
            this.SoLuong = 1;
        }
    }

}
