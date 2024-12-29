//using System;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace shop_hubLaps.Models
//{
//    [Table("Replies")]  // Tạo bảng Reply mới
//    public partial class Reply
//    {
//        [Key]
//        public int id { get; set; }  // Khóa chính

//        [StringLength(50)]
//        public string ten { get; set; }  // Tên người phản hồi

//        [Column(TypeName = "ntext")]
//        public string noidung { get; set; }  // Nội dung phản hồi

//        public DateTime ngayphanhoi { get; set; }  // Ngày phản hồi

//        public int madanhgia { get; set; }  // Khóa ngoại đến bảng DanhGia

//        [ForeignKey("madanhgia")]
//        public virtual DanhGia DanhGia { get; set; }  // Liên kết với bình luận gốc
//    }

//}
