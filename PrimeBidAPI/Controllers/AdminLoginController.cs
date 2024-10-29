using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLoginController : Controller
    {
        private static readonly List<AdminLogin> AdminUsers = new List<AdminLogin>
        {
            new AdminLogin { Username = "Esanki@", Password = "primebidesa" },
            new AdminLogin { Username = "Sandali@", Password = "primebidsan" },
            new AdminLogin { Username = "Chanuka@", Password = "primebidchk" },
            new AdminLogin { Username = "Rovin@", Password = "primebidrvn" },
            new AdminLogin { Username = "Oneli@", Password = "primebidoni" },
        };

        [HttpPost]
        public IActionResult Login([FromBody] AdminLogin login)
        {
            // Check if the provided username and password are valid
            var user = AdminUsers.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);

            if (user != null)
            {
                // Login successful
                return Ok("Login successful!");
            }
            else
            {
                // Invalid login
                return Unauthorized("Invalid username or password!");
            }
        }
    }
}

