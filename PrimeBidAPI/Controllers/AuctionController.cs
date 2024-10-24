using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;
using System.Threading.Tasks;

namespace PrimeBidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : Controller
    {

        private readonly AuctionService _auctionService;

        public AuctionController(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuctionItem([FromBody] AuctionItem auctionItem)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("Model state errors: " + string.Join(", ", errors));
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _auctionService.AddAuctionItem(auctionItem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while creating auction item: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

