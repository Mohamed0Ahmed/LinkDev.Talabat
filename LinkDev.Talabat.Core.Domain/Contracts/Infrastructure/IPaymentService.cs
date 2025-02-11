using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket;

namespace LinkDev.Talabat.Core.Domain.Contracts.Infrastructure
{
    public interface IPaymentService
    {
        public Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);

        //public Task<bool> UpdateOrderPaymentStatus(string requestBody, string signatureHeader);
    }
}
