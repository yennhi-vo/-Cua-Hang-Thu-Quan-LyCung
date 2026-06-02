
using PetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetShop.Controllers
{
    public class HomeController : Controller

    {
        SHOPBANTHUCUNGEntities data = new SHOPBANTHUCUNGEntities();

        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                Banners = data.BANNERs.ToList(),
                Bannerchinhes=data.BANNERCHINHs.ToList(),
                LoaiThuCungs = data.LoaiThuCungs.ToList(),
                LoaiDichVus = data.LoaiDichVus.ToList(),
                ChamSocSucKhoes = data.ChamSocSucKhoes.ToList(),
                LoaiPhuKiens = data.LoaiPhuKiens.ToList()
            };
            return View(model);
        }
        public ActionResult About()
        {
            return View();
        }





    }
}