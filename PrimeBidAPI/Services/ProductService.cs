using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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

        public async Task<ItemDto> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);

            // SQL query for the Item fields
            var sqlQuery = @"
            SELECT i.Id, i.ItemName, i.ItemImage, i.EstimatedBid, i.ItemDescription, i.EndDate, i.Category,
                (SELECT TOP 1 bh.BidAmount FROM [dbo].[BidHistories] AS bh WHERE bh.ItemId = i.Id ORDER BY bh.BidDate DESC) AS BidAmount
            FROM [dbo].[Items] AS i
            WHERE i.Id = @ProductId";

            var product = await _context.Items
                .FromSqlRaw(sqlQuery, new SqlParameter("@ProductId", id))
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    ItemName = i.ItemName,
                    ItemImage = i.ItemImage,
                    EstimatedBid = i.EstimatedBid,
                    ItemDescription = i.ItemDescription,
                    EndDate = i.EndDate,
                    Category = i.Category,
                    BidAmount = i.BidHistories.FirstOrDefault().BidAmount // Set BidAmount from BidHistories
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found.", id);
            }

            return product;
        }

        public async Task<List<ItemDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Fetching all products.");

            var sqlQuery = @"
            SELECT i.Id, i.ItemName, i.ItemImage, i.EstimatedBid, i.ItemDescription, i.EndDate, i.Category,
                (SELECT TOP 1 bh.BidAmount FROM [dbo].[BidHistories] AS bh WHERE bh.ItemId = i.Id ORDER BY bh.BidDate DESC) AS BidAmount
            FROM [dbo].[Items] AS i";

            var products = await _context.Items
                .FromSqlRaw(sqlQuery)
                .Include(i => i.BidHistories) // Include BidHistories
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    ItemName = i.ItemName,
                    ItemImage = i.ItemImage,
                    EstimatedBid = i.EstimatedBid,
                    ItemDescription = i.ItemDescription,
                    Category = i.Category,
                    EndDate = i.EndDate,
                    BidAmount = i.BidHistories.FirstOrDefault() != null ? i.BidHistories.FirstOrDefault().BidAmount : 0 // Safe access
                })
                .ToListAsync();

            if (!products.Any())
            {
                _logger.LogWarning("No products found.");
            }

            return products;
        }

        public async Task<List<ItemDto>> GetSoonToEndAuctionsAsync()
        {
            _logger.LogInformation("Fetching soon-to-end auctions.");

            var soonToEndAuctions = await _context.Items
                .OrderBy(i => i.EndDate) // Sort by EndDate to get soonest ending items
                .Take(4) // Limit to the first 4 items
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    ItemName = i.ItemName,
                    ItemImage = i.ItemImage,
                    EstimatedBid = i.EstimatedBid,
                    ItemDescription = i.ItemDescription,
                    Category = i.Category,
                    EndDate = i.EndDate,
                    BidAmount = i.BidHistories.FirstOrDefault() != null ? i.BidHistories.FirstOrDefault().BidAmount : 0
                })
                .ToListAsync();

            return soonToEndAuctions;
        }

        public async Task<List<ItemDto>> GetPopularAuctionsAsync()
        {
            _logger.LogInformation("Fetching popular auctions.");

            var popularAuctions = await _context.Items
                .OrderByDescending(i => i.BidHistories.Count) // Sort by bid count to get popular items
                .Take(4) // Limit to the first 4 items
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    ItemName = i.ItemName,
                    ItemImage = i.ItemImage,
                    EstimatedBid = i.EstimatedBid,
                    ItemDescription = i.ItemDescription,
                    Category = i.Category,
                    EndDate = i.EndDate,
                    BidAmount = i.BidHistories.FirstOrDefault() != null ? i.BidHistories.FirstOrDefault().BidAmount : 0
                })
                .ToListAsync();

            return popularAuctions;
        }


    }
}