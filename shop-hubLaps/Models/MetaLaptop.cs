using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("MetaLaptop")]
    public partial class MetaLaptop
    {
        // Khóa chính
        [Key]
        public int mameta { get; set; }

        // Khóa meta cho laptop
        [StringLength(255)]
        public string keymeta { get; set; }

        // Giá trị tương ứng với khóa meta
        [StringLength(255)]
        public string valuemeta { get; set; }

        // Khóa ngoại liên kết tới laptop
        public int? malaptop { get; set; }

        // Tham chiếu tới đối tượng Laptop
        public virtual Laptop Laptop { get; set; }
    }
}
