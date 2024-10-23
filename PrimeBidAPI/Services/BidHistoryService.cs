using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace PrimeBidAPI.Services
{
    public class BidHistoryService : IBidHistoryService
    {
        private readonly AuctionDbContext _context;

        public BidHistoryService(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<List<BidHistory>> GetBidHistoryAsync(int userId)
        {
            return await _context.BidHistories
                                 .Where(b => b.Id == userId)
                                 .ToListAsync();
        }
    }
}
