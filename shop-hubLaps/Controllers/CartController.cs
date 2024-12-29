using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Extensions;
using shop_hubLaps.Models;
using shop_hubLaps.Models.Vnpay;
using shop_hubLaps.Service;
using shop_hubLaps.Service.Vnpay;
using shop_hubLaps.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    public class CartController : Controller
    {
        private readonly string _secretKey = "sk_test_51Qai30ALvOyqlJWEdpEyLPmsbYliTfDoby8Mfcic5G7DlS2rkE4LaGwDUtMGcDdtmjLXUPvAya00kz7kZ1X7flTc00LS9vhx20";

        private readonly DataModel _context;

        private readonly IUserService _userService;

        private readonly IMomoService _momoService;

        private readonly IVnPayService _vnPayService;

        private readonly UserManager<SampleUser> _userManager;

        private readonly ILogger<CartController> _logger;

        // Constructor với UserManager và IUserService
        public CartController(DataModel context, IUserService userService, IMomoService momoService, IVnPayService vnPayService,
                              UserManager<SampleUser> userManager, ILogger<CartController> logger)
        {
            _context = context;

            _userService = userService;

            _userManager = userManager;

            _momoService = momoService;

            _vnPayService = vnPayService;

            _logger = logger;
        }

        // Lấy giỏ hàng của người dùng
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));
            }

            var userId = user?.Id;
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.Laptop)
                .FirstOrDefaultAsync(d => d.makh == userId && d.tinhtrang == "CART");

            if (donHang == null)
            {
                donHang = new DonHang
                {
                    makh = userId,
                    tinhtrang = "CART",
                    ChiTietDonHangs = new List<ChiTietDonHang>()
                };
                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();
            }
            // Cập nhật số lượng trong giỏ hàng từ database
            ViewBag.Tongsoluong = donHang.ChiTietDonHangs.Sum(ct => ct.soluong);

            // Trả về View với giỏ hàng
            return View(donHang);
        }

        [Route("Cart/Add/{malaptop}")]
        public async Task<IActionResult> Add(int malaptop, decimal dongia, int soluong = 1, string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));
            }

            var userId = user?.Id;
            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(d => d.makh == userId && d.tinhtrang == "CART");

            if (donHang == null)
            {
                donHang = new DonHang
                {
                    makh = userId,
                    tinhtrang = "CART",
                    gia = 0
                };
                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();
            }

            var chiTiet = await _context.ChiTietDonHangs
                .FirstOrDefaultAsync(ct => ct.madon == donHang.madon && ct.malaptop == malaptop);

            if (chiTiet != null)
            {
                chiTiet.soluong += soluong;  // Cộng số lượng sản phẩm
            }
            else
            {
                chiTiet = new ChiTietDonHang
                {
                    madon = donHang.madon,
                    malaptop = malaptop,
                    dongia = dongia,  // Đảm bảo giá trị này được gán đúng
                    soluong = soluong
                };
                _context.ChiTietDonHangs.Add(chiTiet);
            }

            donHang.CapNhatGiaTri();  // Cập nhật lại giá trị tổng cộng cho đơn hàng
            await _context.SaveChangesAsync();

            HttpContext.Session.SetObject("Cart", donHang.ChiTietDonHangs);  // Lưu giỏ hàng vào session

            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng thành công!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Increase(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));

            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(d => d.makh == user.Id && d.tinhtrang == "CART");

            if (donHang == null) return NotFound();

            var chiTiet = donHang.ChiTietDonHangs.FirstOrDefault(ct => ct.malaptop == id);
            var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.malaptop == id); // Lấy laptop từ bảng Laptop

            if (laptop != null && chiTiet != null)
            {
                // Kiểm tra xem số lượng tồn kho có đủ không
                if (chiTiet.soluong < laptop.soluongton)
                {
                    chiTiet.soluong += 1; // Tăng số lượng lên 1
                    donHang.CapNhatGiaTri(); // Cập nhật lại tổng tiền giỏ hàng
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Số lượng sản phẩm đã được cập nhật!";

                }
                else
                {
                    // Thông báo hết hàng
                    TempData["ErrorMessage"] = "Số lượng yêu cầu vượt quá số lượng tồn kho!";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Decrease(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));

            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(d => d.makh == user.Id && d.tinhtrang == "CART");

            if (donHang == null) return NotFound();

            var chiTiet = donHang.ChiTietDonHangs.FirstOrDefault(ct => ct.malaptop == id);
            var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.malaptop == id); // Lấy laptop từ bảng Laptop

            if (laptop != null && chiTiet != null)
            {
                // Kiểm tra số lượng tồn kho trước khi giảm
                if (chiTiet.soluong > 1)
                {
                    chiTiet.soluong -= 1; // Giảm số lượng xuống 1
                    donHang.CapNhatGiaTri(); // Cập nhật lại tổng tiền giỏ hàng
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Số lượng sản phẩm đã được cập nhật!";

                }
                else
                {
                    TempData["ErrorMessage"] = "Số lượng không thể giảm xuống dưới 1!";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));

            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(d => d.makh == user.Id && d.tinhtrang == "CART");

            if (donHang == null) return NotFound();

            var chiTiet = donHang.ChiTietDonHangs.FirstOrDefault(ct => ct.malaptop == id);
            var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.malaptop == id); // Lấy laptop từ bảng Laptop

            if (laptop != null && chiTiet != null && quantity > 0)
            {
                // Kiểm tra số lượng yêu cầu có vượt quá số lượng tồn kho không
                if (quantity <= laptop.soluongton)
                {
                    chiTiet.soluong = quantity; // Cập nhật số lượng
                    donHang.CapNhatGiaTri(); // Cập nhật lại tổng tiền giỏ hàng
                    await _context.SaveChangesAsync();

                    // Thêm thông báo thành công
                    TempData["SuccessMessage"] = "Số lượng sản phẩm đã được cập nhật!";
                }
                else
                {
                    // Thông báo hết hàng
                    TempData["ErrorMessage"] = "Số lượng yêu cầu vượt quá số lượng tồn kho!";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Số lượng phải lớn hơn 0!";
            }

            return RedirectToAction("Index");
        }
       
        [HttpPost]
        public async Task<IActionResult> XoaSanPham(int malaptop)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect(Url.Content("~/Identity/Account/Login?returnUrl=/Cart/Index"));

            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(d => d.makh == user.Id && d.tinhtrang == "CART");

            if (donHang == null) return NotFound();

            var chiTiet = donHang.ChiTietDonHangs.FirstOrDefault(ct => ct.malaptop == malaptop);
            if (chiTiet != null)
            {
                _context.ChiTietDonHangs.Remove(chiTiet);
                donHang.CapNhatGiaTri();
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult XacNhanDonHang(DonHang model)
        {
            // Process the order details, save to the database, etc.
            return RedirectToAction("ThanhToan");
        }

        public async Task<IActionResult> ThanhToan()
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

        [HttpPost]
        public async Task<IActionResult> ThanhToanConfirm(string paymentMethod, string Name, decimal Amount, string OrderDescription, string OrderType, string stripeToken, decimal amount, DateTime CreatedDate)
        {
            try
            {
                var donHang = await GetUserCartAsync();
                if (donHang == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy giỏ hàng.";
                    return RedirectToAction("Index", "Cart");
                }

                donHang.ngaydat = DateTime.Now;
                donHang.PhuongThucThanhToan = paymentMethod;

                // Update order details
                _context.DonHangs.Update(donHang);
                await _context.SaveChangesAsync();

                // Ensure valid payment method
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    TempData["ErrorMessage"] = "Phương thức thanh toán không hợp lệ.";
                    return RedirectToAction("Index", "Cart");
                }

                // Process payment method
                switch (paymentMethod)
                {
                    case "COD":
                        donHang.thanhtoan = false;
                        donHang.tinhtrang = "PROCESSING";
                        break;
                    case "Vnpay":
                        donHang.thanhtoan = false;
                        donHang.tinhtrang = "PENDING";
                        break;
                    case "Stripe":
                        donHang.thanhtoan = false;
                        donHang.tinhtrang = "PENDING";
                        break;
                    default:
                        TempData["ErrorMessage"] = "Phương thức thanh toán không hợp lệ.";
                        return RedirectToAction("Index", "Cart");
                }

               

                // Handle inventory update, ensuring no null value for soluongton
                foreach (var chiTiet in donHang.ChiTietDonHangs)
                {
                    var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.malaptop == chiTiet.malaptop);
                    if (laptop != null)
                    {
                        // Ensure soluongton is not null and update the inventory
                        if (laptop.soluongton.HasValue)
                        {
                            laptop.soluongton -= chiTiet.soluong; // Decrease inventory
                            _context.Laptops.Update(laptop);
                        }
                        else
                        {
                            _logger.LogError($"Số lượng tồn kho cho laptop {laptop.malaptop} không hợp lệ.");
                            TempData["ErrorMessage"] = $"Lỗi với tồn kho của laptop {laptop.malaptop}.";
                            return RedirectToAction("Index", "Cart");
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // Redirect based on payment method
                if (paymentMethod == "Vnpay")
                {
                    return RedirectToAction("CreatePaymentUrlVnpay", "Cart", new
                    {
                        orderId = donHang.madon,
                        Name,
                        Amount = Amount > 0 ? Amount : 0,  // Ensure Amount is not zero or negative
                        OrderDescription = OrderDescription ?? string.Empty, // Use empty string if null
                        OrderType = OrderType ?? string.Empty, // Use empty string if null
                        CreatedDate = CreatedDate == default ? DateTime.Now : CreatedDate // Use current date if CreatedDate is default
                    });
                }

                if (paymentMethod == "Stripe" && !string.IsNullOrEmpty(stripeToken))
                {
                    try
                    {
                        // Initialize Stripe API with your secret key
                        StripeConfiguration.ApiKey = "sk_test_51Qai30ALvOyqlJWEv04kz1Anob3IWy11u26wGQYtoWRmS1GbNEpXR6b2S7rRoTgtI9AmQC2khe51yHJQ6GFXIfqB00m2RVYN40";

                        var amountInCents = (long)(amount * 100); // Convert amount to cents (Stripe requires cents)

                        var chargeOptions = new ChargeCreateOptions
                        {
                            Amount = amountInCents,
                            Currency = "vnd",
                            Source = stripeToken,
                            Description = "Thanh toán đơn hàng"
                        };

                        var chargeService = new ChargeService();
                        var charge = await chargeService.CreateAsync(chargeOptions);

                        if (charge.Status == "succeeded")
                        {
                            donHang.tinhtrang = "PAID";
                            donHang.thanhtoan = true;
                            _context.DonHangs.Update(donHang);
                            await _context.SaveChangesAsync();

                            TempData["SuccessMessage"] = "Thanh toán thành công qua Stripe!";
                            return RedirectToAction("ThanhToanThanhCong");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Thanh toán không thành công qua Stripe. Vui lòng thử lại!";
                            return RedirectToAction("ThanhToanThatBai");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi trong quá trình thanh toán với Stripe");
                        TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình thanh toán qua Stripe.";
                        return RedirectToAction("ThanhToanThatBai");
                    }
                }

                return RedirectToAction("OrderConfirmed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong quá trình xử lý thanh toán.");
                TempData["ErrorMessage"] = $"Lỗi thanh toán: {ex.Message}";
                return RedirectToAction("Index", "Cart");
            }
        }

        public IActionResult OrderConfirmed()
            {
                return View();
            }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
            {
                var response = await _momoService.CreatePaymentAsync(model);
                return Redirect(response.PayUrl);
            }


        public IActionResult PaymentCallback()
        {
            var paymentResult = _vnPayService.PaymentExecute(HttpContext.Request.Query);
            if (paymentResult.Success)
            {
                var donHang = _context.DonHangs.FirstOrDefault(d => d.madon.ToString() == paymentResult.OrderId);

                if (donHang == null)
                {
                    TempData["ErrorMessage"] = "Đơn hàng không hợp lệ.";
                    return RedirectToAction("Index", "Cart");
                }

                // Cập nhật trạng thái đơn hàng thành PAID
                donHang.tinhtrang = "PAID";
                donHang.thanhtoan = true;
                _context.DonHangs.Update(donHang);
                _context.SaveChanges();

                // Create a new VnpayModel to store payment information
                var vnpayTransaction = new VnpayModel
                {
                    OrderDescription = paymentResult.OrderDescription,
                    TransactionId = paymentResult.TransactionId,
                    OrderId = paymentResult.OrderId,
                    PaymentMethod = paymentResult.PaymentMethod,
                    PaymentId = paymentResult.PaymentId,
                    CreatedDate = DateTime.Now,
                    DonHang = donHang
                };

                // Save VnpayTransaction details to database
                _context.VnpayModels.Add(vnpayTransaction);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thanh toán thành công!";
                return RedirectToAction("OrderConfirmed");
            }
            else
            {
                TempData["ErrorMessage"] = "Thanh toán không thành công.";
                return RedirectToAction("Index", "Cart");
            }
        }



        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> StripePayment(string stripeToken, decimal amount)
        {
            try
            {
                // Initialize Stripe API with the Secret Key
                StripeConfiguration.ApiKey = _secretKey;

                // Convert amount from VND to cents
                var amountInCents = (long)(amount * 100); // Convert VND to cents by multiplying by 100

                // Check if amount exceeds the maximum allowed by Stripe (₫99,999,999 or 999,999,990 cents)
                if (amountInCents > 999999990)
                {
                    TempData["ErrorMessage"] = "Số tiền thanh toán không được vượt quá ₫99,999,999.";
                    return RedirectToAction("ThanhToanThatBai");
                }

                // Create the charge options
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = amountInCents,  // Amount should be in cents, so multiply by 100
                    Currency = "vnd",        // Currency used (Vietnam Dong)
                    Description = "Thanh toán đơn hàng", // Description of the order
                    Source = stripeToken,    // Stripe token from client-side
                };

                // Create a charge service
                var chargeService = new ChargeService();
                Charge charge = await chargeService.CreateAsync(chargeOptions);

                // Check the status of the charge
                if (charge.Status == "succeeded")
                {
                    // If payment is successful, update the order status and redirect to success page
                    var donHang = await GetUserCartAsync();
                    donHang.tinhtrang = "PAID";
                    donHang.thanhtoan = true;
                    _context.DonHangs.Update(donHang);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thanh toán thành công!";
                    return RedirectToAction("ThanhToanThanhCong");
                }
                else
                {
                    // If payment failed, update order status and redirect to failure page
                    TempData["ErrorMessage"] = "Thanh toán không thành công, vui lòng thử lại!";
                    return RedirectToAction("ThanhToanThatBai");
                }
            }
            catch (StripeException ex)
            {
                // Handle any errors from Stripe (e.g., network issues, invalid token)
                _logger.LogError(ex, "Lỗi trong quá trình thanh toán với Stripe");
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình thanh toán qua Stripe: " + ex.Message;
                return RedirectToAction("ThanhToanThatBai");
            }
        }

        public IActionResult ThanhToanThanhCong()
        {
            return View();
        }

        public IActionResult ThanhToanThatBai()
        {
            return View();
        }

        private async Task<DonHang?> GetUserCartAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            // Find an existing cart for the current user
            return await _context.DonHangs
                .Include(d => d.ChiTietDonHangs) // Include related data
                .FirstOrDefaultAsync(d => d.makh == userId && d.tinhtrang == "CART"); // Ensure correct status
        }

    }
}