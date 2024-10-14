using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
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
               
        }
    }
}
