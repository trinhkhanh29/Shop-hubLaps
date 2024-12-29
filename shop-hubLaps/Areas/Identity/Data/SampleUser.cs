using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using shop_hubLaps.Models;

namespace shop_hubLaps.Areas.Identity.Data
{
    public class SampleUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string? Profile { get; set; }

        public string? Avatar { get; set; }

        [NotMapped]
        public string HoTen => $"{FirstName} {LastName}";

        public string? DiaChi { get; set; }
    }
}
