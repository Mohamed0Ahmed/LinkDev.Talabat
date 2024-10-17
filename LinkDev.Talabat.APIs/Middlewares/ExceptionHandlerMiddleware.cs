using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Controllers.Exceptions;
using System.Net;
using System.Text.Json;

namespace LinkDev.Talabat.APIs.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<CustomExceptionHandlerMiddleware> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {

                await _next(httpContext);

                //if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                //{
                //    var response = new ApiResponse((int)HttpStatusCode.NotFound, $"The requested endpoint {httpContext.Request.Path} is not found");
                //    await httpContext.Response.WriteAsync(response.ToString());
                //}
            }
            catch (Exception ex)
            {
                ApiResponse response;
                switch (ex)
                {

                    case NotFoundException:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        httpContext.Response.ContentType = "application/json";

                        response = new ApiResponse(404, ex.Message);

                        await httpContext.Response.WriteAsync(response.ToString());
                        break;

                    default:

                        if (_webHostEnvironment.IsDevelopment())
                        {

                            // Development Mode
                            _logger.LogError(ex, ex.Message);
                            response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString());

                        }
                        else
                        {
                            // Production Mode
                            response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString());
                        }

                        httpContext.Response.StatusCode = 500;
                        httpContext.Response.ContentType = "application/json";

                        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                        break;

                }




            }
        }
    }
}
