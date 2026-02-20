using Azure.Core;
using BusinessLayer.Authentication.DTOs;
using BusinessLayer.Authentication.Services;
using BusinessLayer.Tokens.DTOs;
using BusinessLayer.Tokens.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace ApiBank.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController:ControllerBase
    {

        private readonly IAuthService _authService;

        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IAuthService authService, IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
        }

        // 1. LOGIN
        [HttpPost("login")]
        public async Task<IActionResult>Login([FromBody] LoginRequest request)
        {

            var response = await _authService.LoginAsync(request);

            if (!response.IsSuccess)
                return Unauthorized(new { response.Message });

            return Ok(response);
        }

        [HttpPost("refresh")]

        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            var response = await _refreshTokenService.RefreshAsync(request.RefreshToken);

            if(response == null)
                return Unauthorized();


            return Ok(response);

        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { message = "RefreshToken is required" });


            var result = await _refreshTokenService.RevokeAsync(request.RefreshToken);


            if (!result)
                return BadRequest(new { message = "Invalid refresh token" });

            return Ok(new { message = "Logged out Successfully" });

        }


    }
}
