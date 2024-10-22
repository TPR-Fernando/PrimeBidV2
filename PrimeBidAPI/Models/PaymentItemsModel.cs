using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class PaymentItemsModel
    {
        [Key]
        public int Id { get; set; }

        public int PaymentId { get; set; } // Foreign key to PaymentModel
        public int ItemId { get; set; } // Foreign key to ItemModel

        // New property for Quantity
        public int Quantity { get; set; } // Quantity of the item in the payment

        // Navigation properties
        public virtual PaymentModel? Payment { get; set; }
        public virtual Item? Item { get; set; }
    }
}
