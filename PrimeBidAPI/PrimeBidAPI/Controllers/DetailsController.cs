using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;

namespace MizuConnectAPITEst.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly BidService _bidService;
        private readonly AuctionService _auctionService;
        private readonly WatchlistService _watchlistService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ProductService productService,
            BidService bidService,
            AuctionService auctionService,
            WatchlistService watchlistService,
            ILogger<HomeController> logger)
        {
            _productService = productService;
            _bidService = bidService;
            _auctionService = auctionService;
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

        [HttpPost("api/products/{id}/bid")]
        public async Task<IActionResult> PlaceBid(int id, [FromBody] BidRequest bidRequest)
        {
            var result = await _bidService.PlaceBidAsync(id, bidRequest.UserId, bidRequest.BidAmount);

            if (!result)
                return BadRequest("You are failure");

            return Ok(new { message = "Bid placed successfully!" });
        }

        [HttpGet("api/products/{id}/bids")]
        public async Task<IActionResult> GetBiddingHistory(int id)
        {
            var bids = await _bidService.GetBiddingHistoryAsync(id);
            if (bids == null)
                return NotFound();

            return Ok(bids);
        }

        [HttpGet("api/products/{id}/timer")]
        public async Task<IActionResult> GetAuctionTimer(int id)
        {
            var timer = await _auctionService.GetAuctionTimerAsync(id);
            if (timer == null)
                return NotFound();

            return Ok(timer);
        }

        [HttpPost("api/users/{userId}/watchlist")]
        public async Task<IActionResult> AddToWatchlist(string userId, [FromBody] WatchlistRequest request)
        {
            var result = await _watchlistService.AddToWatchlistAsync(userId, request.ProductId);

            if (!result)
                return BadRequest("You are failure");

            return Ok(new { message = "Product added to watchlist!" });
        }

        [HttpDelete("api/users/{userId}/watchlist/{productId}")]
        public async Task<IActionResult> RemoveFromWatchlist(string userId, string productId)
        {
            var result = await _watchlistService.RemoveFromWatchlistAsync(userId, productId);

            if (!result)
                return BadRequest("You are failure");

            return Ok(new { message = "Product removed from watchlist!" });
        }

        [HttpGet("api/users/{userId}/watchlist")]
        public async Task<IActionResult> GetWatchlist(string userId)
        {
            var watchlist = await _watchlistService.GetWatchlistAsync(userId);
            if (watchlist == null)
                return NotFound();

            return Ok(watchlist);
        }

    }
}