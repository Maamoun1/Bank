using ApiBank.DTOs.Requests;
using ApiBank.DTOs.Responses;
using BusinessLayer.Security;
using DataAccessLayer.Respository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly TokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, TokenService tokenService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {

            if(string.IsNullOrWhiteSpace(request.Username)  || string.IsNullOrWhiteSpace(request.Password))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message="Username and password are required.."
                };
            }

            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess=false,
                    Message="Invalid username or password"
                };
            }

            if(!user.IsActive)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Account is inactive or disabled"
                };
            }

            bool passwordIsCorrect = _passwordHasher.VerifyPassword(request.Password, user.Password);

            if(!passwordIsCorrect)
            {
                return new LoginResponse
                {
                    IsSuccess=false,
                    Message="Invalid Username or password"
                };
            }

            var (token, expiresAt) = _tokenService.GenerateTokenWithExpiry(user);


            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Login Successful",
                AccessToken = token,
                ExpiresAt = expiresAt
            };

        }
    }
}
