using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class PaymentModel
    {
        [Key]
        public int Id { get; set; }

        public string? ContactName { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? ZipAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default value

        public virtual ICollection<PaymentItemsModel> PaymentItems { get; set; } = new List<PaymentItemsModel>();

        // Add this property back
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
