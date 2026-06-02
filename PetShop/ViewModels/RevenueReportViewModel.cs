using System;
using System.Collections.Generic;

namespace PetShop.ViewModels
{
    public class RevenueReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        
        // Revenue by category
        public List<CategoryRevenueData> RevenueByCategory { get; set; }
        
        // Daily revenue for chart
        public List<DailyRevenueData> DailyRevenue { get; set; }
        
        // Top selling products
        public List<TopProductViewModel> TopProducts { get; set; }
        
        // Payment methods breakdown
        public List<PaymentMethodData> PaymentMethods { get; set; }
        
        public RevenueReportViewModel()
        {
            RevenueByCategory = new List<CategoryRevenueData>();
            DailyRevenue = new List<DailyRevenueData>();
            TopProducts = new List<TopProductViewModel>();
            PaymentMethods = new List<PaymentMethodData>();
        }
    }
    
    public class CategoryRevenueData
    {
        public string TenLoai { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoLuong { get; set; }
    }
    
    public class DailyRevenueData
    {
        public DateTime Ngay { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoDonHang { get; set; }
    }
    
    public class PaymentMethodData
    {
        public string PhuongThuc { get; set; }
        public int SoGiaoDich { get; set; }
        public decimal TongTien { get; set; }
    }
}
