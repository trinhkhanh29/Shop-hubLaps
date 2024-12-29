using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("QuangCao")]
    public partial class QuangCao
    {
        // Khóa chính
        [Key]
        public int maqc { get; set; }

        // Tên quảng cáo
        [Required]
        [StringLength(255)]
        public string tenqc { get; set; }

        // Tên công ty
        [Required]
        [StringLength(200)]
        public string tencongty { get; set; }

        // Hình nền quảng cáo
        [StringLength(100)]
        public string hinhnen { get; set; }

        // Link đến quảng cáo
        [StringLength(100)]
        public string link { get; set; }

        // Ngày bắt đầu hiển thị quảng cáo
        [Column(TypeName = "smalldatetime")]
        public DateTime? ngaybatdau { get; set; }

        // Ngày hết hạn quảng cáo
        [Column(TypeName = "smalldatetime")]
        public DateTime? ngayhethan { get; set; }

        // Trạng thái quảng cáo (có hoạt động hay không)
        public bool trangthai { get; set; }


    }
}
