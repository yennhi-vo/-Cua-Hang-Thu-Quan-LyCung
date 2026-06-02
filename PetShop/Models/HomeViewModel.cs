using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetShop.Models;
namespace PetShop.Models
{
    public class HomeViewModel
    {
        public List<BANNER> Banners { get; set; }
        public List<BANNERCHINH> Bannerchinhes { get; set; }
        public List<LoaiThuCung> LoaiThuCungs { get; set; }
        public List<LoaiDichVu> LoaiDichVus { get; set; }
        public List<ChamSocSucKhoe> ChamSocSucKhoes { get; set; }
        public List<LoaiPhuKien> LoaiPhuKiens { get; set; }
    }
}