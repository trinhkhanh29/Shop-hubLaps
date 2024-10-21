using shop_hubLaps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shop_hubLaps.Service
{
    public interface IOrderService
    {
        Task<List<DonHang>> GetOrdersByUserAsync(string makh); // Sửa kiểu bool thành string
    }
}
