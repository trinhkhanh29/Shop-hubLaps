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
            var tinTucs = await _context.TinTucs.Include(t => t.ChuDe).ToListAsync();
            return View(tinTucs);
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

        /************************************/

        // Form tạo mới tin tức
        public IActionResult Create()
        {
            // Đảm bảo rằng ChuDeList đã được gán đúng danh sách chủ đề từ cơ sở dữ liệu
            ViewData["ChuDeList"] = new SelectList(_context.ChuDes, "machude", "tenchude");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("matin,tieude,tomtat,slug,noidung,luotxem,ngaycapnhat,xuatban,machude")] TinTuc tinTuc, IFormFile? hinhnen)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem machude có giá trị hợp lệ không
                if (tinTuc.machude == null || tinTuc.machude == 0)
                {
                    ModelState.AddModelError("machude", "Vui lòng chọn một chủ đề.");
                }

                try
                {
                    // Xử lý lưu hình ảnh nếu có
                    if (hinhnen != null && IsValidImage(hinhnen))
                    {
                        string fileName = await SaveImage(hinhnen);
                        tinTuc.hinhnen = fileName;
                        _logger.LogInformation("Image uploaded: {ImageName}", fileName);
                    }

                    // Thêm tin tức vào cơ sở dữ liệu
                    _context.Add(tinTuc);
                    await _context.SaveChangesAsync();

                    // Cập nhật thông báo thành công
                    TempData["SuccessMessage"] = "Tin tức đã được tạo thành công!";
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Database error while creating TinTuc");
                    TempData["ErrorMessage"] = $"Lỗi cơ sở dữ liệu: {dbEx.Message}";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating TinTuc");
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }
            else
            {
                // Nếu dữ liệu không hợp lệ, hiển thị lỗi chi tiết
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                string errorMessages = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại:\n";

                foreach (var error in errors)
                {
                    errorMessages += $"{error.ErrorMessage}\n";
                }

                TempData["ErrorMessage"] = errorMessages;
            }

            // Quay lại trang Index của TinTuc với thông báo
            return RedirectToAction("Index", "TinTuc");
        }







        // Lưu hình ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            EnsureImageFolderExists();

            // Tạo tên tệp duy nhất sử dụng GUID để tránh xung đột
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(_imageFolder, fileName);

            Console.WriteLine($"Saving image: {filePath}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }

        /************************************/

        // Đảm bảo thư mục chứa hình ảnh tồn tại
        private void EnsureImageFolderExists()
        {
            if (!Directory.Exists(_imageFolder))
            {
                Directory.CreateDirectory(_imageFolder);
            }
        }

        // Kiểm tra định dạng hình ảnh hợp lệ
        private bool IsValidImage(IFormFile file)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            // Kiểm tra chiều dài của tên file
            if (file.FileName.Length > 70)
            {
                ModelState.AddModelError("Hinh", "Tên hình ảnh quá dài. Vui lòng chọn hình ảnh khác.");
                return false;
            }

            return validExtensions.Contains(fileExtension);
        }
    }
}
