using BusinessLayer.Authentication.DTOs;
using BusinessLayer.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController:ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult>Login([FromBody] LoginRequest request)
        {

            var response = await _authService.LoginAsync(request);

            if (!response.IsSuccess)
                return Unauthorized(new { response.Message });



            return Ok(response);
        }
    }
}
