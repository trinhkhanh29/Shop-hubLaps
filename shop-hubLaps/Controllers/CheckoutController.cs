using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;
using shop_hubLaps.Models.Vnpay;
using shop_hubLaps.Service;
using shop_hubLaps.Service.Vnpay;

namespace shop_hubLaps.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly DataModel _context;
        private readonly UserManager<SampleUser> _userManager;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(IVnPayService vnPayService, DataModel context, UserManager<SampleUser> userManager, ILogger<CheckoutController> logger)
        {
            _vnPayService = vnPayService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // Returns the initial view for checkout
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Checkout(string PaymentMethod, string PaymentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/ThanhToan"));
            }

            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.Laptop)
                .FirstOrDefaultAsync(d => d.tinhtrang == "CART" && d.makh == user.Id);

            if (donHang == null || donHang.ChiTietDonHangs == null || !donHang.ChiTietDonHangs.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.";
                return RedirectToAction("Index", "Cart");
            }

            donHang.CapNhatGiaTri(); // Tính tổng giá trị đơn hàng
            _context.Update(donHang);
            await _context.SaveChangesAsync();

            return View(donHang); // Truyền thông tin đơn hàng sang view
        }

        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            // Gọi đến service để thực hiện xử lý kết quả thanh toán từ VnPay
            var response = _vnPayService.PaymentExecute(Request.Query);

            _logger.LogInformation($"VnPay Response: OrderId={response.OrderId}, Success={response.Success}, TransactionId={response.TransactionId}");

            if (response.Success)
            {
                try
                {
                    long OrderId = Convert.ToInt64(response.OrderId); // Convert to long for comparison

                    // Kiểm tra đơn hàng có tồn tại không
                    var donHang = await _context.DonHangs
                        .Include(d => d.ChiTietDonHangs)
                        .FirstOrDefaultAsync(d => d.madon == OrderId); // Ensure comparison is with long type

                    if (donHang != null)
                    {
                        // Cập nhật thông tin đơn hàng
                        donHang.thanhtoan = true;
                        donHang.tinhtrang = "PAID";
                        donHang.ngaydat = DateTime.Now;

                        // Cập nhật kho
                        foreach (var chiTiet in donHang.ChiTietDonHangs)
                        {
                            var laptop = await _context.Laptops.FindAsync(chiTiet.malaptop);
                            if (laptop != null && laptop.soluongton >= chiTiet.soluong)
                            {
                                laptop.soluongton -= chiTiet.soluong;
                            }
                            else
                            {
                                TempData["ErrorMessage"] = $"Sản phẩm {laptop.tenlaptop} không đủ số lượng trong kho.";
                                return RedirectToAction("Index", "Cart");
                            }
                        }

                        // Tạo đối tượng lưu thông tin VnPay vào cơ sở dữ liệu
                        var vnpayModel = new VnpayModel
                        {
                            OrderId = response.OrderId,
                            TransactionId = response.TransactionId,
                            PaymentMethod = "VnPay",
                            PaymentId = response.PaymentId,
                            OrderDescription = response.OrderDescription,
                            CreatedDate = DateTime.Now  // Set CreatedDate to the current time
                        };

                        // Lưu thông tin thanh toán VnPay vào cơ sở dữ liệu
                        _context.VnpayModels.Add(vnpayModel);
                        await _context.SaveChangesAsync(); // Save payment details

                        TempData["SuccessMessage"] = "Thông tin thanh toán VnPay đã được lưu vào cơ sở dữ liệu!";
                    }
                    else
                    {
                        // Log if the order was not found
                        _logger.LogError($"Order not found with OrderId: {response.OrderId}");
                        TempData["ErrorMessage"] = "Đơn hàng không hợp lệ.";
                    }
                }
                catch (OverflowException ex)
                {
                    _logger.LogError($"OverflowException: {ex.Message}");
                    TempData["ErrorMessage"] = "Giá trị đơn hàng quá lớn hoặc quá nhỏ để xử lý.";
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception: {ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình xử lý thanh toán.";
                }
            }
            else
            {
                _logger.LogError($"Payment failed: {response.OrderId}");
                TempData["ErrorMessage"] = "Thanh toán không thành công.";
            }

            return View("PaymentCallbackVnpay", response);
        }
    }
}
