using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Common;

namespace LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders
{
    public class OrderToCreateDto
    {
        public required string BasketId { get; set; }
        public required int DeliveryMethodId { get; set; }
        public required AddressDto ShippingAddress { get; set; }
    }
}
