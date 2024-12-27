using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Index(string searchTerm)
        {
            var laptopsQuery = _context.Laptops
                .Include(l => l.NhuCau)
                .Include(l => l.Hang)
                .AsQueryable();  // Start with the full list of laptops

            // If a search term is provided, filter the laptops
            if (!string.IsNullOrEmpty(searchTerm))
            {
                laptopsQuery = laptopsQuery.Where(l =>
                    l.tenlaptop.Contains(searchTerm) || // Search by laptop name
                    (l.Hang != null && l.Hang.tenhang.Contains(searchTerm)) || // Search by brand name (if Hang is not null)
                    (l.NhuCau != null && l.NhuCau.tennhucau.Contains(searchTerm)) || // Search by needs (if NhuCau is not null)
                    (l.cpu != null && l.cpu.Contains(searchTerm)) || // Search by CPU (if cpu is not null)
                    (l.gpu != null && l.gpu.Contains(searchTerm)) // Search by GPU (if gpu is not null)
                );
            }

            var laptops = await laptopsQuery.ToListAsync();

            ViewBag.NhuCauList = _context.NhuCaus.ToList();
            ViewBag.HangList = _context.Hangs.ToList();

            return View(laptops);
        }

        /************************************/

        // GET: Laptop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var laptop = _context.Laptops.FirstOrDefault(l => l.malaptop == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);  // Pass a single Laptop object to the view
        }

        /************************************/

        // GET: Laptop/Create
        public IActionResult Create()        {
           
            ViewBag.HangList = _context.Hangs.ToList();

            ViewBag.NhuCauList = _context.NhuCaus.ToList(); 

            return View();
        }

        // POST: Laptop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tenlaptop,giaban,mota,hinh,cpu,gpu,ram,hardware,manhinh,pin,trangthai,mahang,manhucau,soluongton")] Laptop laptop,IFormFile? hinh)
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
                laptop.ngaycapnhat = DateTime.Now;
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
            
                return NotFound();

            var laptop = await _context.Laptops.FindAsync(id);

            ViewBag.HangList = _context.Hangs.ToList();

            ViewBag.NhuCauList = _context.NhuCaus.ToList();

            if (laptop == null)
            
                return NotFound();
            
            return View(laptop);


        }
        // POST: Laptop/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("malaptop,tenlaptop,giaban,mota,hinh,cpu,gpu,ram,hardware,manhinh,pin,trangthai,manhucau,mahang,soluongton")] Laptop laptop, IFormFile? hinh)

        {
            if (id != laptop.malaptop)
            {
                TempData["ErrorMessage"] = "ID Laptop không khớp.";
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                return View(laptop);
            }
            try
            {
                // Kiểm tra nếu có upload hình mới
                if (hinh != null && hinh.Length > 0)
                {
                    // Kiểm tra tính hợp lệ của hình ảnh
                    if (!IsValidImage(hinh))
                    {
                        TempData["ErrorMessage"] = "Hình ảnh không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF, tên file dưới 70 ký tự.";
                        return RedirectToAction("Index", "Laptop");
                    }

                    // Đường dẫn và tên file mới
                    var fileName = Path.GetFileName(hinh.FileName);
                    var filePath = Path.Combine(_imageFolder, fileName);

                    // Xóa hình ảnh cũ (nếu có)
                    if (!string.IsNullOrEmpty(laptop.hinh))
                    {
                        var oldFilePath = Path.Combine(_imageFolder, Path.GetFileName(laptop.hinh));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Lưu hình ảnh mới vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinh.CopyToAsync(stream);
                    }

                    // Cập nhật đường dẫn ảnh mới
                    laptop.hinh = "/images/" + fileName;
                }

                laptop.ngaycapnhat = DateTime.Now;

                _context.Update(laptop);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật Laptop thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaptopExists(laptop.malaptop))
                {
                    TempData["ErrorMessage"] = "Laptop không tồn tại.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi xung đột khi cập nhật dữ liệu.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }

            // Dù đúng hay sai, luôn quay về trang chính
            return RedirectToAction("Index", "Laptop");
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

        // API endpoint để lấy danh sách laptops
        [HttpGet("GetLaptops")]
        public IActionResult GetLaptops()
        {
            // Lấy danh sách laptops từ database
            var laptops = _context.Laptops.ToList();
            return Ok(laptops); // Trả về kết quả dưới dạng JSON
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
