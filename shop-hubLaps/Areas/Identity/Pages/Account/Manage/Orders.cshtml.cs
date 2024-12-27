using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Areas.Identity.Pages.Account.Manage
{
    public class OrdersModel : PageModel
    {
        private readonly UserManager<SampleUser> _userManager;
        private readonly DataModel _context;

        public OrdersModel(UserManager<SampleUser> userManager, DataModel context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ViewModel to pass data to Razor Page
        public UserOrderHistoryViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Lấy các đơn hàng từ cơ sở dữ liệu
            var orders = await _context.DonHangs
                .Include(o => o.ChiTietDonHangs)
                    .ThenInclude(ct => ct.Laptop)  // Kiểm tra chắc chắn rằng Laptop đã được bao gồm
                .Where(o => o.makh == user.Id)
                .OrderByDescending(o => o.ngaydat)
                .ToListAsync();

            // Khởi tạo ViewModel với các đơn hàng lấy được
            ViewModel = new UserOrderHistoryViewModel
            {
                DonHangs = orders ?? new List<DonHang>()  // Đảm bảo DonHangs không null
            };

            return Page();
        }
        public IActionResult OnPostOrderConfirmed()
        {
            // Sau khi đơn hàng được xác nhận, điều hướng người dùng đến trang Quản lý đơn hàng
            return RedirectToPage("/Identity/Account/Manage/Orders");
        }
    }
}
