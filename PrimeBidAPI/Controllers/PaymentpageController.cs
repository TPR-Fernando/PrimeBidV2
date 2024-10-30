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

        // GET: api/paymentpage/{paymentId}
        //[HttpGet("{paymentId}")]
        //public async Task<IActionResult> GetPaymentPage(int paymentId)
        //{
        //    var payment = await _context.Payments
        //        .Where(p => p.Id == paymentId)
        //        .Select(p => new PaymentPurchaseVM
        //        {
        //            Id = p.Id,
        //            ContactName = p.ContactName,
        //            Mobile = p.Mobile,
        //            Address = p.Address,
        //            ZipAddress = p.ZipAddress,
        //            TotalAmount = p.TotalAmount,
        //            Items = p.PaymentItems.Select(i => new Item
        //            {
        //                Id = i.Item.Id, // Assuming you want the Item Id
        //                ItemName = i.Item.ItemName,
        //                ItemImage = i.Item.ItemImage,
        //                Price = i.Item.Price,
        //            }).ToList(),
        //            Nonce = "" // You can set a default or empty value here
        //        })
        //        .FirstOrDefaultAsync();

        //    if (payment == null)
        //    {
        //        return NotFound();
        //    }

        //    var gateway = _braintreeService.GetGateway();
        //    var clientToken = gateway.ClientToken.Generate();

        //    return Ok(new { payment, clientToken });
        //}

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
    }
}
