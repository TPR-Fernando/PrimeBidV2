using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Models;
using PrimeBidAPI.Data;
using System.Threading.Tasks;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderConfirmationController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public OrderConfirmationController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: api/orderconfirmation/{paymentId}
        //[HttpGet("{paymentId}")]
        //public async Task<IActionResult> GetOrderConfirmation(int paymentId = 1)
        //{
        //    // Log the incoming paymentId
        //    Console.WriteLine($"Received request for paymentId: {paymentId}");

        //    var payment = await _context.Payments
        //        .Include(p => p.PaymentItems)
        //        .ThenInclude(pi => pi.Item)
        //        .Where(p => p.Id == paymentId)
        //        .Select(p => new PaymentPurchaseVM
        //        {
        //            Id = p.Id,
        //            ContactName = p.ContactName,
        //            Mobile = p.Mobile,
        //            Address = p.Address,
        //            ZipAddress = p.ZipAddress,
        //            TotalAmount = p.TotalAmount,
        //            Nonce = "", // Placeholder, if needed for future use
        //            Items = p.PaymentItems.Select(pi => new Item
        //            {
        //                Id = pi.Item != null ? pi.Item.Id : 0,
        //                ItemImage = pi.Item != null ? pi.Item.ItemImage : "defaultImage.jpg",
        //                Price = pi.Item != null ? pi.Item.Price : 0,
        //            }).ToList()
        //        })
        //        .FirstOrDefaultAsync();

        //    if (payment == null)
        //    {
        //        // Log the issue for further debugging
        //        Console.WriteLine($"No payment found for PaymentId: {paymentId}");
        //        return NotFound(new { message = "No payment found for this ID." });
        //    }

        //    // Log the payment details for debugging
        //    Console.WriteLine($"Payment found: Id={payment.Id}, TotalAmount={payment.TotalAmount}, ItemsCount={payment.Items.Count}");

        //    return Ok(payment); // Return the payment details
        //}

        // POST: api/orderconfirmation/paymentmethods
        [HttpPost("paymentmethods")]
        public IActionResult PaymentMethods([FromBody] PaymentPurchaseVM model)
        {
            // Here you would typically handle the payment processing logic
            // For demonstration purposes, I'm just modifying the ID

            model.Id += 1;

            // Log the processed payment method
            Console.WriteLine($"Processing payment for model Id: {model.Id}");

            // You would typically save this information or perform further actions here
            // Redirecting is not applicable in APIs; instead, return the new payment ID
            return Ok(new { message = "Proceeding to payment page.", newPaymentId = model.Id });
        }
    }
}
