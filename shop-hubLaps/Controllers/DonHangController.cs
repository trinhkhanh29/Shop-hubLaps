using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DonHangController : Controller
    {
        private readonly DataModel _context;

        public DonHangController(DataModel context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, string laptopFilter, string dateFilter, string statusFilter)
        {
            var query = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.Laptop)
                .AsQueryable();

            // Tìm kiếm theo Mã Đơn, Mã Khách Hàng, Tên Laptop hoặc Tên Hãng
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d =>
                    d.madon.ToString().Contains(searchTerm) ||
                    d.makh.Contains(searchTerm) ||
                    d.ChiTietDonHangs.Any(ct => ct.Laptop.tenlaptop.Contains(searchTerm)) ||  // Tìm theo Tên Laptop
                    d.ChiTietDonHangs.Any(ct => ct.Laptop.Hang.tenhang.Contains(searchTerm))    // Tìm theo Tên Hãng (assuming `Hang` is the brand)
                );
            }

            // Lọc theo Tên Laptop
            if (!string.IsNullOrEmpty(laptopFilter))
            {
                query = query.Where(d => d.ChiTietDonHangs.Any(ct => ct.Laptop.tenlaptop == laptopFilter));
            }

            // Lọc theo Ngày
            if (dateFilter == "newest")
            {
                query = query.OrderByDescending(d => d.ngaydat);
            }
            else if (dateFilter == "oldest")
            {
                query = query.OrderBy(d => d.ngaydat);
            }

            // Lọc theo Tình Trạng
            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = query.Where(d => d.tinhtrang == statusFilter);
            }

            var result = await query.ToListAsync();

            // Lấy danh sách tên laptop cho ViewData
            ViewData["Laptops"] = await _context.Laptops.Select(l => l.tenlaptop).Distinct().ToListAsync();

            return View(result);
        }

        // GET: DonHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(m => m.madon == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHang/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("madon,makh,gia,tinhtrang,ngaydat,ngaygiao,thanhtoan,giaohang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }

        // GET: DonHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ.";
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                TempData["ErrorMessage"] = "Đơn hàng không tồn tại.";
                return NotFound();
            }
            return View(donHang);
        }

        // POST: DonHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("madon,makh,gia,tinhtrang,ngaydat,ngaygiao,thanhtoan,giaohang")] DonHang donHang)
        {
            if (id != donHang.madon)
            {
                TempData["ErrorMessage"] = "ID đơn hàng không khớp.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật đơn hàng thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.madon))
                    {
                        TempData["ErrorMessage"] = "Đơn hàng không tồn tại khi cập nhật.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật.";
                        throw;
                    }
                }
            }

            // Nếu Model không hợp lệ, quay lại cùng thông báo lỗi
            TempData["ErrorMessage"] = "Có lỗi trong dữ liệu gửi lên.";
            return View(donHang);
        }


        // GET: DonHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ.";
                return RedirectToAction(nameof(Index));  // Điều hướng về trang Index nếu ID không hợp lệ
            }

            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(m => m.madon == id);

            if (donHang == null)
            {
                TempData["ErrorMessage"] = "Đơn hàng không tồn tại.";
                return RedirectToAction(nameof(Index));  // Điều hướng về trang Index nếu không tìm thấy đơn hàng
            }

            return View(donHang);  // Trả về view với dữ liệu đơn hàng
        }

        // POST: DonHang/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var donHang = await _context.DonHangs.FindAsync(id);  // Tìm đơn hàng theo id

                if (donHang != null)
                {
                    _context.DonHangs.Remove(donHang);  // Thực hiện xóa đơn hàng
                    await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
                    TempData["SuccessMessage"] = "Đơn hàng đã được xóa thành công.";  // Thông báo thành công
                }
                else
                {
                    TempData["ErrorMessage"] = "Đơn hàng không tồn tại khi xóa.";  // Thông báo lỗi nếu không tìm thấy đơn hàng
                }
            }
            catch (Exception ex)
            {
                // Xử lý trường hợp lỗi
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa: " + ex.Message;  // Thông báo lỗi chi tiết
            }

            return RedirectToAction(nameof(Index));  // Quay lại trang Index sau khi xóa hoặc gặp lỗi
        }




        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.madon == id);
        }
    }
}
