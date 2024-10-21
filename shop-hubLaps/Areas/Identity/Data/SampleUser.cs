using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using shop_hubLaps.Models;

namespace shop_hubLaps.Areas.Identity.Data
{
    // Thêm dữ liệu hồ sơ cho người dùng ứng dụng bằng cách thêm các thuộc tính cho lớp SampleUser
    public class SampleUser : IdentityUser
    {
        // Tên của người dùng
        public string FirstName { get; set; }

        // Họ của người dùng
        public string LastName { get; set; }

        // Ngày sinh của người dùng
        public DateTime? NgaySinh { get; set; }

        // Hồ sơ cá nhân của người dùng
        public string Profile { get; set; }

        // Đường dẫn đến ảnh đại diện của người dùng
        public string Avatar { get; set; }

        // Họ và tên đầy đủ
        public string HoTen { get; set; }

        // Địa chỉ của người dùng
        public string DiaChi { get; set; }

        // Danh sách các đơn hàng của người dùng
        //public virtual ICollection<DonHang> DonHangs { get; set; } = new HashSet<DonHang>(); // Khởi tạo danh sách đơn hàng
    }
}
