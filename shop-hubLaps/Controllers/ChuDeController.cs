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

        /************************************/

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

        /************************************/

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
        public async Task<IActionResult> Edit(int id, [Bind("machude,tenchude,slug")] ChuDe chuDe, IFormFile? newHinh)
        {
            if (id != chuDe.machude)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Retrieve the existing entity from the database
                var existingChuDe = await _context.ChuDes.AsNoTracking().FirstOrDefaultAsync(x => x.machude == id);
                if (existingChuDe == null)
                    return NotFound();

                // Preserve the existing image if no new image is uploaded
                chuDe.hinh = existingChuDe.hinh;

                // Replace the image if a new one is uploaded
                if (newHinh != null && IsValidImage(newHinh))
                {
                    // Delete the old image file if it exists
                    if (!string.IsNullOrEmpty(existingChuDe.hinh))
                    {
                        DeleteImage(existingChuDe.hinh);
                    }

                    // Save the new image and update the property
                    chuDe.hinh = await SaveImage(newHinh);
                }

                try
                {
                    // Update the entity in the database
                    _context.Entry(chuDe).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Chủ đề đã được chỉnh sửa thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuDeExists(chuDe.machude))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Log validation errors for debugging
            Console.WriteLine("ModelState is invalid: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));

            TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật. Vui lòng kiểm tra lại!";
            return View(chuDe);
        }

        // Utility method to check if an entity exists
        private bool ChuDeExists(int id)
        {
            return _context.ChuDes.Any(e => e.machude == id);
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

        /************************************/

        // Xác nhận xóa chủ đề
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chuDe = await _context.ChuDes.FirstOrDefaultAsync(m => m.machude == id);
            if (chuDe == null) return NotFound();

            return View(chuDe);
        }

        // Thực hiện xóa chủ đề
        // POST: Administrator/ChuDe/Delete/5
        [HttpPost]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe == null)
            {
                TempData["ErrorMessage"] = "Chủ đề không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Xóa hình ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(chuDe.hinh))
                {
                    DeleteImage(chuDe.hinh);
                }

                // Xóa chủ đề khỏi cơ sở dữ liệu
                _context.ChuDes.Remove(chuDe);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Chủ đề đã được xóa thành công!";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Lỗi xóa chủ đề: {ex.Message}");
                TempData["ErrorMessage"] = "Không thể xóa chủ đề vì nó đang được liên kết trong hệ thống!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi không xác định. Vui lòng thử lại!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Xóa hình ảnh
        private void DeleteImage(string? imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                string filePath = Path.Combine(_imageFolder, imageName);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                        Console.WriteLine($"Đã xóa hình ảnh: {filePath}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Không thể xóa hình ảnh: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Hình ảnh không tồn tại tại đường dẫn: " + filePath);
                }
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
            return validExtensions.Contains(fileExtension);
        }
    }
}
