using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Orders;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Core.Application.Services.Orders
{
    internal class OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(string BuyerEmail, OrderToCreateDto order)
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

                    }
                }
            }


            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 3  Map Address

            var address = mapper.Map<Address>(order.ShippingAddress);


            // 4  Create Order

            var orderToCreate = new Order()
            {
                BuyerEmail = BuyerEmail,
                ShippingAddress = address,
                Items = orderItems,
                SubTotal = subTotal,
                DeliveryMethodId = order.DeliveryMethodId,
            };

            await unitOfWork.GetRepository<Order, int>().AddAsync(orderToCreate);



            // 5  Save To Database
            var created = await unitOfWork.CompleteAsync() > 0;
            if (!created)
                throw new BadRequestException("An Error Has Occurred");

            return mapper.Map<OrderToReturnDto>(orderToCreate);
        }

        public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderToReturnDto> GetOrderByAsync(string BuyerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderToReturnDto>> GetOrderForUserAsync(string BuyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
