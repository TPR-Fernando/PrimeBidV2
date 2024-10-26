using PrimeBidAPI.Data;
using PrimeBidAPI.Models;

namespace PrimeBidAPI.Services
{
    public class AuctionItemService
    {
        private readonly AuctionDbContext _context;

        public AuctionItemService(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<AuctionItem> AddAuctionItem(AuctionItem item)
        {
            _context.AuctionItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
