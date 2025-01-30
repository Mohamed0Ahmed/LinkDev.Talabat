using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders;

namespace LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Orders
{
    public interface IOrderService
    {

        Task<OrderToReturnDto> CreateOrderAsync(string Buyer, OrderToCreateDto order);
        Task<OrderToReturnDto> GetOrderByAsync(string BuyerEmail, int orderId);
        Task<IEnumerable<OrderToReturnDto>> GetOrderForUserAsync(string BuyerEmail);
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync();
    }
}
