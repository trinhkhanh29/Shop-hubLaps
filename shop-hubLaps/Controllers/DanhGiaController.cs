using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.Linq;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DanhGiaController : Controller
    {
        private readonly DataModel _context;

        public DanhGiaController(DataModel context)
        {
            _context = context;
        }

        // GET: DanhGia/Index
        public IActionResult Index()
        {
            var danhGias = _context.DanhGias
                .Include(d => d.Laptop)
                .Where(d => d.trangthai == true)
                .OrderByDescending(d => d.ngaydanhgia)
                .ToList();

            return View(danhGias);
        }

        // GET: DanhGia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Ensure id is not null
            if (id == null)
                return NotFound();

            // Find the DanhGia by id asynchronously
            var danhGia = await _context.DanhGias
                .FirstOrDefaultAsync(m => m.madanhgia == id);

            // If DanhGia does not exist, return NotFound
            if (danhGia == null)
                return NotFound();

            // If found, return the delete confirmation view with the DanhGia data
            return View(danhGia);
        }


        // POST: DanhGia/Delete/5
        [HttpPost]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Fetch the DanhGia object by id
                var danhGia = await _context.DanhGias
                    .FirstOrDefaultAsync(d => d.madanhgia == id);

                // If DanhGia exists, remove it from the database
                if (danhGia != null)
                {
                    _context.DanhGias.Remove(danhGia);
                    await _context.SaveChangesAsync(); // Use async save method to ensure no blocking
                    TempData["SuccessMessage"] = "Đánh giá đã được xóa thành công."; // Success message
                }
                else
                {
                    TempData["ErrorMessage"] = "Đánh giá không tồn tại."; // Error message if item not found
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception (optional)
                TempData["ErrorMessage"] = "Lỗi hệ thống khi xóa đánh giá. Vui lòng thử lại sau."; // Database error message
            }

            // Redirect to the index page after deletion
            return RedirectToAction(nameof(Index));
        }
    }
}
