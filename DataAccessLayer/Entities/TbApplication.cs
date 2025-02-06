using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbApplication
{
    public int ApplicationId { get; set; }

    public int ApplicantPersonId { get; set; }

    public int AccountClassId { get; set; }

    public DateTime ApplicantDate { get; set; }

    public DateTime LastStatusDate { get; set; }

    public int PaidFees { get; set; }

    public int? CreatedByUserId { get; set; }

    public byte? ApplicationStatus { get; set; }

    public virtual TbAccountsType AccountClass { get; set; } = null!;

    public virtual ICollection<TbAccount> Accounts { get; set; } = new List<TbAccount>();

    public virtual TbPerson ApplicantPerson { get; set; } = null!;

    public virtual TbUser? CreatedByUser { get; set; }
}
