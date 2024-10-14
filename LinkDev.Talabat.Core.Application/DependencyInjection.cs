using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddAutoMapper(Mapper => Mapper.AddProfile<MappingProfile>());
            services.AddAutoMapper(typeof(MappingProfile));


            //services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IServiceManager, ServiceManager>();


            return services;
        }
    }
}
