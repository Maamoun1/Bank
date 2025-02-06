using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class TbAccountsType
    {
        public int AccountClassesId { get; set; }

        public string AccountName { get; set; } = null!;

        public string AccountDescription { get; set; } = null!;

        public int MinummAllowAge { get; set; }

        public int DefaultValidityLength { get; set; }

        public double AccountFees { get; set; }

        public virtual ICollection<TbApplication> Applications { get; set; } = new List<TbApplication>();
    }
}
