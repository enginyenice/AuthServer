﻿using AuthServer.Core.Dto;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            //throw new CustomException("Custom bir hata meydana geldi (Örnek)");
            var result = await _authenticationService.CreateToken(loginDto);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _authenticationService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshToken)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshToken.Token);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshToken)
        {
            var result =await _authenticationService.CreateTokenByRefreshToken(refreshToken.Token);
            return ActionResultInstance(result);
        }
    }

}
