using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TinTucController : Controller
    {
        private readonly DataModel _context;
        private readonly string _imageFolder;
        private readonly ILogger<TinTucController> _logger;

        public TinTucController(DataModel context, ILogger<TinTucController> logger)
        {
            _context = context;
            _logger = logger;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images");

        }

        // Hiển thị danh sách tin tức
        public async Task<IActionResult> Index()
        {
            try
            {
                var tinTucList = await _context.TinTucs
                    .Include(t => t.ChuDe)
                    .ToListAsync();

                if (!tinTucList.Any())
                {
                    TempData["WarningMessage"] = "Hiện không có tin tức nào để hiển thị.";
                }

                return View(tinTucList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách tin tức");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải danh sách tin tức.";
                return View(new List<TinTuc>());
            }
        }


        /************************************/

        // GET: TinTuc/Create
        // GET: TinTuc/Create
        public IActionResult Create()
        {
            var categories = _context.ChuDes.ToList();
            if (categories == null || categories.Count == 0)
            {
                // If no categories found, set an empty list in ViewData
                TempData["ErrorMessage"] = "No categories found, please add categories first.";
                ViewData["ChuDeList"] = new List<ChuDe>();  // Return an empty list
            }
            else
            {
                // Pass categories to the view
                ViewData["ChuDeList"] = categories;
            }

            return View(new TinTuc("Default Title", "default-slug", 1, new ChuDe()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("matin,tieude,hinhnen,tomtat,slug,noidung,ngaycapnhat,xuatban,machude")] TinTuc tinTuc, IFormFile? hinhnen)
        {
            _logger.LogInformation("Initial Model: " + JsonConvert.SerializeObject(tinTuc));

            // Set default values if not provided
            if (string.IsNullOrEmpty(tinTuc.tieude)) tinTuc.tieude = "Default Title";
            if (string.IsNullOrEmpty(tinTuc.slug)) tinTuc.slug = "default-slug";
            if (string.IsNullOrEmpty(tinTuc.noidung)) tinTuc.noidung = "Default content";
            if (tinTuc.ngaycapnhat == null) tinTuc.ngaycapnhat = DateTime.Now;

            // Check and assign a default category if no category is selected
            if (tinTuc.machude == 0)
            {
                tinTuc.machude = 1; // Default category (could be a valid ID)
            }

            // Set 'xuatban' to true as it's always being published
            tinTuc.xuatban = true;

            _logger.LogInformation("Model after applying default values: " + JsonConvert.SerializeObject(tinTuc));

            if (ModelState.IsValid)
            {
                try
                {
                    if (hinhnen != null && hinhnen.Length > 0)
                    {
                        var filePath = Path.Combine(_imageFolder, hinhnen.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await hinhnen.CopyToAsync(stream);
                        }
                        tinTuc.hinhnen = "/Content/images/" + hinhnen.FileName;
                    }

                    // Add the news article to the database
                    _context.Add(tinTuc);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "The news article was created successfully!";
                    return RedirectToAction("Index", "TinTuc");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating news article");
                    TempData["ErrorMessage"] = "An error occurred while creating the news article.";
                }
            }
            else
            {
                // Log specific errors for invalid fields
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("ModelState Error: " + error.ErrorMessage);
                }

                // Return feedback on invalid fields
                string missingFields = string.Join(", ", ModelState.Keys.Where(key => ModelState[key]?.Errors?.Any() ?? false));
                TempData["ErrorMessage"] = "The following fields are required or invalid: " + missingFields;
            }

            // Return the view with the model and error messages
            ViewData["ChuDeList"] = _context.ChuDes.ToList();
            return View(tinTuc);
        }





        // Hiển thị chi tiết tin tức
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tinTuc = await _context.TinTucs
                .Include(t => t.ChuDe)
                .FirstOrDefaultAsync(m => m.matin == id);

            if (tinTuc == null) return NotFound();

            return View(tinTuc);
        }


        //// Lưu hình ảnh
        //private async Task<string> SaveImage(IFormFile image)
        //{
        //    EnsureImageFolderExists();  // Đảm bảo thư mục chứa ảnh tồn tại

        //    string fileName = Path.GetFileName(image.FileName); // Lấy tên file ảnh
        //    string filePath = Path.Combine(_imageFolder, fileName); // Đường dẫn lưu ảnh

        //    // Kiểm tra xem ảnh đã tồn tại chưa
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        // Nếu ảnh tồn tại, có thể đổi tên hoặc xử lý phù hợp
        //        fileName = Path.GetFileNameWithoutExtension(image.FileName) + Guid.NewGuid() + Path.GetExtension(image.FileName);
        //        filePath = Path.Combine(_imageFolder, fileName);
        //    }

        //    // Lưu ảnh vào thư mục
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(stream);
        //    }

        //    return fileName; // Trả về tên file để lưu vào database
        //}

        //// Đảm bảo thư mục chứa hình ảnh tồn tại
        //private void EnsureImageFolderExists()
        //{
        //    if (!Directory.Exists(_imageFolder))  // Kiểm tra thư mục chứa ảnh
        //    {
        //        Directory.CreateDirectory(_imageFolder); // Nếu không có, tạo thư mục
        //    }
        //}
    }
}
