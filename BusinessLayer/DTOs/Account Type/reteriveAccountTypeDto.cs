using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Account_Type
{
    public class reteriveAccountTypeDto
    {

        public int AccountClassesId { get; set; }

        public string AccountName { get; set; } = null!;

        public string AccountDescription { get; set; } = null!;

        public int MinummAllowAge { get; set; }

        public int DefaultValidityLength { get; set; }

        public double AccountFees { get; set; }

    }
}
