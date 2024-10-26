using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Middleware;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.Core.Domain.Entities.Identities;
using LinkDev.Talabat.Infrastructure;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
                        Errors = (IEnumerable<string>)errors
                    });
                };
            });



            webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

            webApplicationBuilder.Services.AddHttpContextAccessor().AddScoped<ILoggedInUserService, LoggedInUserService>();

            webApplicationBuilder.Services.AddApplicationServices();
            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);
         
            #endregion



            var app = webApplicationBuilder.Build();


            #region  Databases Initilaztion And Data Seeding

            await app.InitializeDbAsync();



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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion




            app.Run();
        }
    }
}
