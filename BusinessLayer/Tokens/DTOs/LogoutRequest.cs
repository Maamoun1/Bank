using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tokens.DTOs
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = null!;
    }

}
