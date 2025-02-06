using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbLoginRegister
{
    public int LoginRegisterId { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TbUser CreatedByUser { get; set; } = null!;
}
