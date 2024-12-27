using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;
using shop_hubLaps.Service;
using shop_hubLaps.Service.Vnpay;

namespace shop_hubLaps.Controllers
{

    public class CheckoutController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public CheckoutController(IVnPayService vnPayService)

        {
            _vnPayService = vnPayService;

        }

        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }

    }
}
