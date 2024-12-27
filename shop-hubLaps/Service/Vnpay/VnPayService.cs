using shop_hubLaps.Library;
using shop_hubLaps.Models.Vnpay;

namespace shop_hubLaps.Service.Vnpay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<VnPayService> _logger;

        public VnPayService(IConfiguration configuration, ILogger<VnPayService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneId = _configuration["TimeZoneId"];

            if (string.IsNullOrEmpty(timeZoneId))
            {
                _logger.LogError("TimeZoneId is missing or empty in the configuration.");
                throw new ArgumentNullException("TimeZoneId", "TimeZoneId cannot be null or empty.");
            }
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"] ?? throw new ArgumentNullException("PaymentCallBack:ReturnUrl");

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"] ?? throw new ArgumentNullException("Vnpay:Version"));
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"] ?? throw new ArgumentNullException("Vnpay:Command"));
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"] ?? throw new ArgumentNullException("Vnpay:TmnCode"));
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"] ?? throw new ArgumentNullException("Vnpay:CurrCode"));
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"] ?? throw new ArgumentNullException("Vnpay:Locale"));
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(
                _configuration["Vnpay:BaseUrl"] ?? throw new ArgumentNullException("Vnpay:BaseUrl"),
                _configuration["Vnpay:HashSecret"] ?? throw new ArgumentNullException("Vnpay:HashSecret")
            );

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(
                collections,
                _configuration["Vnpay:HashSecret"] ?? throw new ArgumentNullException("Vnpay:HashSecret")
            );

            return response;
        }
    }
}
