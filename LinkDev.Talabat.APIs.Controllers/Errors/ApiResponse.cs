using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Errors
{
    public class ApiResponse
    {

        public int StatusCode { get; set; }
        public string? Message { get; set; }


        public ApiResponse(int statusCode, string? message =null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatus(statusCode);
        }

        private string? GetDefaultMessageForStatus(int statusCode)
        {

            return statusCode switch
            {
                400 => "Bad Request , You have made",
                401 => "Unauthorized , You Are Not",
                404 => "Resource was not found",
                500 => "Errors are the path to the dark side. Errors lead to anger",
                _ => null,

            };
        }
    }
}
