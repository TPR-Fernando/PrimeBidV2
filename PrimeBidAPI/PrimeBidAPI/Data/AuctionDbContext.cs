using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Models;

namespace PrimeBidAPI.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<BidHistory> BidHistories { get; set; }
        public DbSet<Profile> Profiles { get; set; }
    }
}
