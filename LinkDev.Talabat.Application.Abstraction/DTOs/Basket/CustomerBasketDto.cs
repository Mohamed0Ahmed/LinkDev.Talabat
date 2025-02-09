using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket
{
    public class CustomerBasketDto
    {
        [Required]
        public required string Id { get; set; }
        public IEnumerable<BasketItemDto> Items  { get; set; } = [];

        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
