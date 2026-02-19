using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;


namespace BusinessLayer.Service.IService
{
    public interface IRefreshTokenService
    {

        Task<string> GenerateRefreshTokenAsync(int userId);

        Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(int refreshTokenId);
    }
}
