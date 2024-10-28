using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class AuctionItem
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        [Required]
        public decimal StartingBid { get; set; }

        [Required]
        public int AuctionDuration { get; set; }

        public string AdditionalTerms { get; set; }



    }
}
