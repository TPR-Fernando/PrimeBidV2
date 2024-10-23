﻿namespace PrimeBidAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string? ItemImage { get; set; }
        public string? EstimatedBid { get; set; }
        public string ItemDescription { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<BidHistory> BidHistories { get; set; } = new List<BidHistory>();

        // PaymentItems relationship with Foreign Key
        public virtual ICollection<PaymentItemsModel> PaymentItems { get; set; } = new List<PaymentItemsModel>();
    }
}
