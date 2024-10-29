using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeBidAPI.Models
{
    [NotMapped]
    public class AuctionItemWithPictures
    {
        public AuctionItem AuctionItem { get; set; } // Contains auction item details

        [NotMapped]
        [Required(ErrorMessage = "At least one image file is required.")]// Ignore this property in the database mapping
        public IFormFile[] Files { get; set; } // Array of uploaded files
    }
}
