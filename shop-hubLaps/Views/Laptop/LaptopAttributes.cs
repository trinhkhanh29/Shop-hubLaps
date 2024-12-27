
namespace shop_hubLaps.Views.Laptop
{
    public static class LaptopAttributes
    {      
        // CPU names dictionary
        public static readonly Dictionary<string, string> CpuNames = new Dictionary<string, string>
    {
        { "intel_core_i3", "Intel Core i3" },
        { "intel_core_i5", "Intel Core i5" },
        { "intel_core_i7", "Intel Core i7" },
        { "intel_core_i9", "Intel Core i9" },
        { "intel_celeron_pentium", "Intel Celeron / Pentium" },
        { "intel_core_u5", "Intel Core U5" },
        { "intel_core_u7", "Intel Core U7" },
        { "intel_core_u9", "Intel Core U9" },
        { "amd_ryzen_5", "AMD Ryzen 5" },
        { "amd_ryzen_7", "AMD Ryzen 7" },
        { "amd_ryzen_9", "AMD Ryzen 9" },
        { "apple_m1", "Apple M1" },
        { "apple_m1_pro", "Apple M1 Pro" },
        { "apple_m1_max", "Apple M1 Max" },
        { "apple_m2", "Apple M2" },
        { "apple_m2_pro", "Apple M2 Pro" },
        { "apple_m2_max", "Apple M2 Max" },
        { "apple_m3", "Apple M3" },
        { "apple_m3_pro", "Apple M3 Pro" },
        { "apple_m3_max", "Apple M3 Max" },
        { "qualcomm_snapdragon", "Qualcomm Snapdragon" },
        { "snapdragon_x_plus", "Snapdragon X Plus" },
        { "other", "Khác" }
    };

        // GPU names dictionary
        public static readonly Dictionary<string, string> GpuNames = new Dictionary<string, string>
    {
        { "RX 6550M", "AMD Radeon™ RX 6550M" },
        { "Arc A530M", "Intel Arc A530M" },
        { "MX550", "NVIDIA GeForce MX550" },
        { "MX570", "NVIDIA GeForce MX570" },
        { "GTX 1650", "NVIDIA GeForce GTX 1650" },
        { "RTX 2050", "NVIDIA GeForce RTX 2050" },
        { "RTX 3050", "NVIDIA GeForce RTX 3050" },
        { "RTX 3050Ti", "NVIDIA GeForce RTX 3050 Ti" },
        { "RTX 3060", "NVIDIA GeForce RTX 3060" },
        { "RTX 3070", "NVIDIA GeForce RTX 3070" },
        { "RTX 4050", "NVIDIA GeForce RTX 4050" },
        { "RTX 4060", "NVIDIA GeForce RTX 4060" },
        { "RTX 4070", "NVIDIA GeForce RTX 4070" },
        { "RTX 4080", "NVIDIA GeForce RTX 4080" },
        { "RTX 4090", "NVIDIA GeForce RTX 4090" },
        { "other", "Khác" }
    };

        // RAM sizes dictionary
        public static readonly Dictionary<string, string> RamSizes = new Dictionary<string, string>
    {
        { "4gb", "4 GB" },
        { "8gb", "8 GB" },
        { "12gb", "12 GB" },
        { "16gb", "16 GB" },
        { "18gb", "18 GB" },
        { "24gb", "24 GB" },
        { "32gb", "32 GB" },
        { "36gb", "36 GB" },
        { "48gb", "48 GB" },
        { "64gb", "64 GB" },
        { "96gb", "96 GB" },
        { "128gb", "128 GB" },
        { "other", "Khác" }
    };

        // Storage type dictionary (Hardware)
        public static readonly Dictionary<string, string> HardwareTypes = new Dictionary<string, string>
    {
        { "ssd_120gb", "SSD 120 GB" },
        { "ssd_250gb", "SSD 250 GB" },
        { "ssd_500gb", "SSD 500 GB" },
        { "ssd_1tb", "SSD 1 TB" },
        { "ssd_2tb", "SSD 2 TB" },
        { "ssd_4tb", "SSD 4 TB" },
        { "hdd_500gb", "HDD 500 GB" },
        { "hdd_1tb", "HDD 1 TB" },
        { "hdd_2tb", "HDD 2 TB" },
        { "hdd_4tb", "HDD 4 TB" },
        { "hdd_8tb", "HDD 8 TB" },
        { "hybrid_500gb", "Hybrid 500 GB" },
        { "hybrid_1tb", "Hybrid 1 TB" },
        { "hybrid_2tb", "Hybrid 2 TB" },
        { "other", "Khác" }
    };

        // Display sizes dictionary
        public static readonly Dictionary<string, string> ScreenSizes = new Dictionary<string, string>
    {
        { "13inch", "13 inch" },
        { "14inch", "14 inch" },
        { "15inch", "15 inch" },
        { "16inch", "16 inch" },
        { "17inch", "17 inch" },
        { "18inch", "18 inch" },
        { "16_9", "16:9" },
        { "16_10", "16:10" },
        { "3_2", "3:2" },
        { "4_3", "4:3" },
        { "fullhd", "Full HD (1920x1080)" },
        { "2k", "2K (2560x1440)" },
        { "4k", "4K (3840x2160)" },
        { "hd", "HD (1366x768)" },
        { "other", "Khác" }
    };
        // Pin names dictionary
        public static readonly Dictionary<string, string> PinNames = new Dictionary<string, string>
    {
        { "3cell_50wh", "3-cell, 50 Wh" },
        { "4cell_60wh", "4-cell, 60 Wh" },
        { "4cell_70wh", "4-cell, 70 Wh" },
        { "6cell_90wh", "6-cell, 90 Wh" },
        { "6cell_100wh", "6-cell, 100 Wh" },
        { "6cell_120wh", "6-cell, 120 Wh" },
        { "7cell_150wh", "7-cell, 150 Wh" },
        { "other", "Khác" }

    };
    }

}
