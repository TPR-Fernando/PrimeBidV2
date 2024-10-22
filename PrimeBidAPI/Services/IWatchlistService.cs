using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public interface IWatchlistService
    {
        Task<List<WatchlistModel>> GetWatchlistAsync(int userId);
        Task AddToWatchlistAsync(int userId, WatchlistModel item);
        Task RemoveFromWatchlistAsync(int userId, int itemId);
    }
}
