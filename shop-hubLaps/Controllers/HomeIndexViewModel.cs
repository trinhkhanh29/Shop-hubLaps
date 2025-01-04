using System.Collections.Generic;

namespace shop_hubLaps.Models
{
    public class HomeIndexViewModel
    {
        internal List<Laptop> Laptops;

        public string SearchQuery { get; set; }

        public List<QuangCao> QuangCaos { get; set; } // Add this property

        public List<Hang> Brands { get; set; }

        public List<NhuCau> NhuCaus { get; set; }

        public List<Laptop> FilteredLaptops { get; set; }

        public List<string> LaptopNames => Laptops.Select(l => l.tenlaptop).ToList();

        public List<string> LaptopDescriptions => Laptops.Select(l => l.mota).ToList();

        public List<string> LaptopImages => Laptops.Select(l => l.hinh).ToList();

        public List<decimal?> LaptopPrices => Laptops.Select(l => l.giaban).ToList();

        public List<string> LaptopGPUs => Laptops.Select(l => l.gpu).ToList();

        public int CartItemCount { get; set; }

        public List<string> CpuFilter { get; set; }

        public Dictionary<string, string> CpuFilters { get; set; }  // Mapping of CPU value to display name

        public List<string> SelectedCpuFilters { get; set; }  // List of selected CPU filters

        public List<ChuDe> ChuDeList { get; set; }
    }
}


