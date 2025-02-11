using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Accounts
{
    public class ReteriveAccountDto
    {
        public int AccountId {  get; set; }
        public string PinCode {  get; set; }
        public string IssueReason {  get; set; }
        public double Balance {  get; set; }
        public bool IsActive { get; set; }
    }
}
