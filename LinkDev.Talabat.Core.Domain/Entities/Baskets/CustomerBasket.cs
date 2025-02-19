using LinkDev.Talabat.Core.Domain.Common;

namespace LinkDev.Talabat.Core.Domain.Entities.Baskets
{
    public class CustomerBasket : BaseEntity<string>
    {

        public IEnumerable<BasketItem> Items { get; set; } = [];
        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
