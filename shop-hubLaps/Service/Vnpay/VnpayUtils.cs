/*using System.Security.Cryptography;
using System.Text;

namespace shop_hubLaps.Service.Vnpay
{
    public static class VnpayUtils
    {
        /// <summary>
        /// Tính toán SecureHash từ rawData và secretKey.
        /// </summary>
        /// <param name="rawData">Chuỗi dữ liệu cần mã hóa.</param>
        /// <param name="secretKey">Khóa bí mật.</param>
        /// <returns>Chuỗi hash SHA256 (hex string).</returns>
        public static string ComputeHash(string rawData, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return string.Concat(hash.Select(b => b.ToString("x2"))); // Chuyển hash thành chuỗi hex
            }
        }
    }
}
*/