using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Entities;

namespace BusinessLayer.Tokens.Service
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireInMinutes;

        public TokenService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "Jwt:Key is not configured. " +
                    "Development: run `dotnet user-secrets set \"Jwt:Key\" \"<key>\"`. " +
                    "Production: set the Jwt__Key environment variable.");

            if (Encoding.UTF8.GetByteCount(_key) < 32)
                throw new InvalidOperationException(
                    "Jwt:Key is too short. It must be at least 32 bytes (256 bits) for HMAC-SHA256.");

            _issuer = configuration["Jwt:Issuer"] ?? "BankApi";
            _audience = configuration["Jwt:Audience"] ?? "BankApiUser";
            _expireInMinutes = configuration.GetValue("Jwt:ExpireInMinutes", 30);
        }

        public string GenerateToken(TbUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            if (user.UserRoles != null && user.UserRoles.Any())
            {
                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null && !string.IsNullOrEmpty(userRole.Role.RoleName))
                        claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                }
            }

            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireInMinutes),
                signingCredentials: signingCredentials);

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