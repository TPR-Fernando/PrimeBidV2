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
                // Save the auction item to the database
                _context.AuctionItems.Add(model.AuctionItem);
                await _context.SaveChangesAsync();

                // Directory for saving files
                var resourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                if (!Directory.Exists(resourceDirectory))
                {
                    Directory.CreateDirectory(resourceDirectory); // Create the directory if missing
                }

                // Track whether the image URL was set
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

                        // Set the ImageUrl only if not already set (for the first image)
                        if (!isImageUrlSet)
                        {
                            model.AuctionItem.ImageUrl = imageUrl;
                            isImageUrlSet = true;
                        }
                    }
                }

                // Update the auction item with the ImageUrl
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
    }
}





