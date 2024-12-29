using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;
using System.Linq;
using System.Threading.Tasks;
using iText.IO.Font.Constants;
using iText.IO.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Kernel.Pdf.Xobject;
using iText.IO.Image;
using iText.IO.Util;
using static System.Net.Mime.MediaTypeNames;


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


        public async Task<IActionResult> OnGetDownloadOrderPdfAsync(int orderId)
        {
            // Lấy thông tin đơn hàng từ cơ sở dữ liệu
            var order = await _context.DonHangs
                .Include(o => o.ChiTietDonHangs)
                .ThenInclude(ct => ct.Laptop)
                .FirstOrDefaultAsync(o => o.madon == orderId);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
                return RedirectToPage();
            }
            // Lấy thông tin khách hàng từ Identity
            var customer = await _userManager.FindByIdAsync(order.makh);  // Tìm người dùng qua makh (userId)
            var customerName = customer?.HoTen ?? "Khách hàng không xác định"; // Lấy tên khách hàng từ Identity, nếu không có thì mặc định "Khách hàng không xác định"

            // Phương thức thanh toán của đơn hàng
            var paymentMethod = order.PhuongThucThanhToan ?? "Chưa chọn phương thức thanh toán"; // Phương thức thanh toán, nếu không có thì mặc định là "Chưa chọn phương thức thanh toán"

            using (var memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer); // Đã sửa lỗi khai báo trùng tên biến writer
                var document = new Document(pdf); // Đảm bảo dùng tên đúng là document


                // Đường dẫn đến font Lexend đã tải về (đặt file trong thư mục wwwroot/fonts)
                string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/static/Lexend-Regular.ttf");

                // Đăng ký font Lexend
                var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                // Cấu hình font mặc định
                document.SetFont(font);

                // Đường dẫn tới logo
                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/hubLaps.png");

                // Tạo PdfImageXObject từ hình ảnh
                var imageData = ImageDataFactory.Create(logoPath); // Sử dụng ImageDataFactory để tạo ảnh từ đường dẫn
                PdfImageXObject xObject = new PdfImageXObject(imageData);

                // Đặt chiều rộng và chiều cao cho ảnh
                float imageWidth = 250;  // Chiều rộng ảnh (đơn vị tính là điểm)
                float imageHeight = 50; // Chiều cao ảnh (đơn vị tính là điểm)

                // Thêm hình ảnh vào tài liệu
                var image = new iText.Layout.Element.Image(xObject) // Sử dụng đầy đủ tên lớp Image
                    .SetWidth(imageWidth)    // Đặt chiều rộng
                    .SetHeight(imageHeight)  // Đặt chiều cao
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
                document.Add(image); // Dùng document thay vì doc

                // Thêm đường kẻ ngang
                document.Add(new Paragraph().SetBorderBottom(new SolidBorder(1)));


                // Thêm tiêu đề
                document.Add(new Paragraph("Thông Tin Đơn Hàng").SetFontSize(20).SetBold());

                // Thêm thông tin khách hàng và phương thức thanh toán
                document.Add(new Paragraph($"Tên Khách Hàng: {customerName}"));
                document.Add(new Paragraph($"Phương Thức Thanh Toán: {paymentMethod}"));

                // Thêm thông tin đơn hàng
                document.Add(new Paragraph($"Mã Đơn Hàng: {order.madon}"));
                document.Add(new Paragraph($"Ngày Đặt: {order.ngaydat?.ToString("dd/MM/yyyy HH:mm")}"));
                document.Add(new Paragraph($"Tổng Tiền: {order.gia.ToString("C0", new System.Globalization.CultureInfo("vi-VN"))}"));
                document.Add(new Paragraph($"Trạng Thái: {order.tinhtrang}"));

                // Thêm danh sách sản phẩm dưới dạng bảng
                document.Add(new Paragraph("\nChi Tiết Sản Phẩm:").SetBold());

                // Tạo bảng với số cột cần thiết (ví dụ: 4 cột cho tên laptop, số lượng, đơn giá và thành tiền)
                float[] columnWidths = { 2f, 1f, 1f, 1f };  // Điều chỉnh chiều rộng của các cột (tên laptop, số lượng, đơn giá, thành tiền)
                Table table = new Table(columnWidths);

                // Thêm tiêu đề cho các cột của bảng và căn giữa chữ
                table.AddHeaderCell("Tên Laptop").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                table.AddHeaderCell("Số Lượng").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                table.AddHeaderCell("Đơn Giá").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                table.AddHeaderCell("Thành Tiền").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                foreach (var detail in order.ChiTietDonHangs)
                {
                    // Thêm thông tin sản phẩm vào bảng và căn giữa chữ
                    table.AddCell(detail.Laptop.tenlaptop).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);  // Tên laptop
                    table.AddCell(detail.soluong.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER); // Số lượng
                    table.AddCell(detail.dongia.ToString("C0", new System.Globalization.CultureInfo("vi-VN"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER); // Đơn giá
                    table.AddCell((detail.soluong * detail.dongia).ToString("C0", new System.Globalization.CultureInfo("vi-VN"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER); // Thành tiền
                }

                // Đặt chiều rộng cho các cột theo tỷ lệ
                table.SetWidth(UnitValue.CreatePercentValue(100));  // Đặt tỷ lệ chiều rộng của bảng là 100%

                document.Add(table);

                document.Add(new Paragraph("GIÁM ĐỐC")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetBold()
                    .SetMarginRight(37)
                    .SetMarginTop(5));  // Giảm khoảng cách trên (tùy chỉnh giá trị)

                document.Add(new Paragraph("(Đã ký)")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetBold()
                    .SetMarginRight(39)
                    .SetMarginTop(5));  // Giảm khoảng cách trên

                document.Add(new Paragraph("Trịnh Quốc Khánh")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetBold()
                    .SetMarginRight(20)
                    .SetMarginTop(5));  // Giảm khoảng cách trên

                // Thêm đường kẻ ngang
                document.Add(new Paragraph().SetBorderBottom(new SolidBorder(1)));

                // Địa chỉ
                document.Add(new Paragraph("ĐC: Thịnh Liệt Hoàng Mai Hà Nội")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginRight(20)
                    .SetMarginTop(5));  // Giảm khoảng cách trên


                // Đóng tài liệu
                document.Close();

                // Trả về file PDF để tải xuống
                return File(memoryStream.ToArray(), "application/pdf", $"DonHang_{order.madon}.pdf");
            }
        }
    }
}
