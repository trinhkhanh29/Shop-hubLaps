using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("machude,tenchude,slug,hinh")] ChuDe chuDe, IFormFile? hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và lưu hình ảnh
                    if (hinh != null)
                    {
                        // Kiểm tra xem ảnh có hợp lệ không
                        if (!IsValidImage(hinh))
                        {
                            ModelState.AddModelError("hinh", "Định dạng hình ảnh không hợp lệ. Vui lòng chọn ảnh JPG, JPEG, PNG hoặc GIF.");
                            return View(chuDe);  // Trả về form với lỗi nếu ảnh không hợp lệ
                        }

                        // Lưu ảnh vào thư mục và lấy tên file
                        string fileName = await SaveImage(hinh);
                        chuDe.hinh = fileName;  // Lưu tên file hình ảnh vào chuDe
                    }

                    // Thêm chủ đề mới vào cơ sở dữ liệu
                    _context.Add(chuDe);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chủ đề đã được tạo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log lỗi chi tiết
                    Console.WriteLine($"Lỗi khi tạo chủ đề: {ex.Message}");

                    // Thêm thông báo lỗi chung
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo chủ đề. Vui lòng thử lại!";
                    return View(chuDe);
                }
            }
            else
            {
                // Trả về form với lỗi nếu model không hợp lệ
                TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại!";
                return View(chuDe);
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("machude,tenchude,slug,hinh")] ChuDe chuDe, IFormFile? newHinh)
        {
            if (id != chuDe.machude)
            {
                TempData["ErrorMessage"] = "ID không khớp với chủ đề!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingChuDe = await _context.ChuDes.AsNoTracking().FirstOrDefaultAsync(x => x.machude == id);
                if (existingChuDe == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy chủ đề để chỉnh sửa!";
                    return NotFound();
                }

                chuDe.hinh = existingChuDe.hinh;

                if (newHinh != null)
                {
                    if (!IsValidImage(newHinh))
                    {
                        ModelState.AddModelError("hinh", "Định dạng hình ảnh không hợp lệ. Vui lòng chọn ảnh JPG, JPEG, PNG hoặc GIF.");
                        return View(chuDe); 
                    }

                    if (!string.IsNullOrEmpty(existingChuDe.hinh))
                    {
                        DeleteImage(existingChuDe.hinh);
                    }

                    chuDe.hinh = await SaveImage(newHinh);
                }

                try
                {
                    _context.Entry(chuDe).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Chủ đề đã được chỉnh sửa thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Lỗi khi cập nhật chủ đề: {ex.Message}");
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật chủ đề. Vui lòng thử lại!";
                    return View(chuDe);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi không xác định. Vui lòng thử lại!";
                    return View(chuDe);
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));
                TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại!";
                return View(chuDe);
            }
        }


        // Utility method to check if an entity exists
        private bool ChuDeExists(int id)
        {
            return _context.ChuDes.Any(e => e.machude == id);
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



        // Kiểm tra định dạng hình ảnh hợp lệ
        private bool IsValidImage(IFormFile file)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            return validExtensions.Contains(fileExtension);
        }
    }
}
