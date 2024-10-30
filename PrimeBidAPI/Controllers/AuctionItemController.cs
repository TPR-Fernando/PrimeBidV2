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

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionItem>> GetAuctionItem(int id)
        {
            try
            {
                var item = await _context.AuctionItems.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the auction item with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("CreateAuctionItemWithPictures")]
        public async Task<IActionResult> CreateAuctionItemWithPictures([FromForm] AuctionItemWithPictures model)
        {
            if (model.AuctionItem == null || model.Files == null || model.Files.Length == 0)
            {
                return BadRequest("Auction item or files are missing.");
            }

            try
            {
                _context.AuctionItems.Add(model.AuctionItem);
                await _context.SaveChangesAsync();

                var resourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                if (!Directory.Exists(resourceDirectory))
                {
                    Directory.CreateDirectory(resourceDirectory);
                }

                bool isImageUrlSet = false;

                foreach (var file in model.Files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(resourceDirectory, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var imageUrl = $"https://localhost:7217/Resources/{file.FileName}";
                        if (!isImageUrlSet)
                        {
                            model.AuctionItem.ImageUrl = imageUrl;
                            isImageUrlSet = true;
                        }
                    }
                }

                _context.AuctionItems.Update(model.AuctionItem);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Auction item and images added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving AuctionItem.");
                return StatusCode(500, new
                {
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet]
        [Route("GetAllAuctionItems")]
        public async Task<ActionResult<IEnumerable<AuctionItem>>> GetAllAuctionItems()
        {
            try
            {
                var items = await _service.GetAllAuctionItems();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all auction items.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Approve/{id}")]
        public async Task<IActionResult> ApproveAuctionItem(int id)
        {
            try
            {
                var itemDto = await _service.ApproveAuctionItemAsync(id);
                if (itemDto == null)
                {
                    // More specific messages can help identify the issue
                    var auctionItemExists = await _service.CheckAuctionItemExists(id); // Implement this check in your service
                    if (!auctionItemExists)
                    {
                        return NotFound(new { message = "Auction item not found." });
                    }
                    return BadRequest(new { message = "Approval failed due to an unknown reason." });
                }
                return Ok(new { message = "Auction item approved and moved to Items table successfully.", item = itemDto });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while approving the auction item with ID {Id}", id);
                return StatusCode(500, new { message = "Database error occurred while processing your request." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while approving the auction item with ID {Id}", id);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpDelete("Reject/{id}")]
        public async Task<IActionResult> RejectAuctionItem(int id)
        {
            try
            {
                var isSuccess = await _service.RejectAuctionItemAsync(id);
                if (!isSuccess)
                {
                    return NotFound(new { message = "Auction item not found for rejection." });
                }
                return Ok(new { message = "Auction item rejected successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rejecting the auction item with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
