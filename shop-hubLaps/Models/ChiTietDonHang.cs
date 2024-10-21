using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        // Khóa chính từ DonHang
        [Key, Column(Order = 0)] // Đánh dấu đây là khóa chính đầu tiên
        public int madon { get; set; }

        // Khóa chính từ Laptop
        [Key, Column(Order = 1)] // Đánh dấu đây là khóa chính thứ hai
        public int malaptop { get; set; }

        public decimal dongia { get; set; }

        public int soluong { get; set; }

        // Mối quan hệ với DonHang
        public virtual DonHang DonHang { get; set; }

        // Mối quan hệ với Laptop
        public virtual Laptop Laptop { get; set; }
    }
}
