using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionItemController : Controller
    {
        private readonly AuctionItemService _service;
        private readonly ILogger<AuctionItemController> _logger; // Logger instance
        private readonly AuctionDbContext _context; // Assuming you have this for database access

        public AuctionItemController(AuctionItemService service, AuctionDbContext context, ILogger<AuctionItemController> logger)
        {
            _service = service;
            _context = context; // Initialize DbContext
            _logger = logger; // Initialize logger
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuctionItem([FromBody] AuctionItem item)
        {
            // Log the incoming request data
            _logger.LogInformation("Received auction item: {@Item}", item);

            if (!ModelState.IsValid)
            {
                // Log model state errors
                _logger.LogWarning("Model state is invalid: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var newItem = await _service.AddAuctionItem(item);
                return CreatedAtAction(nameof(GetAuctionItem), new { id = newItem.Id }, newItem); // Changed to point to GetAuctionItem
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions
                _logger.LogError(ex, "An error occurred while creating the auction item.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionItem>> GetAuctionItem(int id)
        {
            try
            {
                // Attempt to find the auction item by ID
                var item = await _context.AuctionItems.FindAsync(id);

                // If the item is not found, return a 404 Not Found response
                if (item == null)
                {
                    return NotFound();
                }

                // If the item is found, return it with a 200 OK response
                return Ok(item);
            }
            catch (Exception ex) // Capture the exception to log it
            {
                // Log any unexpected exceptions
                _logger.LogError(ex, "An error occurred while fetching the auction item with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    
    }
}
