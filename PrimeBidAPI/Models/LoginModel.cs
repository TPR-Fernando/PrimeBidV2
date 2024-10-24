using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class LoginModel
    {
        //These error messages arent necessary as far as i know. But for quality of life.
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
