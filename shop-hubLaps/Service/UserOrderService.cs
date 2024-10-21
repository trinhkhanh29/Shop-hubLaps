using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;

namespace shop_hubLaps.Service
{
    public class UserOrderService
    {
        private readonly DataModel _context;

        public UserOrderService(DataModel context)
        {
            _context = context;
        }

        public async Task<List<DonHang>> GetOrdersByUserMakhAsync(string makh)
        {
            return await _context.DonHangs
                .Where(o => o.makh == makh)
                .ToListAsync();
        }
    }
}
