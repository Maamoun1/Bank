using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbCurrency
{
    public int CurrencyId { get; set; }

    public string CountryName { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public double CurrencyRate { get; set; }
}
