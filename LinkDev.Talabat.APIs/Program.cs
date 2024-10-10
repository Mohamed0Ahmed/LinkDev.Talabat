
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.APIs
{
    public class Program
    {

        // Entry Point
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);


            #region Configure Services


            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            webApplicationBuilder.Services.AddHttpContextAccessor();
            webApplicationBuilder.Services.AddScoped<ILoggedInUserService , LoggedInUserService>();

            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);


            #endregion



            var app = webApplicationBuilder.Build();


            #region  Databases Initilaztion And Data Seeding

             await app.InitializeStoreContext();

            

            #endregion




            #region Configure Kestrel Middleware


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            #endregion




            app.Run();
        }
    }
}
