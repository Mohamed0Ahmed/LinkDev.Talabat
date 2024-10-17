﻿namespace LinkDev.Talabat.APIs.Controllers.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public required IEnumerable<string > Errors { get; set; }

        public ApiValidationErrorResponse(string? message = null)
            :base (400 , message)
        {
            
        }
    }
}
