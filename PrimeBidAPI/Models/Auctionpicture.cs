using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeBidAPI.Models
{
    public class Auctionpicture
    {
        [Key]
        public int Id { get; set; } // Primary key

        [NotMapped]
        public IFormFile file { get; set; } // Not mapped to the database, used only for file upload

        public string FileName { get; set; } // Stores the file name, e.g., "image.jpg"

        public byte[] FileData { get; set; } // Store file content as binary data (optional)

        // Alternatively, use a file path if storing files in the file system
        // public string FilePath { get; set; } // Path to where the file is saved
    }
}
