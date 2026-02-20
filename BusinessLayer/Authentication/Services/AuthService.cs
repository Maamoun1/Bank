using Azure;
using BusinessLayer.Authentication.DTOs;
using BusinessLayer.Security;
using BusinessLayer.Tokens.Service;
using DataAccessLayer.Respository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IRefreshTokenService _refreshTokenService;


        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,IRefreshTokenService refreshTokenService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _refreshTokenService = refreshTokenService;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {

            //  Validate input
            if (string.IsNullOrWhiteSpace(request.Username)  || string.IsNullOrWhiteSpace(request.Password))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message="Username and password are required.."
                };
            }

            //  Get user
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess=false,
                    Message="Invalid username or password"
                };
            }

            // 3️ Check active
            if (!user.IsActive)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Account is inactive or disabled"
                };
            }

            //  Verify password
            bool passwordIsCorrect = _passwordHasher.VerifyPassword(request.Password, user.Password);

            if(!passwordIsCorrect)
            {
                return new LoginResponse
                {
                    IsSuccess=false,
                    Message="Invalid Username or password"
                };
            }

            // 5. Generate AccessToken + RefreshToken
            var tokens = await _refreshTokenService.GenerateTokensAsync(user);

            // 6.Return response
            return new LoginResponse
            {

                IsSuccess = true,
                Message="Login Successful",

                AccessToken= tokens.AccessToken,
                AccessTokenExpiresAt=tokens.AccessTokenExpiresAt,

                RefreshToken=tokens.RefreshToken,
                RefreshTokenExpiresAt=tokens.RefreshTokenExpiresAt

            };


        }

        public async Task LogoutAsync(int userId)
        {
            await _refreshTokenService.RevokeAllAsync(userId);

        }
    }
}
