using shop_hubLaps.Models;
using System.Collections.Generic;

namespace shop_hubLaps.Services
{
    public interface ILaptopService
    {
        IEnumerable<Laptop> GetLaptopsBySort(int sortId);
    }
}
