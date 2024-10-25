using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using System;

namespace PrimeBidAPI.Services
{
    public class AuctionService
    {
        private readonly AuctionDbContext _context;

        // Constructor that accepts the AuctionDbContext
        public AuctionService(AuctionDbContext context)
        {
            _context = context;
        }

        // Method to add an auction item
        public async Task<AuctionItem> AddAuctionItem(AuctionItem auctionItem)
        {
            try
            {
                // Add the auction item to the context
                await _context.AuctionItems.AddAsync(auctionItem);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the added auction item
                return auctionItem;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving auction item: {ex.Message}");
                throw;
            }
        }
    }
}