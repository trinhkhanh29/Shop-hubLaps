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

        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, DataModel context, ILaptopService laptopService, ICartService cartService)
        {
            _logger = logger;
            _context = context;
            _laptopService = laptopService;
            _cartService = cartService;
        }

        public IActionResult Index(int? manhucau, decimal? pmin, decimal? pmax, string cpuFilter)
        {
            var nhuCaus = _context.NhuCaus.ToList();
            var brands = _context.Hangs.ToList();
            var chuDeList = _context.ChuDes.ToList();

            var laptopsQuery = _context.Laptops.AsQueryable();

            if (manhucau.HasValue)
            {
                laptopsQuery = laptopsQuery.Where(l => l.manhucau == manhucau);
            }

            if (pmin.HasValue && pmax.HasValue)
            {
                laptopsQuery = laptopsQuery.Where(l => l.giaban >= pmin && l.giaban <= pmax);
            }

            // Filter by CPU (if provided)
            if (!string.IsNullOrEmpty(cpuFilter))
            {
                var selectedCpus = cpuFilter.Split(',');  // Split the comma-separated CPU filters
                laptopsQuery = laptopsQuery.Where(l => selectedCpus.Contains(l.cpu));  // Filter laptops based on the selected CPUs
            }

            var laptops = laptopsQuery.ToList();

            // Fetch active advertisements from the QuangCao table
            var quangCaos = _context.QuangCaos.Where(q => q.trangthai == true).ToList();

            // Lấy thông tin giỏ hàng của người dùng (nếu có)
            var userId = User.Identity.Name;  // Hoặc lấy User ID
            var cartItems = _cartService.GetCartItems(userId);  // Giả sử có phương thức để lấy giỏ hàng của người dùng
            var cartItemCount = cartItems.Sum(item => item.soluong);  // Tính tổng số sản phẩm trong giỏ

            var cpuFilters = new Dictionary<string, string>
            {
                { "intel_core_i3", "Intel Core i3" },
                { "intel_core_i5", "Intel Core i5" },
                { "intel_core_i7", "Intel Core i7" },
                { "intel_core_i9", "Intel Core i9" },
                { "intel_celeron_pentium", "Intel Celeron / Pentium" },
                { "intel_core_u5", "Intel Core U5" },
                { "intel_core_u7", "Intel Core U7" },
                { "intel_core_u9", "Intel Core U9" },
                { "amd_ryzen_5", "AMD Ryzen 5" },
                { "amd_ryzen_7", "AMD Ryzen 7" },
                { "amd_ryzen_9", "AMD Ryzen 9" },
                { "apple_m1", "Apple M1" },
                { "apple_m1_pro", "Apple M1 Pro" },
                { "apple_m1_max", "Apple M1 Max" },
                { "apple_m2", "Apple M2" },
                { "apple_m2_pro", "Apple M2 Pro" },
                { "apple_m2_max", "Apple M2 Max" },
                { "apple_m3", "Apple M3" },
                { "apple_m3_pro", "Apple M3 Pro" },
                { "apple_m3_max", "Apple M3 Max" },
                { "qualcomm_snapdragon", "Qualcomm Snapdragon" },
                { "snapdragon_x_plus", "Snapdragon X Plus" },
                { "other", "Khác" }
            };

            // Convert the cpuFilter string to a list of selected CPUs (if any)
            var selectedCpuFilters = cpuFilter?.Split(',')?.ToList() ?? new List<string>();

            var viewModel = new HomeIndexViewModel
            {
                Brands = brands,
                NhuCaus = nhuCaus,
                Laptops = laptops,
                QuangCaos = quangCaos,
                CartItemCount = cartItemCount,
                CpuFilters = cpuFilters,
                SelectedCpuFilters = selectedCpuFilters,
                ChuDeList = chuDeList
            };

            return View(viewModel);
        }

        public IActionResult LoadMoreLaptops(int start, int count)
        {
            var laptops = _context.Laptops
                .Skip(start)
                .Take(count)
                .ToList();

            return PartialView("_LaptopPartial", laptops);
        }


        [HttpGet("Home/Search")]
        public IActionResult Search(string query, decimal? minPrice, decimal? maxPrice, int? brandId, int? categoryId)
        {
            var laptops = _context.Laptops.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                laptops = laptops.Where(l => EF.Functions.Like(l.tenlaptop, $"%{query}%"));
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                laptops = laptops.Where(l => l.giaban >= minPrice && l.giaban <= maxPrice);
            }

            if (brandId.HasValue)
            {
                laptops = laptops.Where(l => l.mahang == brandId);
            }

            if (categoryId.HasValue)
            {
                laptops = laptops.Where(l => l.manhucau == categoryId);
            }

            // Create a new HomeIndexViewModel with the laptops and other necessary data
            var nhuCaus = _context.NhuCaus.ToList();
            var brands = _context.Hangs.ToList();
            var quangCaos = _context.QuangCaos.Where(q => q.trangthai == true).ToList();

            var viewModel = new HomeIndexViewModel
            {
                Laptops = laptops.ToList(),  // List of laptops
                Brands = brands,             // List of brands (if needed in view)
                NhuCaus = nhuCaus,           // List of nhuCaus (if needed in view)
                QuangCaos = quangCaos        // Active advertisements (if needed in view)
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

    public class AddToCartModel
    {
        public int malaptop { get; set; }

        public int quantity { get; set; }
    }
}
