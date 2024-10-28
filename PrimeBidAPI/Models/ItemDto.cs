namespace PrimeBidAPI.Models
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public string? ItemImage { get; set; }
        public string? EstimatedBid { get; set; }
        public string? ItemDescription { get; set; }
        public DateTime EndDate { get; set; }
        public string? Category { get; set; }
        public decimal? BidAmount { get; set; }
    }

}
