using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly AuctionDbContext _context;

        public WatchlistService(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<List<WatchlistModel>> GetWatchlistAsync(int userId)
        {
            // Correctly filter by UserId to get all watchlist items for the specific user
            return await _context.Watchlists
                                 .Where(item => item.UserId == userId) // Use UserId instead of Id
                                 .ToListAsync();
        }

        public async Task AddToWatchlistAsync(int userId, WatchlistModel item)
        {
            // Set UserId in the watchlist item instead of Id
            item.UserId = userId; // Associate the item with the user
            await _context.Watchlists.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromWatchlistAsync(int userId, int itemId)
        {
            // Correct the filtering logic to use UserId
            var item = await _context.Watchlists
                                     .FirstOrDefaultAsync(w => w.Id == itemId && w.UserId == userId); // Use UserId here
            if (item != null)
            {
                _context.Watchlists.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
