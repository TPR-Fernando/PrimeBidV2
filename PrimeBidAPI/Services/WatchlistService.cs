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
            return await _context.Watchlists
                                 .Where(item => item.Id == userId)
                                 .ToListAsync();
        }

        public async Task AddToWatchlistAsync(int userId, WatchlistModel item)
        {
            item.Id = userId; // Associate the item with the user
            await _context.Watchlists.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromWatchlistAsync(int userId, int itemId)
        {
            var item = await _context.Watchlists
                                     .FirstOrDefaultAsync(w => w.Id == itemId && w.Id == userId);
            if (item != null)
            {
                _context.Watchlists.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
