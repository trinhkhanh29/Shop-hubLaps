using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using shop_hubLaps.Models;
using shop_hubLaps.Service;
using shop_hubLaps.Service.Vnpay;

namespace shop_hubLaps.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IMomoService _momoService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IMomoService momoService, IVnPayService vnPayService)
        {
            _momoService = momoService;
            _vnPayService = vnPayService;

        }

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        [Route("PaymentCallBack")]
        public IActionResult PaymentCallBack(IQueryCollection query)
        {
            var result = _momoService.PaymentExecuteAsync(query);
            return View(result);  // Hiển thị kết quả thanh toán
        }
    }

}
