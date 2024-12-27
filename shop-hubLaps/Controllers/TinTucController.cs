using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
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
        public IActionResult Create()
        {
            var categories = _context.ChuDes.ToList();
            if (categories == null || categories.Count == 0)
            {
                // Handle the case where no categories exist in the database.
                ViewData["ChuDeList"] = new List<ChuDe>();
            }
            else
            {
                ViewData["ChuDeList"] = categories;
            }

            return View(new TinTuc());
        }

        // POST: TinTuc/Create
        // POST: TinTuc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("matin,tieude,hinhnen,tomtat,slug,noidung,luotxem,ngaycapnhat,xuatban,machude")] TinTuc tinTuc, IFormFile? hinhnen)
        {
            // Check if model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Process the image upload if a file is provided
                    if (hinhnen != null && hinhnen.Length > 0)
                    {
                        var filePath = Path.Combine(_imageFolder, hinhnen.FileName);  // Define file path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await hinhnen.CopyToAsync(stream);  // Save the file to the server
                        }
                        tinTuc.hinhnen = "/Content/images/" + hinhnen.FileName;  // Set the image path in the model
                    }

                    // Add the news article to the database
                    _context.Add(tinTuc);
                    await _context.SaveChangesAsync();  // Save changes to the database

                    // Set a success message and redirect to the Index page
                    TempData["SuccessMessage"] = "The news article was created successfully!";
                    return RedirectToAction("Index", "TinTuc");
                }
                catch (Exception ex)
                {
                    // Log error and show an error message
                    _logger.LogError(ex, "Error while creating news article");
                    TempData["ErrorMessage"] = "An error occurred while creating the news article.";
                }
            }
            else
            {
                // If the model state is not valid, return the view with the existing model and validation errors
                TempData["ErrorMessage"] = "Please ensure all required fields are filled out correctly.";
            }

            // Return the same view with the model to show validation errors
            ViewData["ChuDeList"] = _context.ChuDes.ToList();  // Load categories again
            return View(tinTuc);  // Return the view with the model to show any validation errors
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


        // Lưu hình ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            EnsureImageFolderExists();  // Đảm bảo thư mục chứa ảnh tồn tại

            string fileName = Path.GetFileName(image.FileName); // Lấy tên file ảnh
            string filePath = Path.Combine(_imageFolder, fileName); // Đường dẫn lưu ảnh

            // Kiểm tra xem ảnh đã tồn tại chưa
            if (System.IO.File.Exists(filePath))
            {
                // Nếu ảnh tồn tại, có thể đổi tên hoặc xử lý phù hợp
                fileName = Path.GetFileNameWithoutExtension(image.FileName) + Guid.NewGuid() + Path.GetExtension(image.FileName);
                filePath = Path.Combine(_imageFolder, fileName);
            }

            // Lưu ảnh vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName; // Trả về tên file để lưu vào database
        }

        // Đảm bảo thư mục chứa hình ảnh tồn tại
        private void EnsureImageFolderExists()
        {
            if (!Directory.Exists(_imageFolder))  // Kiểm tra thư mục chứa ảnh
            {
                Directory.CreateDirectory(_imageFolder); // Nếu không có, tạo thư mục
            }
        }
    }
}
