using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PrimeBidAPI.Services
{
    public class AuctionItemService
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<AuctionItemService> _logger;


        public AuctionItemService(AuctionDbContext context, ILogger<AuctionItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AuctionItem> AddAuctionItem(AuctionItem item)
        {


            _context.AuctionItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<AuctionItem>> GetAllAuctionItems()
        {
            return await _context.AuctionItems.ToListAsync();
        }

        public async Task<ItemDto?> ApproveAuctionItemAsync(int auctionItemId)
        {
            _logger.LogInformation("Approving auction item with ID: {AuctionItemId}", auctionItemId);

            var auctionItem = await _context.AuctionItems.FindAsync(auctionItemId);
            if (auctionItem == null) return null; // Return null if not found

            // Create a new Item based on the AuctionItem details
            var newItem = new Item
            {
                ItemName = auctionItem.ProductName,
                ItemDescription = auctionItem.ItemDescription,
                Category = auctionItem.Category,
                EstimatedBid = auctionItem.StartingBid.ToString(),
                EndDate = auctionItem.AuctionEndDate,
                ItemImage = auctionItem.ImageUrl
            };

            try
            {
                // Add the new item to the Items table
                _context.Items.Add(newItem);
                await _context.SaveChangesAsync(); // This will save and generate an ID for newItem

                // Remove the approved auction item
                _context.AuctionItems.Remove(auctionItem);
                await _context.SaveChangesAsync(); // Save the removal of the auction item
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the new item or removing the auction item with ID {AuctionItemId}", auctionItemId);
                return null; // Indicate failure for database-related issues
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the auction item with ID {AuctionItemId}", auctionItemId);
                return null; // Indicate unexpected failures
            }

            return new ItemDto
            {
                Id = newItem.Id, // The ID is now available after saving
                ItemName = newItem.ItemName,
                ItemImage = newItem.ItemImage,
                EstimatedBid = newItem.Price.ToString(),
                ItemDescription = newItem.ItemDescription,
                EndDate = newItem.EndDate,
                Category = newItem.Category,
                BidAmount = newItem.Price
            };
        }

        // Implement a method to check if the auction item exists
        public async Task<bool> CheckAuctionItemExists(int auctionItemId)
        {
            return await _context.AuctionItems.AnyAsync(ai => ai.Id == auctionItemId);
        }


        public async Task<bool> RejectAuctionItemAsync(int auctionItemId)
        {
            _logger.LogInformation("Rejecting auction item with ID: {AuctionItemId}", auctionItemId);

            var auctionItem = await _context.AuctionItems.FindAsync(auctionItemId);
            if (auctionItem == null)
            {
                return false; // Item not found
            }

            _context.AuctionItems.Remove(auctionItem);
            await _context.SaveChangesAsync();

            return true; // Item successfully removed
        }

    }
}
