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
                // Save the auction item to the database to get the ID
                _context.AuctionItems.Add(model.AuctionItem);
                await _context.SaveChangesAsync();

                // Loop through each uploaded file
                foreach (var file in model.Files)
                {
                    if (file.Length > 0)
                    {
                        // Define the file path and save the file
                        var filePath = Path.Combine("D:\\NSBM\\PrimeBidV2\\PrimeBidFrontend\\view\\Resources", file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Generate URL (modify based on how you serve images, e.g., local server, cloud)
                        var imageUrl = $"https://localhost:7217/Resources/{file.FileName}";

                        // Save the URL to the database if it's the first image
                        if (string.IsNullOrEmpty(model.AuctionItem.ImageUrl))
                        {
                            model.AuctionItem.ImageUrl = imageUrl;
                        }
                    }
                }

                // Save the updated AuctionItem with the ImageUrl
                await _context.SaveChangesAsync();

                return Ok(new { message = "Auction item and images added successfully." });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message;
                _logger.LogError(ex, "An error occurred while saving AuctionItem. Inner Exception: {InnerException}", innerExceptionMessage);

                return StatusCode(500, new
                {
                    error = ex.Message,
                    innerException = innerExceptionMessage
                });
            }
        }

    }
}





