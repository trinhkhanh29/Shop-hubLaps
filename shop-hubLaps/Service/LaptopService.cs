using shop_hubLaps.Models;
using System.Collections.Generic;
using System.Linq;

namespace shop_hubLaps.Services
{
    public class LaptopService : ILaptopService
    {
        private readonly DataModel _context;

        public LaptopService(DataModel context)
        {
            _context = context;
        }

        public IEnumerable<Laptop> GetLaptopsBySort(int sortId)
        {
            IQueryable<Laptop> query = _context.Laptops;

            // Áp dụng sắp xếp tùy thuộc vào sortId
            switch (sortId)
            {
                case 1:  // Sắp xếp mới nhất
                    query = query.OrderByDescending(l => l.ngaycapnhat);
                    break;
                case 9:  // Sắp xếp bán chạy
                    query = query.OrderByDescending(l => l.soluongton);  // Giả sử rằng soluongton có thể đại diện cho "số lượng bán"
                    break;
                case 3:  // Sắp xếp theo giá
                    query = query.OrderBy(l => l.giaban);
                    break;
                default:  // Sắp xếp mặc định theo tên laptop
                    query = query.OrderBy(l => l.tenlaptop);
                    break;
            }

            return query.ToList();
        }
    }
}
