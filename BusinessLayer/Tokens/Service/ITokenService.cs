using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tokens.Service
{
    public interface ITokenService
    {
        string GenerateToken(TbUser user);

        (string Token, DateTime ExpiresAt) GenerateTokenWithExpiry(TbUser user);

    }
}
