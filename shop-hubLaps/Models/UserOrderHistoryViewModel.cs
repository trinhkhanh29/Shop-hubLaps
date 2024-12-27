using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;

namespace shop_hubLaps.Models
{
    public class UserOrderHistoryViewModel
    {
        // Personal data of the user
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string BirthDate { get; set; }

        // Order history
        public IEnumerable<DonHang> DonHangs { get; set; }
    }
}
