namespace PrimeBidAPI.Models
{
    public class BidHistory
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidDate { get; set; }
        public string ItemImage { get; set; }

        // Add this foreign key property
        public int ItemId { get; set; } // Foreign key to Item

        // Add this navigation property
        public Item Item { get; set; } // Navigation property for relationship with Item
    }
}
