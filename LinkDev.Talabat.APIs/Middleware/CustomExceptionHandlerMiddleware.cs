using Azure;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.Core.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace LinkDev.Talabat.APIs.Middleware
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

            }
            catch (Exception ex)
            {

                #region Logging : TODO

                if (_webHostEnvironment.IsDevelopment())
                {

                    // Development Mode
                    _logger.LogError(ex, ex.Message);

                }
                else
                {
                    // Production Mode
                }

                #endregion


                await HandleExceptionAsync(httpContext, ex);

            }
        }






        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
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

                case ValidationException validationException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiValidationErrorResponse( ex.Message) { Errors = validationException.Errors};

                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case BadRequestException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(400, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case UnauthorizedException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(401, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                default:

                    response = _webHostEnvironment.IsDevelopment() ?
                        new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString())
                        :
                        new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);


                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                    break;

            }
        }



    }
}
