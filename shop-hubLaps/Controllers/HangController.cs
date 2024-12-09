using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    public class HangController : Controller
    {
        private readonly DataModel _context;
        private readonly string _imageFolder;

        public HangController(DataModel context)
        {
            _context = context;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images");
        }

        // Hiển thị danh sách hãng
        public async Task<IActionResult> Index()
        {
            var hangList = await _context.Hangs.ToListAsync();
            return View(hangList);
        }

        // Hiển thị chi tiết hãng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hang = await _context.Hangs.FirstOrDefaultAsync(h => h.mahang == id);

            if (hang == null) return NotFound();

            return View(hang);
        }

        /************************************/

        // Form tạo mới hãng
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tenhang,hinh")] Hang hang, IFormFile? hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và lưu hình ảnh nếu có
                    if (hinh != null && IsValidImage(hinh))
                    {
                        string fileName = await SaveImage(hinh);
                        hang.hinh = fileName;  // Lưu tên file hình ảnh vào trường hinh
                    }

                    // Thêm hãng vào cơ sở dữ liệu (mahang sẽ được tự động sinh)
                    _context.Add(hang);
                    await _context.SaveChangesAsync();

                    // Cập nhật thông báo thành công
                    TempData["SuccessMessage"] = "Hãng đã được tạo thành công!";
                }
                catch (Exception ex)
                {
                    // Cập nhật thông báo lỗi nếu có ngoại lệ
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }
            else
            {
                // Kiểm tra lỗi của modelState và hiển thị thông báo chi tiết
                string validationErrors = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại:\n";

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    validationErrors += $"{error.ErrorMessage}\n";
                }

                TempData["ValidationErrors"] = validationErrors;
            }

            // Quay lại trang Index của Hang
            return RedirectToAction("Index", "Hang");
        }

        /************************************/

        // Sửa hãng
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hang = await _context.Hangs.FindAsync(id);
            if (hang == null) return NotFound();

            return View(hang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("mahang,tenhang,hinh")] Hang hang, IFormFile? hinh)
        {
            if (id != hang.mahang) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và lưu hình ảnh nếu có
                    if (hinh != null && IsValidImage(hinh))
                    {
                        string fileName = await SaveImage(hinh);
                        hang.hinh = fileName;  // Lưu tên file hình ảnh vào trường hinh
                    }

                    _context.Update(hang);
                    await _context.SaveChangesAsync();

                    // Cập nhật thông báo thành công
                    TempData["SuccessMessage"] = "Hãng đã được cập nhật thành công!";
                }
                catch (Exception ex)
                {
                    // Cập nhật thông báo lỗi nếu có ngoại lệ
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }
            else
            {
                // Kiểm tra lỗi của modelState và hiển thị thông báo chi tiết
                string validationErrors = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại:\n";

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    validationErrors += $"{error.ErrorMessage}\n";
                }

                TempData["ValidationErrors"] = validationErrors;
            }

            return RedirectToAction("Index", "Hang");
        }

        /************************************/

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

        // Xác nhận xóa hãng
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hang = await _context.Hangs.FirstOrDefaultAsync(m => m.mahang == id);
            if (hang == null) return NotFound();

            return View(hang);
        }

        // Thực hiện xóa hãng
        // POST: Administrator/Hang/Delete/5
        [HttpPost]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hang = await _context.Hangs.FindAsync(id);
            if (hang == null)
            {
                TempData["ErrorMessage"] = "Hãng không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Xóa hình ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(hang.hinh))
                {
                    DeleteImage(hang.hinh);
                }

                // Xóa hãng khỏi cơ sở dữ liệu
                _context.Hangs.Remove(hang);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Hãng đã được xóa thành công!";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Lỗi xóa hãng: {ex.Message}");
                TempData["ErrorMessage"] = "Không thể xóa hãng vì nó đang được liên kết trong hệ thống!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi không xác định. Vui lòng thử lại!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Xóa hình ảnh (nếu có) khỏi thư mục
        private void DeleteImage(string imageName)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
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
