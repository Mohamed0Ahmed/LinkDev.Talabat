using LinkDev.Talabat.Core.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddAutoMapper(Mapper => Mapper.AddProfile<MappingProfile>());
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
