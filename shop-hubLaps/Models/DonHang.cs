using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("DonHang")]
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
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

        [StringLength(1)]
        public string tinhtrang { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public int Property
        {
            get => default;
            set
            {
            }
        }
    }
}
