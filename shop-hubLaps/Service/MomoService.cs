using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using shop_hubLaps.Models;
using shop_hubLaps.Models.Momo;
using shop_hubLaps.Service;

namespace shop_hubLaps.Service
{
    public class MomoService : IMomoService
    {
        //private readonly IOptions<MomoOptionModel> _options;
        //public MomoService(IOptions<MomoOptionModel> options)
        //{
        //    _options = options;
        //}
        //public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        //{
        //    model.OrderId = DateTime.UtcNow.Ticks.ToString();
        //    model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;
        //    var rawData =
        //        $"partnerCode={_options.Value.PartnerCode}" +
        //        $"&accessKey={_options.Value.AccessKey}" +
        //        $"&requestId={model.OrderId}" +
        //        $"&amount={model.Amount}" +
        //        $"&orderId={model.OrderId}" +
        //        $"&orderInfo={model.OrderInfo}" +
        //        $"&returnUrl={_options.Value.ReturnUrl}" +
        //        $"&notifyUrl={_options.Value.NotifyUrl}" +
        //        $"&extraData=";

        //    var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

        //    var client = new RestClient(_options.Value.MomoApiUrl);
        //    var request = new RestRequest() { Method = Method.Post };
        //    request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        //    // Create an object representing the request data
        //    var requestData = new
        //    {
        //        accessKey = _options.Value.AccessKey,
        //        partnerCode = _options.Value.PartnerCode,
        //        requestType = _options.Value.RequestType,
        //        notifyUrl = _options.Value.NotifyUrl,
        //        returnUrl = _options.Value.ReturnUrl,
        //        orderId = model.OrderId,
        //        amount = model.Amount.ToString(),
        //        orderInfo = model.OrderInfo,
        //        requestId = model.OrderId,
        //        extraData = "",
        //        signature = signature
        //    };

        //    request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        //    var response = await client.ExecuteAsync(request);
        //    var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        //    return momoResponse;

        //}





        //public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        //{
        //    var amount = collection.First(s => s.Key == "amount").Value;
        //    var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
        //    var orderId = collection.First(s => s.Key == "orderId").Value;

        //    return new MomoExecuteResponseModel()
        //    {
        //        Amount = amount,
        //        OrderId = orderId,
        //        OrderInfo = orderInfo

        //    };
        //}

        //private string ComputeHmacSha256(string message, string secretKey)
        //{
        //    var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        //    var messageBytes = Encoding.UTF8.GetBytes(message);

        //    byte[] hashBytes;

        //    using (var hmac = new HMACSHA256(keyBytes))
        //    {
        //        hashBytes = hmac.ComputeHash(messageBytes);
        //    }

        //    var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        //    return hashString;
        //}
        private const string Endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        private const string PartnerCode = "MOMO";
        private const string AccessKey = "F8BBA842ECF85";
        private const string SecretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
        private const string ReturnUrl = "https://localhost:7068/Cart/PaymentCallBack";
        private const string NotifyUrl = "https://localhost:7068/Cart/Notify";
        private const string RequestType = "captureMoMoWallet";

        public async Task<MomoResponse> CreatePaymentAsync(OrderInfoModel model)
        {
            string orderId = DateTime.Now.Ticks.ToString();
            string requestId = orderId;
            string rawHash = $"partnerCode={PartnerCode}&accessKey={AccessKey}&requestId={requestId}&amount={model.Amount}&orderId={orderId}&orderInfo={model.OrderInfo}&returnUrl={ReturnUrl}&notifyUrl={NotifyUrl}&extraData=";

            string signature = GenerateSignature(rawHash, SecretKey);

            var requestPayload = new JObject
        {
            { "partnerCode", PartnerCode },
            { "accessKey", AccessKey },
            { "requestId", requestId },
            { "amount", model.Amount.ToString() },
            { "orderId", orderId },
            { "orderInfo", model.OrderInfo },
            { "returnUrl", ReturnUrl },
            { "notifyUrl", NotifyUrl },
            { "extraData", "" },
            { "requestType", "captureMoMoWallet" },
            { "signature", signature }
        };

            using var client = new HttpClient();
            var content = new StringContent(requestPayload.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return new MomoResponse
                {
                    ErrorCode = -1,
                    Message = "Failed to connect to MoMo API."
                };
            }

            var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            return new MomoResponse
            {
                PayUrl = jsonResponse["payUrl"]?.ToString(),
                ErrorCode = int.Parse(jsonResponse["errorCode"]?.ToString() ?? "-1"),
                Message = jsonResponse["localMessage"]?.ToString()
            };
        }

        private static string GenerateSignature(string rawHash, string secretKey)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        Task<MomoCreatePaymentResponseModel> IMomoService.CreatePaymentAsync(OrderInfoModel model)
        {
            throw new NotImplementedException();
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}
