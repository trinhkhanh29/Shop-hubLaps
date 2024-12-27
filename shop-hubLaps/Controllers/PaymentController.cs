using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using shop_hubLaps.Models;
using shop_hubLaps.Service;

namespace shop_hubLaps.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IMomoService _momoService;

        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl); // Điều hướng đến URL thanh toán MoMo
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
