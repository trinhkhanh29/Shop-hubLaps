using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models // Giữ nguyên namespace
{
    [Table("BinhLuan")]
    public partial class BinhLuan
    {
        [Key]
        public int mabinhluan { get; set; }

        [StringLength(50)]
        public string ten { get; set; }

        [Column(TypeName = "ntext")]
        public string noidung { get; set; }

        public int? vote { get; set; }

        public DateTime? ngaybinhluan { get; set; }

        public int? matin { get; set; }

        public bool? trangthai { get; set; }

        public virtual TinTuc TinTuc { get; set; }
    }
}
