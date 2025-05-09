﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("DanhGia")]
    public partial class DanhGia
    {
        [Key]
        public int madanhgia { get; set; }

        [StringLength(50)]
        public string ten { get; set; }

        [Column(TypeName = "ntext")]
        public string noidung { get; set; }

        public int? vote { get; set; }

        public DateTime? ngaydanhgia { get; set; }

        public int malaptop { get; set; }

        public bool? trangthai { get; set; }

        [ForeignKey("malaptop")]
        public virtual Laptop Laptop { get; set; }
    }
}
