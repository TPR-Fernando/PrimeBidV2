using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PrimeBidAPI.Models
{
    public class Auctionpicture
    {
        [Key]
        public int Id { get; set; } // Primary key

        [NotMapped]
        public IFormFile File { get; set; } // Not mapped to the database, used only for file upload

        public string FileName { get; set; } // Stores the file name, e.g., "image.jpg"

        // public byte[] FileData { get; set; } // Optional: Store file content as binary data

        // public string FilePath { get; set; } // Optional: Store path if saving files to disk
    }
}
