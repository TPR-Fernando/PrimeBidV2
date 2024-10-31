namespace PrimeBidAPI.Models
{
    public class Bid
    {
        public bool Success { get; internal set; }
        public object? Message { get; internal set; }
    }

    public class PaymentRequest
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ItemName { get; set; }
        public decimal BidAmount { get; set; }
        public string ItemImage { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }

    public class ItemRequest
    {
        public int Id { get; set; } // Adjust based on your requirements
        public string ItemName { get; set; }
        public decimal? BidAmount { get; set; }
        public string ItemImage { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }
}