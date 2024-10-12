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
                .ForMember(d=>d.Category, O=>O.MapFrom(src=>src.Category!.Name));





            CreateMap<ProductBrand , BrandDto > ();

            CreateMap<ProductCategory, CategoryDto>();
               
        }
    }
}
