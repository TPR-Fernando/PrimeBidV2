using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Data; 

namespace PrimeBidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistrationController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public UserRegistrationController(AuctionDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // Check if email already exists
            var existingUser = _context.Profiles.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
                return BadRequest("Email already registered.");

            // Generate a salt and hash the password
            var salt = GenerateSalt();
            var passwordHash = HashPassword(model.Password, salt);

            // Create a new profile
            var newUser = new Profile
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = passwordHash,
                Salt = salt,
            };

            // Save to the database
            _context.Profiles.Add(newUser);
            _context.SaveChanges();

            return Ok("Registration successful.");
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
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
