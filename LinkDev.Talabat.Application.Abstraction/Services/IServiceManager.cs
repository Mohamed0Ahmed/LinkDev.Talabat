using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Products;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthServices AuthServices { get; }
        public IOrderService OrderService { get; }
    }
}
