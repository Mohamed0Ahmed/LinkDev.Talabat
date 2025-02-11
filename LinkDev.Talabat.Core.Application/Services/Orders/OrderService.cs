using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Orders;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specifications.Orders;

namespace LinkDev.Talabat.Core.Application.Services.Orders
{
    internal class OrderService(IBasketService basketService,
                                IUnitOfWork unitOfWork,
                                IMapper mapper,
                                IPaymentService paymentService) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order)
        {
            // 1 Get Basket From Repository

            var basket = await basketService.GetCustomerBasketAsync(order.BasketId);

            // 2 Get Selected Items At Basket and subTotal

            var orderItems = new List<OrderItem>();

            if (basket.Items.Any())
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    if (product is not null)
                    {

                        var productItemOrder = new ProductItemOrder()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            PictureUrl = product.PictureUrl ?? ""
                        };

                        var orderItem = new OrderItem()
                        {
                            Product = productItemOrder,
                            Price = product.Price,
                            Quantity = item.Quantity,

                        };
                        orderItems.Add(orderItem);


                    }
                }
            }


            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 3  Map Address

            var address = mapper.Map<Address>(order.ShippingAddress);

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(order.DeliveryMethodId) ?? throw new BadRequestException("Invalid delivery method selected.");


            // Check If No Duplicated OF Payment Intent Id
            var orderRepo = unitOfWork.GetRepository<Order, int>();
            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId!);

            var existOrder = await orderRepo.GetWithSpecAsync(spec);
            if (existOrder != null)
            {
                orderRepo.Delete(existOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            // 4  Create Order

            var orderToCreate = new Order()
            {

                ShippingAddress = address,
                BuyerEmail = buyerEmail,
                Items = orderItems,
                SubTotal = subTotal,
                DeliveryMethodId = order.DeliveryMethodId,
                DeliveryMethod = deliveryMethod,
                PaymentIntentId = basket.PaymentIntentId!

            };

            await unitOfWork.GetRepository<Order, int>().AddAsync(orderToCreate);



            // 5  Save To Database
            var created = await unitOfWork.CompleteAsync() > 0;
            if (!created)
                throw new BadRequestException("An Error Has Occurred");

            return mapper.Map<OrderToReturnDto>(orderToCreate);
        }



        public async Task<IEnumerable<OrderToReturnDto>> GetOrderForUserAsync(string BuyerEmail)
        {
            var orderSpec = new OrdersSpecifications(BuyerEmail);


            var orders = await unitOfWork.GetRepository<Order, int>().GetAllWithSpecAsync(orderSpec);

            return mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
        }


        public async Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            var orderSpec = new OrdersSpecifications(buyerEmail, orderId);
            var order = await unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(orderSpec) ?? throw new NotFoundException("order", orderId);

            return mapper.Map<OrderToReturnDto>(order);
        }


        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync()
        {

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethod);
        }
    }
}
