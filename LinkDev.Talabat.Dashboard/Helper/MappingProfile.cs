using AutoMapper;
using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Dashboard.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
