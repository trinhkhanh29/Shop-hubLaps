using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using shop_hubLaps.Services;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace shop_hubLaps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataModel _context;
        private readonly ILaptopService _laptopService;

        public HomeController(ILogger<HomeController> logger, DataModel context, ILaptopService laptopService)
        {
            _logger = logger;
            _context = context;
            _laptopService = laptopService;

        }

        public IActionResult Index(int? manhucau, decimal? pmin, decimal? pmax)
        {
            var nhuCaus = _context.NhuCaus.ToList();
            var brands = _context.Hangs.ToList();

            var laptopsQuery = _context.Laptops.AsQueryable();

            if (manhucau.HasValue)
            {
                laptopsQuery = laptopsQuery.Where(l => l.manhucau == manhucau);
            }

            if (pmin.HasValue && pmax.HasValue)
            {
                laptopsQuery = laptopsQuery.Where(l => l.giaban >= pmin && l.giaban <= pmax);
            }

            var laptops = laptopsQuery.ToList();

            // Fetch active advertisements from the QuangCao table
            var quangCaos = _context.QuangCaos.Where(q => q.trangthai == true).ToList();

            var viewModel = new HomeIndexViewModel
            {
                Brands = brands,
                NhuCaus = nhuCaus,
                Laptops = laptops,
                QuangCaos = quangCaos // Pass the ads to the view

            };

            return View(viewModel);
        }

        public IActionResult Details(int malaptop)
        {
            // Fetch laptop details based on `malaptop` ID
            var laptop = _context.Laptops
                .Include(l => l.DanhGias)
                .FirstOrDefault(l => l.malaptop == malaptop);

            if (laptop == null)
            {
                return NotFound("Laptop không tồn tại.");
            }

            return View(laptop); // Pass the `Laptop` object to the view
        }

        // API endpoint to get laptops by nhuCau (AJAX request)
        [HttpGet]
        public IActionResult GetLaptopsByNhuCau(int nhuCauId)
        {
            var laptops = _context.Laptops
             .Where(l => l.manhucau == nhuCauId)
             .Select(l => new
             {
                 l.malaptop,     // Laptop ID
                 l.tenlaptop,    // Laptop name
                 l.mota,         // Description
                 l.hinh,         // Image URL
                 l.giaban,       // Price
                 l.cpu,          // CPU
                 l.ram,          // RAM
                 l.hardware,     // Hardware
                 l.manhinh,      // Screen size
                 l.pin,          // Battery
                 l.gpu           // GPU (added)
             })
             .ToList();

            // Return the laptops as a JSON response to the frontend
            return Json(laptops);
        }
        public IActionResult FilterLaptops(int pmin, int pmax)
        {
            var filteredLaptops = _context.Laptops
                .Where(l => l.giaban >= pmin && l.giaban <= pmax)
                .ToList();

            return View(filteredLaptops);
        }
       

            // API để lấy laptop theo sắp xếp
            [HttpGet]
        public IActionResult GetLaptopsBySort(int sortId)
        {
            try
            {
                // Gọi dịch vụ để lấy danh sách laptop theo sortId
                var laptops = _laptopService.GetLaptopsBySort(sortId);

                // Trả về danh sách laptop dưới dạng JSON
                return Json(laptops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải laptop theo sắp xếp");
                return BadRequest("Không thể tải laptop. Vui lòng thử lại sau.");
            }
        }

        // Post method to add a comment for a laptop
        [HttpPost]
        public IActionResult AddComment(string ten, string noidung, int vote, int malaptop)
        {
            // Check if name or content is empty
            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(noidung))
            {
                TempData["ErrorMessage"] = "Tên và nội dung không được để trống.";
                return RedirectToAction("Details", new { malaptop });
            }

            // Check if laptop exists
            var laptopExists = _context.Laptops.Any(l => l.malaptop == malaptop);
            if (!laptopExists)
            {
                TempData["ErrorMessage"] = "Laptop không tồn tại.";
                return RedirectToAction("Details", new { malaptop });
            }

            // Remove any HTML tags from the content
            var plainText = Regex.Replace(noidung, "<.*?>", String.Empty);

            // Create new comment (DanhGia)
            var danhGia = new DanhGia
            {
                ten = ten,
                noidung = plainText,
                vote = vote,
                ngaydanhgia = DateTime.Now,
                malaptop = malaptop,
                trangthai = true
            };

            // Add comment to the database
            _context.DanhGias.Add(danhGia);
            _context.SaveChanges();

            // Set success message
            TempData["SuccessMessage"] = "Bình luận của bạn đã được gửi thành công.";
            return RedirectToAction("Details", new { malaptop });
        }

        [HttpGet]
        public IActionResult GetLaptopsByBrand(int brandId)
        {
            var laptops = _context.Laptops
                .Where(l => l.mahang == brandId)
                .Select(l => new
                {
                    l.malaptop,
                    l.tenlaptop,
                    l.mota,
                    l.hinh,
                    l.giaban,
                    l.cpu,
                    l.ram,
                    l.hardware,
                    l.manhinh,
                    l.pin,
                    l.gpu
                })
                .ToList();

            return Json(laptops);
        }

        // Hành động tìm kiếm
        public IActionResult Search(string query, int? manhucau, decimal? pmin, decimal? pmax)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("Index", _context.Laptops.ToList());
            }

            // Lọc sản phẩm theo tên
            var result = _context.Laptops
                .Where(l => l.tenlaptop.Contains(query))
                .ToList();

            ViewBag.SearchQuery = query; // Hiển thị thông báo tìm kiếm
            return View("Index", result);
        }

        // Add laptop to the cart (AJAX)
        [HttpPost]
        public IActionResult AddToCart([FromBody] AddToCartModel model)
        {
            if (model == null || model.malaptop <= 0 || model.quantity <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });
            }

            // Kiểm tra nếu giỏ hàng đã tồn tại
            var cart = _context.DonHangs
                .FirstOrDefault(d => d.makh == userId && d.tinhtrang == "CART");

            // Nếu không có giỏ hàng, tạo mới
            if (cart == null)
            {
                cart = new DonHang
                {
                    makh = userId,
                    tinhtrang = "CART",
                    ngaydat = DateTime.Now
                };
                _context.DonHangs.Add(cart);
                _context.SaveChanges();
            }

            // Lấy thông tin laptop
            var laptop = _context.Laptops.FirstOrDefault(l => l.malaptop == model.malaptop);
            if (laptop == null)
            {
                return Json(new { success = false, message = "Laptop không tồn tại." });
            }

            // Kiểm tra xem laptop đã có trong giỏ hàng chưa
            var existingItem = _context.ChiTietDonHangs
                .FirstOrDefault(c => c.madon == cart.madon && c.malaptop == model.malaptop);

            if (existingItem != null)
            {
                existingItem.soluong += model.quantity; // Cập nhật số lượng nếu đã có trong giỏ hàng
            }
            else
            {
                var newItem = new ChiTietDonHang
                {
                    madon = cart.madon,
                    malaptop = model.malaptop,
                    soluong = model.quantity,
                    dongia = laptop.giaban ?? 0
                };
                _context.ChiTietDonHangs.Add(newItem);
            }

            _context.SaveChanges();
            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
        }    


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    // Model for handling Add to Cart requests
    public class AddToCartModel
    {
        public int malaptop { get; set; }
        public int quantity { get; set; }
    }
}
