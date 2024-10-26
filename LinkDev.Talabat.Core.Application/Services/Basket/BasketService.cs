using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Basket;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Entities.Baskets;
using Microsoft.Extensions.Configuration;

namespace LinkDev.Talabat.Core.Application.Services.Basket
{
    internal class BasketService(IBasketRepository basketRepository, IMapper mapper, IConfiguration configuration) : IBasketService
    {
        public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetAsync(basketId);

            if (basket is null)
                throw new NotFoundException(nameof(CustomerBasket), basketId);

            return mapper.Map<CustomerBasketDto>(basket);

        }

        public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);
            var timeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!));

            var updatedBasket = await basketRepository.UpdateAsync(basket, timeToLive);


            if (updatedBasket is null)
                throw new BadRequestException("Can't Update , THere is Problem with your Basket");


            return basketDto;
        }

        public async Task DeleteCustomerBasketAsync(string id)
        {
            var deleted = await basketRepository.DeleteAsync(id);

            if (!deleted)
                throw new BadRequestException($"Unable to delete this basket.");
        }



    }
}
