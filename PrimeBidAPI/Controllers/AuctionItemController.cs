using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionItemController : Controller
    {
        private readonly AuctionItemService _service;

        public AuctionItemController(AuctionItemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuctionItem([FromBody] AuctionItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newItem = await _service.AddAuctionItem(item);
            return CreatedAtAction(nameof(CreateAuctionItem), new { id = newItem.Id }, newItem);
        }

    }
}
