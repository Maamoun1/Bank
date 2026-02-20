using BusinessLayer.Tokens.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tokens.Service
{
    public  interface IRefreshTokenService
    {

        Task<TokenResponse> GenerateTokensAsync(TbUser user);

        Task<TokenResponse?> RefreshAsync(string refreshToken);

        Task RevokeAllAsync(int userId);

        Task<bool> RevokeAsync(string refreshToken);


    }
}
