using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("MetaLaptop")]
    public partial class MetaLaptop
    {
        [Key]
        public int mameta { get; set; }

        [StringLength(255)]
        public string keymeta { get; set; }

        [StringLength(255)]
        public string valuemeta { get; set; }


        [ForeignKey("Laptop")]
        public int malaptop { get; set; }

        public virtual Laptop Laptop { get; set; }
    }
}
