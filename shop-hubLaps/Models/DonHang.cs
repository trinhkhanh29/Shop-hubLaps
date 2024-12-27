using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shop_hubLaps.Models
{
    [Table("DonHang")]
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            tinhtrang = "CART";
        }

        [Key]
        public int madon { get; set; }

        public bool? thanhtoan { get; set; }
        public bool? giaohang { get; set; }
        public DateTime? ngaydat { get; set; }
        public DateTime? ngaygiao { get; set; }

        [Column("makh")]
        [StringLength(450)] // Chỉnh sửa chiều dài cho makh
        public string makh { get; set; }

        [Column("gia")]
        public decimal gia { get; set; }

        [StringLength(10)]
        public string tinhtrang { get; set; }

        [JsonPropertyName("donhang")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
       
        public void CapNhatGiaTri()
        {
            gia = ChiTietDonHangs.Sum(ct => ct.dongia * ct.soluong);

            foreach (var chiTiet in ChiTietDonHangs)
            {
                chiTiet.gia = chiTiet.dongia * chiTiet.soluong;
            }
        }

    }
}
