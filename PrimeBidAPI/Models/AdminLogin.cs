using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class AdminLogin
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
