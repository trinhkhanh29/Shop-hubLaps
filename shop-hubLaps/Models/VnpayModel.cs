using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_hubLaps.Models
{
    [Table("VnpayTransactions")]
    public class VnpayModel
    {
        [Key]
        public int Id { get; set; }

        public string OrderDescription { get; set; }

        public string TransactionId { get; set; }

        public string OrderId { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual DonHang DonHang { get; set; }

    }
}
