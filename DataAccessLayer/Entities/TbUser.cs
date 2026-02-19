using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class TbUser
{
    public int UserId { get; set; }

    public int PersonId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<TbAccount> Accounts { get; set; } = new List<TbAccount>();

    public virtual ICollection<TbApplication> Applications { get; set; } = new List<TbApplication>();

    public virtual ICollection<TbClient> Clients { get; set; } = new List<TbClient>();

    public virtual ICollection<TbLoginRegister> LoginRegisters { get; set; } = new List<TbLoginRegister>();

    public virtual TbPerson Person { get; set; } = null!;

    public virtual ICollection<TbTransferLog> TransferLogs { get; set; } = new List<TbTransferLog>();

    public virtual ICollection<TbUserRole> UserRoles { get; set; } = new List<TbUserRole>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

}
