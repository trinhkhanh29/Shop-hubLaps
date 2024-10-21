using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("LienHe")]
    public partial class LienHe
    {
        // Khóa chính
        [Key]
        public int malienhe { get; set; }

        // Họ tên người liên hệ
        [Required]
        [StringLength(50)]
        public string hoten { get; set; }

        // Địa chỉ email
        [StringLength(254)]
        public string email { get; set; }

        // Số điện thoại
        [StringLength(50)]
        public string dienthoai { get; set; }

        // Website
        [StringLength(100)]
        public string website { get; set; }

        // Nội dung liên hệ
        [Column(TypeName = "ntext")]
        public string noidung { get; set; }

        // Trạng thái của liên hệ
        public bool? trangthai { get; set; }
    }
}
