using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;

namespace shop_hubLaps.Controllers
{
    public class LaptopController : Controller
    {
        private readonly DataModel _context;
        private readonly string _imageFolder;

        public LaptopController(DataModel context)
        {
            _context = context;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        }

        // GET: Laptop
        public async Task<IActionResult> Index()
        {
            var laptops = await _context.Laptops.ToListAsync();
            return View(laptops);
        }

        /************************************/

        // GET: Laptop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops.FirstOrDefaultAsync(m => m.malaptop == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        /************************************/

        // GET: Laptop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laptop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tenlaptop,giaban,mota,hinh,cpu,gpu,ram,hardware,manhinh,pin,trangthai")] Laptop laptop, IFormFile? hinh)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ.";
                return View(laptop);
            }

            try
            {
                // Kiểm tra và xử lý ảnh
                if (hinh != null && hinh.Length > 0) // Nếu có ảnh được tải lên
                {
                    var fileName = Path.GetFileName(hinh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName); // Fixed path format

                    // Lưu ảnh vào thư mục
                    EnsureImageFolderExists(); // Ensure the image folder exists
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinh.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào đối tượng Laptop
                    laptop.hinh = "/images/" + fileName;
                }
                else
                {
                    // Nếu không có ảnh, loại bỏ lỗi yêu cầu trường hình ảnh
                    ModelState.Remove("hinh");
                }

                // Thêm laptop vào cơ sở dữ liệu
                _context.Add(laptop);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mới Laptop thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                TempData["ErrorMessage"] = "Lỗi cơ sở dữ liệu: " + dbEx.InnerException?.Message ?? dbEx.Message;
            }
            catch (IOException ioEx)
            {
                TempData["ErrorMessage"] = "Lỗi khi xử lý tập tin ảnh: " + ioEx.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
            }

            return RedirectToAction("Index", "Laptop");
        }




        // Utility: Save Image to Server
        private async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var fileName = Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(_imageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/Content/images/" + fileName;
        }

        /************************************/

        // GET: Laptop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }


        // POST: Laptop/Edit/5
        // POST: Laptop/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("malaptop,tenlaptop,giaban,mota,hinh,cpu,gpu,ram,hardware,manhinh,pin,trangthai")] Laptop laptop, IFormFile? hinh)
        {
            if (id != laptop.malaptop)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ.";
                return View(laptop);
            }

            try
            {
                // Kiểm tra và xử lý ảnh
                if (hinh != null && hinh.Length > 0) // Nếu có ảnh mới
                {
                    var fileName = Path.GetFileName(hinh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(laptop.hinh))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Path.GetFileName(laptop.hinh));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Lưu ảnh mới vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinh.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh mới vào đối tượng Laptop
                    laptop.hinh = "/images/" + fileName;
                }

                // Cập nhật laptop trong cơ sở dữ liệu
                _context.Update(laptop);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật Laptop thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaptopExists(laptop.malaptop))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /************************************/

        // GET: Laptop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .FirstOrDefaultAsync(m => m.malaptop == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // POST: Laptop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa Laptop thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy Laptop cần xóa!";
            }
            return RedirectToAction(nameof(Index));
        }


        /************************************/

        private bool LaptopExists(int id)
        {
            return _context.Laptops.Any(e => e.malaptop == id);
        }

        // Kiểm tra định dạng hình ảnh hợp lệ
        private bool IsValidImage(IFormFile file)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (file.FileName.Length > 70)
            {
                ModelState.AddModelError("hinh", "Tên hình ảnh quá dài. Vui lòng chọn hình ảnh khác.");
                return false;
            }

            return validExtensions.Contains(fileExtension);
        }
        private void EnsureImageFolderExists()
        {
            if (!Directory.Exists(_imageFolder))
            {
                Directory.CreateDirectory(_imageFolder);
            }
        }
    }
}
