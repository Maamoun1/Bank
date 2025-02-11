using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Accounts
{
    public class CreateAccountDto
    {

        public int ApplicationId { get; set; }

        public int ClientId { get; set; }

        public string IssueReason { get; set; } = null!;

        public int? CreatedByUserId { get; set; }

    }
}
