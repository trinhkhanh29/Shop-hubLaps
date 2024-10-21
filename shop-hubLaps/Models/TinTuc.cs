using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("TinTuc")]
    public partial class TinTuc
    {
        // Khởi tạo các bình luận liên quan
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TinTuc()
        {
            BinhLuans = new HashSet<BinhLuan>();
        }

        // Khóa chính
        [Key]
        public int matin { get; set; }

        // Tiêu đề của tin tức
        [Required]
        [StringLength(255)]
        public string tieude { get; set; }

        // Hình nền của tin tức
        [StringLength(70)]
        public string hinhnen { get; set; }

        // Tóm tắt nội dung tin tức
        [StringLength(255)]
        public string tomtat { get; set; }

        // Đường dẫn thân thiện (slug)
        [Required]
        [StringLength(100)]
        public string slug { get; set; }

        // Nội dung chi tiết của tin tức
        [Column(TypeName = "ntext")]
        public string noidung { get; set; }

        // Số lượt xem
        public int? luotxem { get; set; }

        // Ngày cập nhật tin tức
        [Column(TypeName = "smalldatetime")]
        public DateTime? ngaycapnhat { get; set; }

        // Trạng thái xuất bản (có hay không)
        public bool? xuatban { get; set; }

        // Khóa ngoại cho chủ đề tin tức
        public int? machude { get; set; }

        // Bộ sưu tập các bình luận liên quan đến tin tức
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }

        // Mối quan hệ với chủ đề
        public virtual ChuDe ChuDe { get; set; }
    }
}
