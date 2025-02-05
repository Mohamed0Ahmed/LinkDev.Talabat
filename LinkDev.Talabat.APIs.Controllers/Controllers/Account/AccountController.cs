using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager) : BaseApiController
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        [HttpPost("login")]         //POST   :   /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var response = await _serviceManager.AuthServices.LoginAsync(model);
            return Ok(response);
        }


        [HttpPost("register")]      //POST   :   /api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var response = await _serviceManager.AuthServices.RegisterAsync(model);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]                //GET   :   /api/account/register
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var result = await _serviceManager.AuthServices.GetCurrentUser(User);
            return Ok(result);
        }


        [HttpGet("address")]    //GET   :   /api/account/address
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var result = await _serviceManager.AuthServices.GetUserAddress(User);
            return Ok(result);
        }

        [HttpPut("address")]    //PUT   :   /api/account/address
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {

            var result = await _serviceManager.AuthServices.UpdateUserAddress(User, address);
            return Ok(result);
        }




    }
}
