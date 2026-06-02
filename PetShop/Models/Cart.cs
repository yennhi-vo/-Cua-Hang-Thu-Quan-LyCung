using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetShop.Models
{
    public class Cart
    {
        
        public List<CartItem> cart;
        public Cart()
        {
            cart = new List<CartItem>();
        }
        public Cart(List<CartItem> list)
        {
            cart = list;
        }
        public int SoLuongMH()
        {
            return cart.Count;
        }
        public int TongSoLuongSP()
        {
            return cart.Sum(x => x.SoLuong);
        }
        public decimal TongThanhTien()
        {
            return cart.Sum(x => x.ThanhTien);
        }
        //public int Them(int id)
        //{
        //    CartItem sp = cart.Find(x => x.MaThuCung == id);
        //    try
        //    {
        //        if (sp != null)//đã tồn tại trong giỏ hàng
        //        {
        //            sp.SoLuong++;
        //        }
        //        else
        //        {
        //            CartItem newItem = new CartItem(id);
        //            cart.Add(newItem);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }

        //    return 1; //thêm thành công
        //}
        public int Them(int id, string loai)
        {
            // Tìm xem trong giỏ đã có SP này (cùng mã và cùng loại) chưa
            CartItem sp = cart.Find(x => x.MaSP == id && x.Loai == loai);
            try
            {
                if (sp != null)
                {
                    sp.SoLuong++;
                }
                else
                {
                    CartItem newItem = new CartItem(id, loai);
                    cart.Add(newItem);
                }
            }
            catch { return -1; }
            return 1;
        }
        public int Xoa(int id, string loai)
        {
            // Tìm chính xác sản phẩm dựa trên ID và Loại
            CartItem sp = cart.Find(x => x.MaSP == id && x.Loai == loai);
            try
            {
                if (sp != null)
                {
                    cart.Remove(sp);
                    return 1; // Xóa thành công
                }
                return -1; // Không tìm thấy sản phẩm
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Giam(int id, string loai)
        {
            // Tìm chính xác sản phẩm dựa trên ID và Loại
            CartItem sp = cart.Find(x => x.MaSP == id && x.Loai == loai);
            try
            {
                if (sp != null)
                {
                    sp.SoLuong--;
                    if (sp.SoLuong <= 0)
                    {
                        cart.Remove(sp);
                    }
                    return 1; // Giảm thành công
                }
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public double PhanTramGiam;
        public double TinhGiamGia()
        {
            return (double)TongThanhTien() * (PhanTramGiam / 100);
        }
        public double TongPhaiThanhToan()
        {
            return (double)TongThanhTien() - TinhGiamGia();
        }

    }
}