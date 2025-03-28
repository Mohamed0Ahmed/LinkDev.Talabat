using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders;

namespace LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Orders
{
    public interface IOrderService
    {

        Task<OrderToReturnDto> CreateOrderAsync(string buyer, OrderToCreateDto order);
        Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId);
        Task<IEnumerable<OrderToReturnDto>> GetOrderForUserAsync(string buyerEmail);
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync();
    }
}
