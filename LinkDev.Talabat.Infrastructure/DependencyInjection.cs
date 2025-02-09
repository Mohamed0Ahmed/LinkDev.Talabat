using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Infrastructure.BasketRepo;
using LinkDev.Talabat.Infrastructure.PaymentServices;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LinkDev.Talabat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString!);
                return connectionMultiplexer;
            });

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
