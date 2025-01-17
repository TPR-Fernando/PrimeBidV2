﻿using System.ComponentModel.DataAnnotations;

namespace PrimeBidAPI.Models
{
    public class PaymentPurchaseVM : PaymentModel
    {
        [Required]
        public string? Nonce { get; set; }

        // Hiding the base class member with 'new'
        Item Items { get; set; } = new Item();
    }
}
