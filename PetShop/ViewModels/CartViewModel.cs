using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetShop.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; }

        public decimal TotalAmount
        {
            get { return Items?.Sum(i => i.ThanhTien) ?? 0; }
        }

        public int TotalQuantity
        {
            get { return Items?.Sum(i => i.SoLuong) ?? 0; }
        }

        public CartViewModel()
        {
            Items = new List<CartItemViewModel>();
        }
    }
}