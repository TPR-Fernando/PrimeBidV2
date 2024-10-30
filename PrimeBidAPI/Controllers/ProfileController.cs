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

        private bool IsSessionValid()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (!IsSessionValid())
                return Unauthorized(new { error = "Session expired. Please login again." });

            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null)
            {
                _logger.LogWarning($"Profile not found for userId: {userId}");
                return NotFound(new { message = "Profile not found" });
            }

            return Ok(profile);
        }

        [HttpGet("bid-history")]
        public async Task<IActionResult> GetBidHistory()
        {
            if (!IsSessionValid())
                return Unauthorized(new { error = "Session expired. Please login again." });

            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var bidHistory = await _bidHistoryService.GetBidHistoryAsync(userId);
            if (bidHistory == null || !bidHistory.Any())
            {
                _logger.LogInformation($"No bid history for userId: {userId}");
                return Ok(new { message = "No bid history found" });
            }

            return Ok(bidHistory);
        }

        [HttpGet("watchlist")]
        public async Task<IActionResult> GetWatchlist()
        {
            if (!IsSessionValid())
                return Unauthorized(new { error = "Session expired. Please login again." });

            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var watchlist = await _watchlistService.GetWatchlistAsync(userId);
            if (watchlist == null || !watchlist.Any())
            {
                _logger.LogInformation($"No watchlist items for userId: {userId}");
                return Ok(new { message = "No watchlist items found" });
            }

            return Ok(watchlist);
        }

        [HttpPut]
        public async Task<IActionResult> EditProfile([FromBody] Profile profile)
        {
            if (!IsSessionValid())
                return Unauthorized(new { error = "Session expired. Please login again." });

            var userId = HttpContext.Session.GetInt32("UserId").Value;

            if (profile == null || string.IsNullOrEmpty(profile.FullName) || string.IsNullOrEmpty(profile.Email))
                return BadRequest(new { message = "Invalid profile data" });

            var updated = await _profileService.UpdateProfileAsync(userId, profile);
            if (!updated)
            {
                _logger.LogError($"Failed to update profile for userId: {userId}");
                return StatusCode(500, new { message = "Failed to update profile" });
            }

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpGet("profiles")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _profileService.GetAllProfilesAsync();
            if (!profiles.Any())
                return NotFound(new { message = "No profiles found" });

            return Ok(profiles);
        }

        // DELETE: api/profile/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProfile()
        {
            // Retrieve the UserId from the session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");

            // Check if the session is valid
            if (sessionUserId == null)
                return Unauthorized(new { error = "Session has expired. Please log in again." });

            // SQL query to delete the profile
            var sqlQuery = "DELETE FROM [dbo].[Profiles] WHERE Id = @UserId";

            // Execute the delete command
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(sqlQuery, new SqlParameter("@UserId", sessionUserId.Value));

            if (affectedRows == 0)
            {
                _logger.LogWarning("Failed to delete profile for userId: {UserId}", sessionUserId.Value);
                return NotFound(new { message = "User profile not found." });
            }

            // Clear the session
            HttpContext.Session.Clear();

            _logger.LogInformation("Profile for userId: {UserId} deleted successfully.", sessionUserId.Value);
            return Ok(new { message = "Profile deleted successfully." });
        }

    }
}
