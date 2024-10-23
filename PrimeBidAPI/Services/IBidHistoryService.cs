using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public interface IBidHistoryService
    {
        Task<List<BidHistory>> GetBidHistoryAsync(int userId);
    }
}
