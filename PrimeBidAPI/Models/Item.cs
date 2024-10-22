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

        public virtual ICollection<BidHistory> BidHistories { get; set; } = new List<BidHistory>();

    }
}
