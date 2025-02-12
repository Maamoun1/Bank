using System;
using System.Collections.Generic;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContext;

public partial class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccount> Accounts { get; set; }

    public virtual DbSet<TbAccountsType> AccountsTypes { get; set; }

    public virtual DbSet<TbApplication> Applications { get; set; }

    public virtual DbSet<TbApplicationsView> ApplicationsViews { get; set; }

    public virtual DbSet<TbClient> Clients { get; set; }

    public virtual DbSet<TbCountry> Countries { get; set; }

    public virtual DbSet<TbCurrency> Currencies { get; set; }

    public virtual DbSet<TbLoginRegister> LoginRegisters { get; set; }

    public virtual DbSet<TbPerson> People { get; set; }

    public virtual DbSet<TbTransferLog> TransferLogs { get; set; }

    public virtual DbSet<TbUser> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-9A4HLOV;Database=Bank;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA58643E85251");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.IssueReason).HasMaxLength(50);
            entity.Property(e => e.Password).HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.AccountNumber).HasMaxLength(40);
            entity.HasOne(d => d.Application).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__Applic__02FC7413");

            entity.HasOne(d => d.Client).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__Client__03F0984C");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK__Accounts__Create__04E4BC85");
        });

        modelBuilder.Entity<TbAccountsType>(entity =>
        {
            entity.HasKey(e => e.AccountClassesId).HasName("PK__AccountC__55636B0E337613A0");

            entity.Property(e => e.AccountClassesId)
                .ValueGeneratedNever()
                .HasColumnName("AccountClassesID");
            entity.Property(e => e.AccountDescription).HasMaxLength(250);
            entity.Property(e => e.AccountName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__Applicat__C93A4F793E225FF2");

            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.AccountClassId).HasColumnName("AccountClassID");
            entity.Property(e => e.ApplicantDate).HasColumnType("datetime");
            entity.Property(e => e.ApplicantPersonId).HasColumnName("ApplicantPersonID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.LastStatusDate).HasColumnType("datetime");

            entity.HasOne(d => d.AccountClass).WithMany(p => p.Applications)
                .HasForeignKey(d => d.AccountClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Applicati__Accou__656C112C");

            entity.HasOne(d => d.ApplicantPerson).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicantPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Applicati__Appli__14270015");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Applications)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK__Applicati__Creat__66603565");
        });

        modelBuilder.Entity<TbApplicationsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Applications_view");

            entity.Property(e => e.AccountName).HasMaxLength(50);
            entity.Property(e => e.ApplicantDate).HasColumnType("datetime");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.FullName).HasMaxLength(41);
            entity.Property(e => e.NationalNo).HasMaxLength(50);
        });

        modelBuilder.Entity<TbClient>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A047E9DBC04");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Clients)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__Created__60A75C0F");

            entity.HasOne(d => d.Person).WithMany(p => p.Clients)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__PersonI__5FB337D6");
        });

        modelBuilder.Entity<TbCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Countrie__10D160BFFE6CE8E7");

            entity.Property(e => e.CountryId)
                .ValueGeneratedNever()
                .HasColumnName("CountryID");
            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbCurrency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currenci__14470B10525C1B47");

            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CountryName).HasMaxLength(50);
            entity.Property(e => e.CurrencyName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbLoginRegister>(entity =>
        {
            entity.HasKey(e => e.LoginRegisterId).HasName("PK__LoginReg__8AE0EB65C06F6702");

            entity.Property(e => e.LoginRegisterId).HasColumnName("LoginRegisterID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.LoginRegisters)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoginRegi__Creat__3C34F16F");
        });

        modelBuilder.Entity<TbPerson>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__People__AA2FFB85012A1F59");

            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.ImagePath).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.NationalNo).HasMaxLength(50);
            entity.Property(e => e.NationalityCountryId).HasColumnName("NationalityCountryID");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.SecondName).HasMaxLength(20);
            entity.Property(e => e.ThirdName).HasMaxLength(20);

            entity.HasOne(d => d.NationalityCountry).WithMany(p => p.People)
                .HasForeignKey(d => d.NationalityCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__People__National__5441852A");
        });

        modelBuilder.Entity<TbTransferLog>(entity =>
        {
            entity.HasKey(e => e.TransferLogId).HasName("PK__Transfer__8B711B7AD2BAA35A");

            entity.ToTable("TransferLog");

            entity.Property(e => e.TransferLogId).HasColumnName("TransferLogID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.DatetimeNow).HasColumnType("datetime");
            entity.Property(e => e.DestinationClientId).HasColumnName("DestinationClientID");
            entity.Property(e => e.SourceClientId).HasColumnName("SourceClientID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.TransferLogs)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferL__Creat__2FCF1A8A");

            entity.HasOne(d => d.DestinationClient).WithMany(p => p.TransferLogDestinationClients)
                .HasForeignKey(d => d.DestinationClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferL__Desti__2EDAF651");

            entity.HasOne(d => d.SourceClient).WithMany(p => p.TransferLogSourceClients)
                .HasForeignKey(d => d.SourceClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TransferL__Sourc__2DE6D218");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF88884EC");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.UserName).HasMaxLength(30);

            entity.HasOne(d => d.Person).WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__PersonID__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
