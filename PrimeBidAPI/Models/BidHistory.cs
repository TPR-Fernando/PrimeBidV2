using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeBidAPI.Models
{
    public class BidHistory
    {
        [Column(TypeName = "int")]
        public int Id { get; set; }  // Changed from Id to BidId

        public int UserId { get; set; }  // Foreign key to Users table
        public int ItemId { get; set; }  // Foreign key to Items table

        public string? ItemName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BidAmount { get; set; } // Make BidAmount nullable if it can be null

        public DateTime BidDate { get; set; } // Renamed from BidDate to BidTime
        public string? ItemImage { get; set; }

        // Contact information
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }

        // Navigation properties
        public Item? Item { get; set; } // Navigation property for relationship with Item
        // Add a navigation property for User if necessary
    }
}
