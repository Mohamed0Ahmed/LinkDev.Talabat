using LinkDev.Talabat.Core.Domain.Entities.Orders;

namespace LinkDev.Talabat.Core.Domain.Specifications.Orders
{
    public class OrdersSpecifications : BaseSpecifications<Order, int>
    {
        public OrdersSpecifications(string buyerEmail, int orderId) :
            base(order => order.BuyerEmail == buyerEmail &&
                 order.Id == orderId)
        {
            AddIncludes();
            AddOrderByDesc(order => order.OrderDate);
        }
        public OrdersSpecifications(string buyerEmail) :
            base(order => order.BuyerEmail == buyerEmail)
        {
            AddIncludes();
            AddOrderByDesc(order => order.OrderDate);
        }
      


        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(order => order.Items);
            Includes.Add(order => order.DeliveryMethod!);
        }

    }
}
