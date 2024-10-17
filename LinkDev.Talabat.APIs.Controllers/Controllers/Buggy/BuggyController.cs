using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Controllers.Exceptions;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Buggy
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("notfound")]                    // /api/buggy/notfound
        public IActionResult GetNotFound()
        {

            throw new NotFoundException();
            //return NotFound(new ApiResponse(404));
        }


        [HttpGet("servererror")]                 // /api/buggy/servererror
        public IActionResult GetServerError()
        {
            throw new Exception();      // 500
        }


        [HttpGet("badrequest")]                  // /api/buggy/badrequest
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }


        [HttpGet("badrequest/{id}")]             // /api/buggy/badrequest/5
        public IActionResult GetValidationError(int id)
        {

            return Ok();
        }


        [HttpGet("unauthorize")]                 // /api/buggy/unauthorize
        public IActionResult GetUnAuthorizeError()
        {
            return Unauthorized(new ApiResponse(401));
        }


        [HttpGet("forbidden")]                   // /api/buggy/forbidden
        public IActionResult GetForbiddenRequest()
        {
            return Forbid();
        }


        [Authorize]
        [HttpGet("authorized")]                  // /api/buggy/authorized
        public IActionResult GetAuthorizeRequest()
        {
            return Ok();
        }
    }
}