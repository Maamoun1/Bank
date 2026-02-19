using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tokens.Helpers
{
    public class TokenHelpers
    {

        public static string GenerateRefreshToken(int byteLength = 32)
        {

            if (byteLength <= 0)
                throw new ArgumentException("Byte length must be positive.", nameof(byteLength));


            var randomBytes = new byte[byteLength];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);

        }



    }
}
