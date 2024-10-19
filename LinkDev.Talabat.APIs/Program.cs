using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Middlewares;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using LinkDev.Talabat.Infrastructure;

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

            webApplicationBuilder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value!.Errors.Count > 0)
                                                          .Select(p => new ApiValidationErrorResponse.ValidationError()
                                                          {
                                                              Field = p.Key,
                                                              Errors = p.Value!.Errors.Select(e => e.ErrorMessage)
                                                          });

                    return new BadRequestObjectResult(new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    });
                };
            });



            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            webApplicationBuilder.Services.AddHttpContextAccessor();
            webApplicationBuilder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            webApplicationBuilder.Services.AddApplicationServices();
            webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);



            #endregion



            var app = webApplicationBuilder.Build();


            #region  Databases Initilaztion And Data Seeding

            await app.InitializeStoreContext();



            #endregion




            #region Configure Kestrel Middleware


            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseStaticFiles();
            app.UseAuthorization();


            app.MapControllers();

            #endregion




            app.Run();
        }
    }
}
