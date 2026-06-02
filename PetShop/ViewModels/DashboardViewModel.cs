using System;
using System.Collections.Generic;

namespace PetShop.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        
        // Recent orders
        public List<RecentOrderViewModel> RecentOrders { get; set; }
        
        // Top selling products
        public List<TopProductViewModel> TopProducts { get; set; }
        
        // Monthly revenue data for chart
        public List<MonthlyRevenueData> MonthlyRevenueChart { get; set; }
        
        public DashboardViewModel()
        {
            RecentOrders = new List<RecentOrderViewModel>();
            TopProducts = new List<TopProductViewModel>();
            MonthlyRevenueChart = new List<MonthlyRevenueData>();
        }
    }
    
    public class RecentOrderViewModel
    {
        public int MaHD { get; set; }
        public DateTime NgayLap { get; set; }
        public string KhachHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
    
    public class TopProductViewModel
    {
        public int MaThuCung { get; set; }
        public string TenThuCung { get; set; }
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }
    
    public class MonthlyRevenueData
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }
}
