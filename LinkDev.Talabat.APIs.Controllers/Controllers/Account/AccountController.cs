﻿using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Account
{
    public class AccountController : BaseApiController
    {
        private readonly IServiceManager _serviceManager;

        public AccountController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


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





    }
}
