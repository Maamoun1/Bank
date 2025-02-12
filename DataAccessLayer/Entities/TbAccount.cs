using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public  class TbAccount
{
    public int AccountId { get; set; }

    public int ApplicationId { get; set; }

    public int ClientId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime IssueDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string IssueReason { get; set; } = null!;

    public double Balance { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedByUserId { get; set; }

    public virtual TbApplication Application { get; set; } = null!;

    public virtual TbClient Client { get; set; } = null!;

    public virtual TbUser? CreatedByUser { get; set; }
}
