using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    public class ChuDeController : Controller
    {
        private readonly DataModel _context;
        private readonly string _imageFolder;

        public ChuDeController(DataModel context)
        {
            _context = context;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images");
        }

        // Hiển thị danh sách các chủ đề
        public async Task<IActionResult> Index()
        {
            var chuDes = await _context.ChuDes.ToListAsync();
            return View(chuDes);
        }

        // Hiển thị chi tiết chủ đề
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chuDe = await _context.ChuDes.FirstOrDefaultAsync(m => m.machude == id);
            if (chuDe == null) return NotFound();

            return View(chuDe);
        }

        // Form tạo mới chủ đề
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo mới chủ đề (bao gồm tải lên hình ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("machude,tenchude,slug")] ChuDe chuDe, IFormFile? hinh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra và lưu hình ảnh
                if (hinh != null && IsValidImage(hinh))
                {
                    string fileName = await SaveImage(hinh);
                    chuDe.hinh = fileName;
                }

                _context.Add(chuDe);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Chủ đề đã được tạo thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(chuDe);
        }

        // Form sửa chủ đề
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe == null) return NotFound();

            return View(chuDe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("machude,tenchude,slug,hinh")] ChuDe chuDe, IFormFile? newHinh)
        {
            // Kiểm tra nếu id không khớp với đối tượng chuDe
            if (id != chuDe.machude)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Tìm chuDe cũ trong DB
                var existingChuDe = await _context.ChuDes.FindAsync(id);

                if (existingChuDe == null)
                    return NotFound();

                // Kiểm tra và thay thế hình ảnh nếu có hình mới
                if (newHinh != null && IsValidImage(newHinh))
                {
                    // Xóa hình ảnh cũ
                    DeleteImage(existingChuDe.hinh);

                    // Lưu hình ảnh mới và cập nhật tên hình ảnh
                    string newFileName = await SaveImage(newHinh);
                    existingChuDe.hinh = newFileName;
                }

                // Cập nhật các trường khác (tenchude, slug)
                existingChuDe.tenchude = chuDe.tenchude;
                existingChuDe.slug = chuDe.slug;

                // Đảm bảo entity được đánh dấu là đã thay đổi
                _context.Entry(existingChuDe).State = EntityState.Modified;

                // Log chi tiết về đối tượng sẽ được cập nhật
                Console.WriteLine($"ChuDe Update: {existingChuDe.machude}, {existingChuDe.tenchude}, {existingChuDe.slug}");

                try
                {
                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Thông báo thành công
                    TempData["SuccessMessage"] = "Chủ đề đã được chỉnh sửa thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Ghi lại lỗi trong quá trình cập nhật cơ sở dữ liệu
                    Console.WriteLine("Error: " + ex.Message);
                    TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi cập nhật dữ liệu. Vui lòng thử lại.";
                    return View(chuDe);
                }
            }

            // Nếu có lỗi trong model binding
            Console.WriteLine("ModelState is invalid: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));
            return View(chuDe);
        }


        // Xác nhận xóa chủ đề
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chuDe = await _context.ChuDes.FirstOrDefaultAsync(m => m.machude == id);
            if (chuDe == null) return NotFound();

            return View(chuDe);
        }

        // Thực hiện xóa chủ đề
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe != null)
            {
                try
                {
                    // Xóa hình ảnh nếu có
                    DeleteImage(chuDe.hinh);

                    // Xóa đối tượng chủ đề trong DB
                    _context.ChuDes.Remove(chuDe);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Chủ đề đã được xóa thành công!";
                    return Json(new { success = true, message = "Item deleted successfully!" });
                }
                catch (Exception ex)
                {
                    // Log exception
                    Console.WriteLine($"Error deleting item: {ex.Message}");
                    TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi xóa chủ đề. Vui lòng thử lại.";
                    return Json(new { success = false, message = "An error occurred while deleting the item." });
                }
            }

            return Json(new { success = false, message = "Item not found!" });
        }



        // Lưu hình ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            EnsureImageFolderExists();

            string fileName = Path.GetFileName(image.FileName);
            string filePath = Path.Combine(_imageFolder, fileName);

            Console.WriteLine($"Saving image: {filePath}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }
        // Xóa hình ảnh
        private void DeleteImage(string? imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                string filePath = Path.Combine(_imageFolder, imageName);
                Console.WriteLine($"Attempting to delete image at: {filePath}");

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine($"Image {imageName} deleted.");
                }
                else
                {
                    Console.WriteLine($"Image {imageName} not found at {filePath}.");
                }
            }
        }


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
            return validExtensions.Contains(fileExtension);
        }
    }
}
