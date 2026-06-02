using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetShop.Controllers
{
    namespace PetShop.Controllers
    {
        public class NewsController : Controller
        {
            SHOPBANTHUCUNGEntities db = new SHOPBANTHUCUNGEntities();

            // GET: Danh sách tin tức
            public ActionResult Index()
            {
                // Lấy toàn bộ tin tức, sắp xếp tin mới nhất lên đầu
                var listTinTuc = db.TinTucs.OrderByDescending(x => x.NgayDang).ToList();
                return View(listTinTuc);
            }

            // GET: Chi tiết một bài tin
            //public ActionResult Details(int id)
            //{
            //    var tin = db.TinTucs.Find(id);
            //    if (tin == null)
            //    {
            //        return HttpNotFound();
            //    }
            //    return View(tin);
            //}
        }
    }
}