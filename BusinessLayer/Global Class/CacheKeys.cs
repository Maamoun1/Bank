using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Global_Class
{
    public static class CacheKeys
    {

        public static string Balance(string accountNumber)
        {
            return $"balance:{accountNumber}";
        }




    }
}
