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

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<BidHistory> BidHistories { get; set; }
        public DbSet<WatchlistModel> Watchlists { get; set; }



        // DbSets for PrimeBid context
        public DbSet<Item> Items { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        //Auction Item
        public DbSet<AuctionItem> AuctionItems { get; set; }
        //AuctionPic
        public DbSet<Auctionpicture> AuctionPic { get; set; }
        //AuctionItemWithPictures
        public DbSet<AuctionItemWithPictures> AuctionItemWithPictures { get; set; }
        public DbSet<AdminLogin> AdminLogins { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=Roro-Box\\SQLEXPRESS;Initial Catalog=PrimeBidDB;Integrated Security=True;Trust Server Certificate=True",
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5 // Maximum number of retries
                        );
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ApplicationDbContext entities
            modelBuilder.Entity<Profile>().ToTable("Profiles");
            modelBuilder.Entity<BidHistory>().ToTable("BidHistories");
            modelBuilder.Entity<WatchlistModel>().ToTable("Watchlists");

            // Configure PrimeBidDbContext entities
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<PaymentModel>().ToTable("Payments");
        }

    }
}