using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using shop_hubLaps.Models;
using shop_hubLaps.Models.Momo;

namespace shop_hubLaps.Service
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);

        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
