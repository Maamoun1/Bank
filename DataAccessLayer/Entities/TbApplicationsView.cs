using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbApplicationsView
{
    public int ApplicationId { get; set; }

    public string NationalNo { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public DateTime ApplicantDate { get; set; }

    public byte? ApplicationStatus { get; set; }
}
