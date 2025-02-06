using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbClient
{
    public int ClientId { get; set; }

    public int PersonId { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreatedByUserId { get; set; }

    public virtual ICollection<TbAccount> Accounts { get; set; } = new List<TbAccount>();

    public virtual TbUser CreatedByUser { get; set; } = null!;

    public virtual TbPerson Person { get; set; } = null!;

    public virtual ICollection<TbTransferLog> TransferLogDestinationClients { get; set; } = new List<TbTransferLog>();

    public virtual ICollection<TbTransferLog> TransferLogSourceClients { get; set; } = new List<TbTransferLog>();
}
