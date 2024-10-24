using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class AuctionItem
    {
        public int Id { get; set; }  // Primary Key

        [Required(ErrorMessage = "Item name is required.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be greater than zero.")]
        public decimal StartingPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Auction length must be at least 1 day.")]
        public int AuctionLength { get; set; }

        public string AdditionalTerms { get; set; }
    }
}
