using LinkDev.Talabat.Core.Domain.Common;

namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
    public class Order : BaseAuditableEntity<int>
    {
        public required string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public required Address ShippingAddress { get; set; }

        public int? DeliveryMethodId { get; set; }
        public virtual DeliveryMethod? DeliveryMethod { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; } = [];
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public decimal GetTotal() => SubTotal + DeliveryMethod!.Cost;

        public string PaymentIntentId { get; set; } = "";
    }
}
