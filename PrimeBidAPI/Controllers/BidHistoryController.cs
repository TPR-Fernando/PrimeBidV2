using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidHistoryController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public BidHistoryController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: api/BidHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidHistory>>> GetAllBidHistories()
        {
            try
            {
                var bidHistories = await _context.BidHistories.ToListAsync();
                return Ok(bidHistories);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<BidHistory>>> GetBidHistoryByItemId(int itemId)
        {
            try
            {
                var bidHistories = await _context.BidHistories
                                                 .Where(bid => bid.ItemId == itemId)
                                                 .ToListAsync();

                if (!bidHistories.Any())
                {
                    return NotFound("No bids found for the specified item.");
                }

                return Ok(bidHistories);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
