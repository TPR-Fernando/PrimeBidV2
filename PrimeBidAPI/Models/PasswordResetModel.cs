using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class PasswordResetModel
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
