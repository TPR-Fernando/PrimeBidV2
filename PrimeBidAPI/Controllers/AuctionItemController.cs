using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionItemController : ControllerBase
    {
        private readonly AuctionItemService _service;
        private readonly ILogger<AuctionItemController> _logger;
        private readonly AuctionDbContext _context;

        public AuctionItemController(AuctionItemService service, AuctionDbContext context, ILogger<AuctionItemController> logger)
        {
            _service = service;
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuctionItem([FromBody] AuctionItem item)
        {
            if (item == null)
            {
                _logger.LogError("Received null item in CreateAuctionItem.");
                return BadRequest("Item cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model validation failed in CreateAuctionItem: {@ModelState}", ModelState);
                return BadRequest(ModelState); // Returns details of validation errors
            }

            try
            {
                var newItem = await _service.AddAuctionItem(item);
                _logger.LogInformation("Auction item created successfully with ID {Id}.", newItem.Id);
                return CreatedAtAction(nameof(CreateAuctionItem), new { id = newItem.Id }, newItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating auction item.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error occurred." });
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

