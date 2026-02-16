using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string plianPassword);

        bool VerifyPassword(string enteredPassword, string storedHash);
    }
}
