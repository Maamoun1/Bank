using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Entities;

namespace BusinessLayer.Auth
{
    public class TokenService
    {

        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireInMinutes;

        public TokenService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is missing in configuration");

            _issuer = configuration["Jwt :Issuer"] ?? "BankApi";

            _audience = configuration["Jwt:Audience"] ?? "BankApiUser";

            _expireInMinutes = configuration.GetValue<int>("Jwt:ExpireInMinutes", 30);

        }

        public string GenerateToken(TbUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),

            };

            if (user.UserRoles != null && user.UserRoles.Any())
            {

                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null && !string.IsNullOrEmpty(userRole.Role.RoleName))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                    }
                }
            }


            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:_issuer,
                audience:_audience,
                claims: claims,
                expires:DateTime.UtcNow.AddMinutes(_expireInMinutes),
                signingCredentials:signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public (string Token, DateTime ExpiresAt) GenerateTokenWithExpiry(TbUser user)
        {
            var token = GenerateToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(_expireInMinutes);
            return (token, expiresAt);
        }




    }
}
