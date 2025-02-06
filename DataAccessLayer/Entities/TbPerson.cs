using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbPerson
{

    public int PersonId { get; set; }

    public string NationalNo { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? ThirdName { get; set; }

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public byte Gendor { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public int NationalityCountryId { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<TbApplication> Applications { get; set; } = new List<TbApplication>();

    public virtual ICollection<TbClient> Clients { get; set; } = new List<TbClient>();

    public virtual TbCountry NationalityCountry { get; set; } = null!;

    public virtual ICollection<TbUser> Users { get; set; } = new List<TbUser>();
}
