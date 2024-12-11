using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("Laptop")]
    public partial class Laptop
    {
        // Constructor để khởi tạo các ICollection
        public Laptop()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            DanhGias = new HashSet<DanhGia>();
            MetaLaptops = new HashSet<MetaLaptop>();
        }

        // Khóa chính
        [Key]
        public int malaptop { get; set; }

        // Tên laptop
        [Required]
        [StringLength(100)]
        public string tenlaptop { get; set; }

        // Giá bán
        public decimal? giaban { get; set; }

        // Mô tả sản phẩm
        [Column(TypeName = "ntext")]
        public string mota { get; set; }

        // Hình ảnh
        [StringLength(70)]
        public string? hinh { get; set; }

        // Mã hàng và nhu cầu
        public int? mahang { get; set; }
        public int? manhucau { get; set; }

        // Thông số kỹ thuật
        [StringLength(100)]
        public string cpu { get; set; }

        [StringLength(100)]
        public string gpu { get; set; }

        [StringLength(100)]
        public string ram { get; set; }

        [StringLength(100)]
        public string hardware { get; set; }

        [StringLength(100)]
        public string manhinh { get; set; }

        // Thông tin cập nhật
        public DateTime? ngaycapnhat { get; set; }

        // Số lượng tồn kho
        public int? soluongton { get; set; }

        // Thông tin pin
        [StringLength(100)]
        public string pin { get; set; }

        // Trạng thái sản phẩm
        public bool? trangthai { get; set; }

        // Quan hệ với các thực thể khác
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual ICollection<DanhGia> DanhGias { get; set; }

        public virtual ICollection<MetaLaptop> MetaLaptops { get; set; }

    }
}
