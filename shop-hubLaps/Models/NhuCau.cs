using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("NhuCau")]
    public partial class NhuCau
    {
        // Constructor khởi tạo
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhuCau()
        {
            Laptops = new HashSet<Laptop>(); // Khởi tạo danh sách Laptops
        }

        // Khóa chính
        [Key]
        public int manhucau { get; set; }

        // Tên nhu cầu
        [Required]
        [StringLength(50)]
        public string tennhucau { get; set; }

        // Danh sách các laptop liên quan đến nhu cầu này
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Laptop> Laptops { get; set; }
    }
}
