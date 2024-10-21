using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("Hang")]
    public partial class Hang
    {
        // Constructor để khởi tạo ICollection
        public Hang()
        {
            Laptops = new HashSet<Laptop>();
        }

        // Khóa chính
        [Key]
        public int mahang { get; set; }

        // Tên hàng
        [Required]
        [StringLength(30)]
        public string tenhang { get; set; }

        // Hình ảnh
        [StringLength(70)]
        public string hinh { get; set; }

        // Bộ sưu tập laptop
        public virtual ICollection<Laptop> Laptops { get; set; }
    }
}
