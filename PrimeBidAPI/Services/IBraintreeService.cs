
using Braintree;

namespace PrimeBidAPI.Services
{
    public interface IBraintreeService
    {

        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();

    }
}
