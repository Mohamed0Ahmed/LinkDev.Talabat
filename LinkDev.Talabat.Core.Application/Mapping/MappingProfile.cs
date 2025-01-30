using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Common;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Domain.Entities.Baskets;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class MappingProfile : Profile
    {

        

        public MappingProfile()
        {

            CreateMap<Product , ProductDisplayDto>()
                .ForMember(d=>d.Brand, O=>O.MapFrom(src=>src.Brand!.Name))
                .ForMember(d=>d.Category, O=>O.MapFrom(src=>src.Category!.Name))
                //.ForMember(d=>d.PictureUrl , o => o.MapFrom(s => $"{"https://localhost:7187"}{s.PictureUrl}"));
                .ForMember(d => d.PictureUrl , o=> o.MapFrom<ProductPictureResolver>() );






            CreateMap<ProductBrand , BrandDto > ();
            CreateMap<ProductCategory, CategoryDto>();


            CreateMap<CustomerBasket , CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest=>dest.DeliveryMethod,options=>options.MapFrom(src=>src.DeliveryMethod!.ShortName));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>());



            CreateMap<Address, AddressDto>();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
