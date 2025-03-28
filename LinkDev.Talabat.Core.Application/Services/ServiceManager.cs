using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using Microsoft.Extensions.Configuration;
namespace LinkDev.Talabat.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthServices> _authServices;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration,
            Func<IBasketService> basketServiceFactory,
            Func<IOrderService> orderServiceFactory,
            Func<IAuthServices> authServiceFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;


            _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
            _basketService = new Lazy<IBasketService>(basketServiceFactory , LazyThreadSafetyMode.ExecutionAndPublication);
            _authServices = new Lazy<IAuthServices>(authServiceFactory , LazyThreadSafetyMode.ExecutionAndPublication);
            _orderService = new Lazy<IOrderService>(orderServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        }


        public IProductService ProductService => _productService.Value;
        public IBasketService BasketService => _basketService.Value;
        public IAuthServices AuthServices => _authServices.Value;
        public IOrderService OrderService => _orderService.Value;
    }
}
