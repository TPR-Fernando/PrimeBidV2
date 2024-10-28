using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class Auctionpicture
    {
        [Required]
        public IFormFile file { get; set; } // This is your uploaded file

        [Required]
        public string FileName { get; set; } // This is the file name (e.g., image.jpg)

    }
}
