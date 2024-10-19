using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Basket
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetCustomerBasketAsync (string id);
        Task<CustomerBasketDto> UpdateCustomerBasketAsync (CustomerBasketDto basketDto);
        Task DeleteCustomerBasketAsync (string id);
    }
}
