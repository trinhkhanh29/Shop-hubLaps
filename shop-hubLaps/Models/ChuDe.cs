using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("ChuDe")]
    public partial class ChuDe
    {
        public ChuDe()
        {
            TinTucs = new HashSet<TinTuc>();
        }

        [Key]
        public int machude { get; set; }

        [StringLength(60)]
        public string tenchude { get; set; }

        [StringLength(70)]
        public string slug { get; set; }

        [StringLength(70)]
        public string? hinh { get; set; }

        public virtual ICollection<TinTuc> TinTucs { get; set; }
    }
}
