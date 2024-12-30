using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shop_hubLaps.Models;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataModel _context;

        public DashboardController(DataModel context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Thống kê các mặt hàng mới nhất
            var newLaptops = _context.Laptops
              .OrderByDescending(l => l.ngaycapnhat)
              .Take(5)
              .Select(l => new { l.tenlaptop, l.hinh }) // Lấy chỉ trường cần thiết
              .ToList();


            // 2. Thống kê trạng thái đơn hàng
            var orderStatusCounts = _context.DonHangs
                .GroupBy(d => d.tinhtrang)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Status, x => x.Count);

            // 3. Tổng doanh thu
            var totalRevenue = _context.DonHangs
                .Where(d => d.tinhtrang == "COMPLETED")
                .Sum(d => d.gia);

            // 3.1. Doanh thu hôm nay
            var todayRevenue = _context.DonHangs
                .Where(d => d.tinhtrang == "COMPLETED" && d.ngaygiao.HasValue && d.ngaygiao.Value.Date == DateTime.Today)
                .Sum(d => d.gia);

            // 4. Sản phẩm bán chạy nhất
            var topSellingProducts = _context.ChiTietDonHangs
                .GroupBy(ct => new { ct.malaptop, ct.Laptop.tenlaptop }) // Lấy cả tên laptop
                .Select(g => new
                {
                    LaptopName = g.Key.tenlaptop.Length > 20 ? g.Key.tenlaptop.Substring(0, 20) + "..." : g.Key.tenlaptop, // Cắt tên nếu quá dài
                    TotalSold = g.Sum(ct => ct.soluong) // Tổng số lượng bán
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToList();

            // 5. Hàng tồn kho thấp
            var lowStockProducts = _context.Laptops
                .Where(l => l.soluongton < 10)
                .ToList();

            // 6. Sản phẩm chưa được đánh giá
            var unratedProducts = _context.Laptops
                .Where(l => !l.DanhGias.Any())
                .ToList();

            // Truyền dữ liệu sang view
            ViewBag.NewLaptops = newLaptops;
            ViewBag.OrderStatusCounts = orderStatusCounts;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TodayRevenue = todayRevenue;
            ViewBag.TopSellingProducts = topSellingProducts;
            ViewBag.LowStockProducts = lowStockProducts;
            ViewBag.UnratedProducts = unratedProducts;

            return View();
        }
    }
}
