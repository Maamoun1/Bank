using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbCountry
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<TbPerson> People { get; set; } = new List<TbPerson>();
}
