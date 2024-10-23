// ProductService.cs
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Threading.Tasks;

namespace PrimeBidAPI.Services
{
    public class ProductService
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AuctionDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Item> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);
            var product = await _context.Items
                .Include(i => i.BidHistories)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found.", id);
            }
                return product;
        }
    }
}
