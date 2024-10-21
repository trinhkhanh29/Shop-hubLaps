using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Service
{
    public class OrderService : IOrderService
    {
        private readonly DataModel _context;

        public OrderService(DataModel context)
        {
            _context = context;
        }

        public async Task<List<DonHang>> GetOrdersByUserAsync(string makh) // Đảm bảo kiểu dữ liệu là string
        {
            var orders = await _context.DonHangs
                .Where(o => o.makh == makh) // So sánh chuỗi
                .ToListAsync();


            return orders;
        }
    }
}
