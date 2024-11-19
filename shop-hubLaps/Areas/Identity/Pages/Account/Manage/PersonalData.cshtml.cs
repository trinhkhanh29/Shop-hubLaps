using System;
using System.IO;
using System.Threading.Tasks;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using shop_hubLaps.Areas.Identity.Data;

namespace shop_hubLaps.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<SampleUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<SampleUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostDownloadPersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            using (var stream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(stream))
                using (var pdf = new PdfDocument(pdfWriter))
                {
                    // Tạo tài liệu
                    Document document = new Document(pdf);

                    // Đường dẫn font chữ (đảm bảo font chữ này hỗ trợ tiếng Việt)
                    var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "YourFont.ttf");
                    var customFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.CP1252, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                    // Thêm tiêu đề
                    document.Add(new Paragraph("Dữ liệu cá nhân của bạn")
                        .SetFont(customFont)
                        .SetFontSize(20)
                        .SetBold()
                        .SetMarginBottom(10));

                    // Thêm tiêu đề cho thông tin chi tiết
                    document.Add(new Paragraph("Thông tin chi tiết:")
                        .SetFont(customFont)
                        .SetFontSize(16)
                        .SetBold()
                        .SetMarginBottom(5));

                    // Tạo bảng với 2 cột
                    var table = new Table(new float[] { 1, 3 });
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Đặt chiều rộng là 100%

                    // Danh sách thông tin người dùng
                    var userData = new (string Title, string Value)[]
                    {
                        ("Họ:", user.FirstName ?? "Không có dữ liệu"),
                        ("Tên:", user.LastName ?? "Không có dữ liệu"),
                        ("Tên người dùng:", user.UserName ?? "Không có dữ liệu"),
                        ("Email:", user.Email ?? "Không có dữ liệu"),
                        ("Số điện thoại:", user.PhoneNumber ?? "Không có dữ liệu"),
                        ("Địa chỉ:", user.DiaChi ?? "Không có dữ liệu"),
                        ("Họ tên:", user.HoTen ?? "Không có dữ liệu"),
                        ("Ngày sinh:", user.NgaySinh?.ToString("d") ?? "Không có dữ liệu"),
                        ("Avatar:", user.Avatar ?? "Không có dữ liệu")
                    };

                    // Thêm hàng vào bảng
                    foreach (var (title, value) in userData)
                    {
                        table.AddHeaderCell(new Cell().Add(new Paragraph(title).SetFont(customFont)));
                        table.AddCell(new Cell().Add(new Paragraph(value).SetFont(customFont)));
                    }

                    // Thêm bảng vào tài liệu
                    document.Add(table);

                    document.Close();
                }

                var fileName = "PersonalData.pdf";
                return File(stream.ToArray(), "application/pdf", fileName);
            }
        }
    }
}
