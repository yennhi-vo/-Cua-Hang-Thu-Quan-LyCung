using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetShop.Models;
namespace PetShop.Models
{
    public class KetQuaTimKiemVM
    {
        public List<ThuCung> ThuCungs { get; set; }
        public List<PhuKien> PhuKiens { get; set; }
        public List<DichVu> DichVus { get; set; }
    }
}