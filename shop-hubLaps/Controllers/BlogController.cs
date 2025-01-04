using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataModel _context;

        public BlogController(DataModel context)
        {
            _context = context;
        }

        // Hiển thị tất cả bài viết đã xuất bản
        public async Task<IActionResult> Index()
        {
            var tinTucs = await _context.TinTucs
                .Where(t => t.xuatban == true)
                .Include(t => t.ChuDe)
                .OrderByDescending(t => t.ngaycapnhat)
                .ToListAsync();

            return View(tinTucs);
        }

        // Hiển thị chi tiết bài viết
        public async Task<IActionResult> Details(int id)
        {
            var tinTuc = await _context.TinTucs
                .Include(t => t.ChuDe) // Load thông tin chủ đề
                .FirstOrDefaultAsync(t => t.matin == id && t.xuatban == true);

            if (tinTuc == null)
            {
                return NotFound();
            }

            // Tăng lượt xem
            tinTuc.luotxem = (tinTuc.luotxem ?? 0) + 1;
            await _context.SaveChangesAsync();

            return View(tinTuc);
        }
    }
}
