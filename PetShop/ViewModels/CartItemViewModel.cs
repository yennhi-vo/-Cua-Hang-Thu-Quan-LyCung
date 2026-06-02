using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetShop.ViewModels
{
    public class CartItemViewModel
    {
        public int MaThuCung { get; set; }
        public string TenThuCung { get; set; }
        public string Hinh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        public decimal ThanhTien
        {
            get { return Gia * SoLuong; }
        }
    }
}