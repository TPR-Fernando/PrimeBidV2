using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class AuctionItem
    {
        [Key]
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
        public int AuctionDuration { get; set; }  // Duration in days

        public string AdditionalTerms { get; set; }

        public string ImagePath { get; set; }  // Path for the uploaded image
    }
}
