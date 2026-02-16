using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace BusinessLayer.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string plianPassword)
        {
            if (string.IsNullOrWhiteSpace(plianPassword))
                throw new ArgumentException("Password can not be empty.");

            return BCrypt.Net.BCrypt.HashPassword(plianPassword);
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {

            if (string.IsNullOrWhiteSpace(enteredPassword) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);

        }

    }
}
