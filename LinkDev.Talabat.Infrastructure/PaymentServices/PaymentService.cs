using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Specifications.Orders;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Options;
using Stripe;
using Product = LinkDev.Talabat.Core.Domain.Entities.Products.Product;

namespace LinkDev.Talabat.Infrastructure.PaymentServices
{
    internal class PaymentService(IBasketRepository basketRepository,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper,
                                  IOptions<StripeSettings> stripeSettings,
                                  IOptions<RedisSettings> redisSettings) : IPaymentService
    {
        private readonly StripeSettings _stripeSettings = stripeSettings.Value;
        private readonly RedisSettings _redisSettings = redisSettings.Value;


        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var basket = await basketRepository.GetAsync(basketId) ?? throw new NotFoundException("Basket", basketId);


            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                    .GetAsync(basket.DeliveryMethodId.Value) ?? throw new NotFoundException(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);

                basket.ShippingPrice = deliveryMethod.Cost;
            }


            if (basket.Items.Any())
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id) ?? throw new NotFoundException("Product", item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent? paymentIntent;
            var paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(basket.Items.Sum(item => item.Price * item.Quantity) + basket.ShippingPrice),
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                try
                {
                    paymentIntent = await paymentIntentService.GetAsync(basket.PaymentIntentId);

                    if (paymentIntent == null)
                    {
                        var options = new PaymentIntentCreateOptions
                        {
                            Amount = (long)(basket.Items.Sum(item => item.Price * item.Quantity) + basket.ShippingPrice),
                            Currency = "USD",
                            PaymentMethodTypes = ["card"]
                        };

                        paymentIntent = await paymentIntentService.CreateAsync(options);
                        basket.PaymentIntentId = paymentIntent.Id;
                        basket.ClientSecret = paymentIntent.ClientSecret;
                    }
                    else
                    {
                        var options = new PaymentIntentUpdateOptions
                        {
                            Amount = (long)(basket.Items.Sum(item => item.Price * item.Quantity) + basket.ShippingPrice),
                        };

                        await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
                    }
                }
                catch (StripeException)
                {
                    throw new NotFoundException("PaymentIntent", basket.PaymentIntentId);
                }
                Console.WriteLine("Generated Client Secret: " + paymentIntent.ClientSecret);

            }


            await basketRepository.UpdateAsync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays));

            return mapper.Map<CustomerBasketDto>(basket);

            throw new NotImplementedException();
        }



        public async Task<bool> UpdateOrderPaymentStatus(string requestBody, string signatureHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(requestBody, signatureHeader, _stripeSettings.WebhookSecret);

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    var paymentIntentSucceeded = stripeEvent.Data.Object as PaymentIntent;

                    await UpdatePaymentIntent(paymentIntentSucceeded!.Id, isPaid: true);

                    break;

                case "payment_intent.payment_failed":
                    var paymentIntentFailed = stripeEvent.Data.Object as PaymentIntent;

                    await UpdatePaymentIntent(paymentIntentFailed!.Id, isPaid: false);

                    break;


            }

            return true;
        }

        private async Task<Order> UpdatePaymentIntent(string PaymentIntentId, bool isPaid)
        {
            var orderRepo = unitOfWork.GetRepository<Order, int>();

            var spec = new OrderWithPaymentIntentSpecifications(PaymentIntentId);

            var order = await orderRepo.GetWithSpecAsync(spec) ?? throw new NotFoundException("Order", PaymentIntentId);



            if (isPaid)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;



            orderRepo.Update(order);
            await unitOfWork.CompleteAsync();
            return order;

        }
    }
}
