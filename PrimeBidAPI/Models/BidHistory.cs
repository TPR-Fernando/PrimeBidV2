using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeBidAPI.Models
{
    public class BidHistory
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BidAmount { get; set; } // Make BidAmount nullable if it can be null

        public DateTime BidDate { get; set; }
        public string? ItemImage { get; set; }

        // Foreign key to Item
        public int ItemId { get; set; }

        // Navigation property for relationship with Item
        public Item Item { get; set; } // Navigation property
    }
}
