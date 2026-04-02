using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Accounts
{
    public class ReteriveAccountDto
    {
        public string AccountNumber {  get; set; }

        public string ClientName {  get; set; }
        public string IssueReason {  get; set; }
        public decimal Balance {  get; set; }
        public bool IsActive { get; set; }
    }
}
