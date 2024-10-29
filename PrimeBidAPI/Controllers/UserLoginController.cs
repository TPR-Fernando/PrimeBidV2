using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Security.Cryptography;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public UserLoginController(AuctionDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Find user by email
            var user = _context.Profiles.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
                return Unauthorized(new { error = "User Not Found." });

            // Verify password
            var passwordHash = HashPassword(model.Password, user.Salt);
            if (user.PasswordHash != passwordHash)
                return Unauthorized(new { error = "Invalid Passsword." });

            // Create a session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFullName", user.FullName);
            //HttpContext.Session.SetString("UserId", user.Id.ToString()); this will store the ID as a string. Kept it just in case.
            HttpContext.Session.SetInt32("UserId", user.Id);

            // Redirect to the Home page
            //return RedirectToAction("Index", "Home");  This part is not necessary since JavaScript is already set up to Redirect the User. But i kept it just in case

            // Return success message
            return Ok(new { message = "Login successful" });
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
