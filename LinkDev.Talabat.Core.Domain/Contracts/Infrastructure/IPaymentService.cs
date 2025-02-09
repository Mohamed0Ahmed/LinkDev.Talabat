using LinkDev.Talabat.Core.Domain.Entities.Baskets;

namespace LinkDev.Talabat.Core.Domain.Contracts.Infrastructure
{
    public interface IPaymentService
    {
        public Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);

        //public Task<bool> UpdateOrderPaymentStatus(string requestBody, string signatureHeader);
    }
}
