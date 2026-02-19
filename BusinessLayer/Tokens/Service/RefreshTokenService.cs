using BusinessLayer.Tokens.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Tokens.Service;
using DataAccessLayer.Respository.IRepository;
using BusinessLayer.Tokens.Helpers;
using Microsoft.VisualBasic;

namespace BusinessLayer.Tokens.Service
{
    public class RefreshTokenService : IRefreshTokenService
    {

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private readonly ITokenService _tokenService;

        private readonly IUserRepository _userRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService, IUserRepository userRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }


        public async Task<TokenResponse> GenerateTokensAsync(TbUser user)
        {

            var (accessToken, expiresAt) = _tokenService.GenerateTokenWithExpiry(user);

            var refreshToken = TokenHelpers.GenerateRefreshToken();

            var tokenHash = TokenHasher.HashToken(refreshToken);

            var entity = new RefreshToken
            {

                UserId = user.UserId,
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt=DateTime.UtcNow.AddDays(7),
                IsRevoked=false
            };

            await _refreshTokenRepository.AddAsync(entity);


            return new TokenResponse
            {

                 AccessToken = accessToken,
                 RefreshToken=refreshToken,
                 ExpireAt=expiresAt

            };
        }

        public async Task RevokeAllAsync(int userId)
        {

            await _refreshTokenRepository.RevokeAllForUserAsync(userId);

        }

        public async Task<TokenResponse?> RefreshAsync(string refreshToken)
        {

            var tokenHash= TokenHasher.HashToken(refreshToken);

            var existingToken = await _refreshTokenRepository.GetByTokenAsync(tokenHash);


            if (existingToken == null)
                return null;

            if (!existingToken.IsRevoked)
                return null;

            if (existingToken.ExpiresAt < DateTime.UtcNow)
                return null;


            var user = await _userRepository.GetByIdAsync(existingToken.UserId);


            if (user == null)
                return null;

            existingToken.IsRevoked = true;
            existingToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(existingToken);


            return await GenerateTokensAsync(user);




        }
    }
}
