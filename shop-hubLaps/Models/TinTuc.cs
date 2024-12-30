using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("TinTuc")]
    public partial class TinTuc
    {
        public TinTuc()
        {
            BinhLuans = new HashSet<BinhLuan>();
            ngaycapnhat = DateTime.Now;
            luotxem = 0;
            xuatban = false;
        }

        public TinTuc(string tieude, string slug, int machude, ChuDe chuDe) : this()
        {
            this.tieude = tieude;
            this.slug = slug;
            this.machude = machude;
            this.ChuDe = chuDe;
        }

        [Key]
        public int matin { get; set; }

        // Tiêu đề của tin tức (Bắt buộc)
        [Required]
        [StringLength(255)]
        public string tieude { get; set; }

        // Hình nền của tin tức (Nullable, cho phép null nếu không có ảnh)
        [StringLength(255)]
        public string? hinhnen { get; set; }

        // Tóm tắt nội dung tin tức (Nullable, cho phép null nếu không có tóm tắt)
        [StringLength(255)]
        public string? tomtat { get; set; }

        // Đường dẫn thân thiện (slug) (Bắt buộc)
        [Required]
        [StringLength(100)]
        public string slug { get; set; }

        // Nội dung chi tiết của tin tức (Nullable, cho phép null nếu không có nội dung)
        [Column(TypeName = "ntext")]
        public string? noidung { get; set; }

        // Số lượt xem (Nullable, có thể để mặc định là 0)
        public int? luotxem { get; set; }

        // Ngày cập nhật tin tức (Nullable)
        [Column(TypeName = "smalldatetime")]
        public DateTime? ngaycapnhat { get; set; }

        // Trạng thái xuất bản (có hay không) (Nullable)
        public bool? xuatban { get; set; }

        [ForeignKey("ChuDe")]
        public int machude { get; set; } // Foreign Key for ChuDe
        public virtual ChuDe ChuDe { get; set; } // Navigation property

        // Bộ sưu tập các bình luận liên quan đến tin tức
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }

    }

}
