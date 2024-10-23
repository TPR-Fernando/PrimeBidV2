using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;
using System.Threading.Tasks;

namespace PrimeBidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailsController : ControllerBase
    {
        private readonly WatchlistService _watchlistService;

        public DetailsController(WatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        // Endpoint to remove an item from the watchlist
        [HttpDelete("users/{userId}/watchlist/{productId}")]
        public async Task<IActionResult> RemoveFromWatchlist(int userId, int productId)
        {
            // Call the service to remove the watchlist item using the userId and productId
            await _watchlistService.RemoveFromWatchlistAsync(userId, productId);

            // Return a success response
            return Ok(new { message = "Product removed from watchlist!" });
        }
    }
}
