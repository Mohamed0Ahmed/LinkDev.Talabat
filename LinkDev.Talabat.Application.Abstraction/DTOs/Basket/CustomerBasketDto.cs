using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket
{
    public class CustomerBasketDto
    {
        [Required(ErrorMessage = "The id field is required.")]
        public required string Id { get; set; }

        [Required]
        public IEnumerable<BasketItemDto> Items { get; set; } = [];

        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
