using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PrimeBidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailsController : ControllerBase
    {
        private readonly WatchlistService _watchlistService;
        private readonly ProductService _productService;
        private readonly ILogger<DetailsController> _logger;

        public DetailsController(
            ProductService productService,
            WatchlistService watchlistService,
            ILogger<DetailsController> logger)
        {
            _productService = productService;
            _watchlistService = watchlistService;
            _logger = logger;
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            _logger.LogInformation("Fetching details for product ID: {ProductId}", id);
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found.", id);
                return NotFound(new { message = "Product not found." });
            }
            _logger.LogInformation("Product with ID: {ProductId} successfully fetched.", id);
            return Ok(product);
        }
        // GET: api/Product
        [HttpGet("products")]
        public async Task<ActionResult<List<ItemDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound(); // 404 if no products
            }

            return Ok(products); // 200 with product list
        }

        // Endpoint to remove an item from the watchlist
        [HttpDelete("users/{userId}/watchlist/{productId}")]
        public async Task<IActionResult> RemoveFromWatchlist(int userId, int productId)
        {
            await _watchlistService.RemoveFromWatchlistAsync(userId, productId);
            _logger.LogInformation("Product ID {ProductId} removed from watchlist for user ID {UserId}.", productId, userId);
            return Ok(new { message = "Product removed from watchlist!" });
        }

        [HttpGet("soon-to-end")]
        public async Task<ActionResult<List<ItemDto>>> GetSoonToEndAuctions()
        {
            _logger.LogInformation("Getting soon-to-end auctions.");
            var soonToEndAuctions = await _productService.GetSoonToEndAuctionsAsync();

            if (soonToEndAuctions == null || !soonToEndAuctions.Any())
            {
                return NotFound(new { message = "No soon-to-end auctions found." });
            }

            return Ok(soonToEndAuctions);
        }

        [HttpGet("popular")]
        public async Task<ActionResult<List<ItemDto>>> GetPopularAuctions()
        {
            _logger.LogInformation("Getting popular auctions.");
            var popularAuctions = await _productService.GetPopularAuctionsAsync();

            if (popularAuctions == null || !popularAuctions.Any())
            {
                return NotFound(new { message = "No popular auctions found." });
            }

            return Ok(popularAuctions);
        }
    }
}