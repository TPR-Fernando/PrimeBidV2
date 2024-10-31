using Azure.Core;
using Braintree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeBidAPI.Data;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentpageController : ControllerBase
    {
        private readonly IBraintreeService _braintreeService;
        private readonly AuctionDbContext _context;

        public PaymentpageController(IBraintreeService braintreeService, AuctionDbContext context)
        {
            _braintreeService = braintreeService;
            _context = context;
        }

        //GET: api/paymentpage/{paymentId
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentPage(int paymentId)
        {
            var payment = await _context.Payments
                .Where(p => p.Id == paymentId)
                .Select(p => new PaymentPurchaseVM
                {
                    Id = p.Id,
                    ContactName = p.ContactName,
                    Mobile = p.Mobile,
                    Address = p.Address,
                    ZipAddress = p.ZipAddress,
                    TotalAmount = p.TotalAmount,
                    Items = p.Items,
                    Nonce = "" // You can set a default or empty value here
                })
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return BadRequest("No Payment Recorded");
            }

            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();

            return Ok(new { payment, clientToken });
        }

        // POST: api/paymentpage
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentPurchaseVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gateway = _braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = model.TotalAmount,
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var result = await gateway.Transaction.SaleAsync(request);

            if (result.IsSuccess())
            {
                // Handle success (e.g., save transaction details in the database)
                return Ok(new { message = "Transaction successful!", transactionId = result.Transaction.Id });
            }
            else
            {
                return BadRequest(new { message = "Transaction failed: " + result.Message });
            }
        }

        [HttpPost("SubmitPayment")]
        public async Task<IActionResult> SubmitPayment([FromBody] PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid payment data.");
            }

            var bidHistory = new BidHistory
            {
                UserId = Instances.CurrentUser.Id, // Replace with the actual user ID, if needed
                ItemId = int.Parse(request.Id), // Assuming you have this value from the item
                ItemName = request.ItemName,
                BidAmount = request.BidAmount,
                BidDate = DateTime.Now, // Or however you want to set this
                ItemImage = request.ItemImage,
                ContactName = request.ContactName,
                ContactNumber = request.ContactNumber,
                Address = request.Address,
                ZipCode = request.ZipCode
            };

            _context.BidHistories.Add(bidHistory);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Payment processed successfully." });
        }

    }


}
