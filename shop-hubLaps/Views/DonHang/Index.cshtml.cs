using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using shop_hubLaps.Models; // Đảm bảo bạn đang import namespace chứa lớp DonHang
using System.Threading.Tasks;

namespace shop_hubLaps.Views.DonHang
{
    public class IndexModel : PageModel
    {
        private readonly DataModel _context;

        public IndexModel(DataModel context)
        {
            _context = context;
        }

        [BindProperty]
        public shop_hubLaps.Models.DonHang DonHang { get; set; } // Sử dụng lớp DonHang từ namespace đã import

        public string Message { get; set; } // Thông báo cho người dùng

        // Phương thức gọi khi trang được truy cập lần đầu
        public void OnGet()
        {
            DonHang = new shop_hubLaps.Models.DonHang(); // Khởi tạo đối tượng DonHang mới
        }

        // Phương thức gọi khi form được gửi
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Kiểm tra tính hợp lệ của Model
            {
                return Page(); // Trả về trang hiện tại nếu có lỗi
            }

            // Thêm đối tượng DonHang vào context
            _context.DonHangs.Add(DonHang);

            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            Message = "Thêm đơn hàng thành công!"; // Thiết lập thông báo thành công
            return RedirectToPage(); // Chuyển hướng về trang này
        }
    }
}
