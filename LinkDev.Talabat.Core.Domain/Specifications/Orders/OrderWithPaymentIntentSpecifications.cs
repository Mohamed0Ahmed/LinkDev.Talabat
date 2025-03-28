using LinkDev.Talabat.Core.Domain.Entities.Orders;

namespace LinkDev.Talabat.Core.Domain.Specifications.Orders
{
    public class OrderWithPaymentIntentSpecifications(string paymentIntentId) : BaseSpecifications<Order, int>(order => order.PaymentIntentId == paymentIntentId)
    {
    }
}
