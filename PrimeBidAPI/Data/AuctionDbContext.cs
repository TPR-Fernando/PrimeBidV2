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
        public DbSet<PaymentItemsModel> PaymentItems { get; set; }

        //Auction Item
        public DbSet<AuctionItem> AuctionItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=tcp:primebidserver.database.windows.net;Database=PrimeBidDB;User ID=admin2;Password=admin@20041127;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
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
            modelBuilder.Entity<PaymentItemsModel>().ToTable("PaymentItems");

            // Configure PaymentItemsModel relationships
            modelBuilder.Entity<PaymentItemsModel>()
                .HasKey(pi => pi.Id);

            modelBuilder.Entity<PaymentItemsModel>()
                .HasOne(pi => pi.Payment)
                .WithMany(p => p.PaymentItems)
                .HasForeignKey(pi => pi.PaymentId);

            modelBuilder.Entity<PaymentItemsModel>()
                .HasOne(pi => pi.Item)
                .WithMany(i => i.PaymentItems)
                .HasForeignKey(pi => pi.ItemId);
        }
    }
}
