﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class AuctionItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? ItemDescription { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Starting bid must be a positive number.")]
        public decimal StartingBid { get; set; }

        // Remove AuctionDuration
        // public int AuctionDuration { get; set; }
        public DateTime AuctionEndDate { get; set; } // New property

        public string? AdditionalTerms { get; set; }
        public string? ImageUrl { get; set; }
    }
}
