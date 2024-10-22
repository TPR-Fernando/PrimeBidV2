using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public class WatchlistService
    {
        internal async Task<bool> AddToWatchlistAsync(string userId, object productId)
        {
            throw new NotImplementedException();
        }

        internal async Task<WatchList> GetWatchlistAsync(string userId)
        {
            throw new NotImplementedException();
        }

        internal async Task<bool> RemoveFromWatchlistAsync(string userId, string productId)
        {
            throw new NotImplementedException();
        }
    }
}