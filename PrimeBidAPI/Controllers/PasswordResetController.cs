using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Security.Cryptography;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public PasswordResetController(AuctionDbContext context)
        {
            _context = context;
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] PasswordResetModel model)
        {
            // Find the user by email
            var user = _context.Profiles.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
                return BadRequest("User not found.");

            // Hash the new password and update it
            var newPasswordHash = HashPassword(model.NewPassword, user.Salt);
            user.PasswordHash = newPasswordHash;

            _context.SaveChanges();

            return Ok("Password reset successful.");
        }

        private string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];
                saltBytes.CopyTo(saltedPassword, 0);
                passwordBytes.CopyTo(saltedPassword, saltBytes.Length);
                var hash = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
