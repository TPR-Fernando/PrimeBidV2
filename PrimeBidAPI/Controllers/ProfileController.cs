using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;
using System.Collections.Generic; // Make sure to include this for List<>
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IBidHistoryService _bidHistoryService;
        private readonly IWatchlistService _watchlistService;
        private readonly ILogger<ProfileController> _logger; // Added logger
        private readonly AuctionDbContext _context;

        // Constructor to inject the services
        public ProfileController(IProfileService profileService, IBidHistoryService bidHistoryService, IWatchlistService watchlistService, ILogger<ProfileController> logger, AuctionDbContext context)
        {
            _profileService = profileService;
            _bidHistoryService = bidHistoryService;
            _watchlistService = watchlistService;
            _logger = logger; // Initialize logger
            _context = context;
        }

        // GET: api/profile/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning($"Profile not found for userId: {userId}");
                return NotFound(new { message = "Profile not found" });
            }

            return Ok(profile); // Return profile data as JSON
        }

        // GET: api/profile/{userId}/bid-history
        [HttpGet("{userId}/bid-history")]
        public async Task<IActionResult> GetBidHistory(int userId)
        {
            var bidHistory = await _bidHistoryService.GetBidHistoryAsync(userId);
            if (bidHistory == null || !bidHistory.Any()) // Check for empty list
            {
                _logger.LogInformation($"Bid history not found or empty for userId: {userId}");
                return Ok(new { message = "No bid history found", bidHistory = new List<BidHistory>() });
            }

            return Ok(bidHistory); // Return bid history data as JSON
        }

        // GET: api/profile/{userId}/watchlist
        [HttpGet("{userId}/watchlist")]
        public async Task<IActionResult> GetWatchlist(int userId)
        {
            var watchlist = await _watchlistService.GetWatchlistAsync(userId);
            if (watchlist == null || !watchlist.Any()) // Check for empty list
            {
                _logger.LogInformation($"Watchlist not found or empty for userId: {userId}");
                return Ok(new { message = "No watchlist items found", watchlist = new List<WatchlistModel>() });
            }

            return Ok(watchlist); // Return watchlist data as JSON
        }

        // PUT: api/profile/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> EditProfile(int userId, [FromBody] Profile profile)
        {
            if (profile == null)
            {
                return BadRequest(new { message = "Invalid profile data" });
            }

            // Validate Profile object (add any specific validation you need)
            if (string.IsNullOrEmpty(profile.FullName) || string.IsNullOrEmpty(profile.Email)) // Example validation
            {
                return BadRequest(new { message = "Name and email cannot be empty" });
            }

            var updated = await _profileService.UpdateProfileAsync(userId, profile);

            if (!updated)
            {
                _logger.LogError($"Failed to update profile for userId: {userId}");
                return StatusCode(500, new { message = "Failed to update profile" });
            }

            return Ok(new { message = "Profile updated successfully" });
        }

        // GET: api/profile/profiles
        [HttpGet("profiles")]
        public ActionResult<List<Profile>> GetAllProfiles()
        {
            var profiles = _profileService.GetAllProfiles().ToList(); // Call the sync method

            if (profiles == null || !profiles.Any())
            {
                return NotFound(); // 404 if no profiles found
            }

            return Ok(profiles); // 200 with the list of profiles
        }

        // DELETE: api/profile/{id}
        [HttpDelete("profiles/{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            _logger.LogInformation("Deleting user profile with ID: {UserId}", id);

            // SQL query to delete the profile
            var sqlQuery = "DELETE FROM [dbo].[Profiles] WHERE Id = @UserId"; // Ensure table name matches

            // Execute the delete command
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(sqlQuery, new SqlParameter("@UserId", id));

            if (affectedRows == 0)
            {
                _logger.LogWarning("User profile with ID: {UserId} not found.", id);
                return NotFound(new { message = "User profile not found." });
            }

            _logger.LogInformation("User profile with ID: {UserId} successfully deleted.", id);
            return Ok(new { message = "User profile deleted successfully." });
        }
    }
}
