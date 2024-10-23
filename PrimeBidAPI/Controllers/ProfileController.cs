using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

        // Constructor to inject the services
        public ProfileController(IProfileService profileService, IBidHistoryService bidHistoryService, IWatchlistService watchlistService, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _bidHistoryService = bidHistoryService;
            _watchlistService = watchlistService;
            _logger = logger; // Initialize logger
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
    }
}
