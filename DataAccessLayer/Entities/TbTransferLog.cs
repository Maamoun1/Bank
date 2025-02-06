using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbTransferLog
{
    public int TransferLogId { get; set; }

    public int SourceClientId { get; set; }

    public int DestinationClientId { get; set; }

    public double Amount { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime? DatetimeNow { get; set; }

    public virtual TbUser CreatedByUser { get; set; } = null!;

    public virtual TbClient DestinationClient { get; set; } = null!;

    public virtual TbClient SourceClient { get; set; } = null!;
}
