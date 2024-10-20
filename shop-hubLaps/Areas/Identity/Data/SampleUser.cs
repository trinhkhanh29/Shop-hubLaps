using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace shop_hubLaps.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SampleUser class
public class SampleUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string Profile { get; set; }

    public string Avatar { get; set; }

    public string HoTen { get; set; }

    public string DiaChi { get; set; }
}

