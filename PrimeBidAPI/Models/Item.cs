namespace PrimeBidAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string? ItemImage { get; set; }
        //current bids to be fetched from the bidHistory table
       

        public string? EstimatedBid { get; set; }
        public string ItemDescription { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; } // Price of the item

        public int Quantity { get; set; } // Quantity of the item
        public virtual ICollection<BidHistory> BidHistories { get; set; } = new List<BidHistory>();
        // Navigation property for related PaymentItems
        public virtual ICollection<PaymentItemsModel> PaymentItems { get; set; } = new List<PaymentItemsModel>();


    }
}
