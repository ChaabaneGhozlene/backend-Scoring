using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace scoring_Backend.Models.Admin;

public partial class SqrAdminContext : DbContext
{
    public SqrAdminContext()
    {
    }

    public SqrAdminContext(DbContextOptions<SqrAdminContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<Control> Controls { get; set; }

    public virtual DbSet<ListCustomer> ListCustomers { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Profil> Profils { get; set; }

    public virtual DbSet<ProfilControl> ProfilControls { get; set; }

    public virtual DbSet<ProfilMenu> ProfilMenus { get; set; }

    public virtual DbSet<Trace20170707> Trace20170707s { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersAgent> UsersAgents { get; set; }

    public virtual DbSet<UsersApplication> UsersApplications { get; set; }

    public virtual DbSet<UsersCampagne> UsersCampagnes { get; set; }

    public virtual DbSet<UsersRole> UsersRoles { get; set; }

    public virtual DbSet<UsersSite> UsersSites { get; set; }

    public virtual DbSet<UsersType> UsersTypes { get; set; }

    public virtual DbSet<ViewListHermesAdmin> ViewListHermesAdmins { get; set; }

    public virtual DbSet<ViewListHermesAdminsSite> ViewListHermesAdminsSites { get; set; }

    public virtual DbSet<ViewListHermesAgent> ViewListHermesAgents { get; set; }

    public virtual DbSet<ViewListHermesCampaign> ViewListHermesCampaigns { get; set; }

    public virtual DbSet<ViewListHermesSalesMan> ViewListHermesSalesMen { get; set; }

    public virtual DbSet<ViewListHermesSupervisorAgent> ViewListHermesSupervisorAgents { get; set; }

    public virtual DbSet<ViewListHermesSupervisorCampaign> ViewListHermesSupervisorCampaigns { get; set; }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("Application");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Com)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Connection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Connection1");

            entity.ToTable("Connection");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.Base)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DatabaseConnection)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Server)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Control>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.ControlId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ControlID");
            entity.Property(e => e.ControlText)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ControlType)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PageId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PageID");
            entity.Property(e => e.PageTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PageUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ListCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListCustomers");

            entity.Property(e => e.AcdOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .UseCollation("French_BIN");
            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("createat");
            entity.Property(e => e.CrmOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid).HasColumnName("customerOid");
            entity.Property(e => e.DatabaseManagerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.DefaultPhoneNumber)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Deleteat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("deleteat");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FlashMediaOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ManagerEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ManagerName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ManagerTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.OnMediaServiceOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.OutExclude)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ProxyOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ProxySipOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.RangeAgents)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.RangeDids)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.RangeQueues)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.RangeStations)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.RecordsFileTemplate)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ReportingOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RfbServerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.ScriptFramesetName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScriptFramesetUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScripterOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SupervisionOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VmcOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.ItemText)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ItemUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MenuId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MenuID");
            entity.Property(e => e.MenuName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profil>(entity =>
        {
            entity.ToTable("Profil");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.Com)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProfilControl>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ControlsId).HasColumnName("ControlsID");
            entity.Property(e => e.ProfilId).HasColumnName("ProfilID");
        });

        modelBuilder.Entity<ProfilMenu>(entity =>
        {
            entity.ToTable("ProfilMenu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.ProfilId).HasColumnName("ProfilID");
        });

        modelBuilder.Entity<Trace20170707>(entity =>
        {
            entity.HasKey(e => e.RowNumber).HasName("PK__trace201__AAAC09D8DE73F2D5");

            entity.ToTable("trace20170707");

            entity.Property(e => e.ApplicationName).HasMaxLength(128);
            entity.Property(e => e.BinaryData).HasColumnType("image");
            entity.Property(e => e.ClientProcessId).HasColumnName("ClientProcessID");
            entity.Property(e => e.Cpu).HasColumnName("CPU");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.LoginName).HasMaxLength(128);
            entity.Property(e => e.NtuserName)
                .HasMaxLength(128)
                .HasColumnName("NTUserName");
            entity.Property(e => e.Spid).HasColumnName("SPID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TextData).HasColumnType("ntext");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users1");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.SiteId).HasColumnName("SiteID");
            entity.Property(e => e.SiteName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<UsersAgent>(entity =>
        {
            entity.ToTable("UsersAgent");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SiteOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<UsersApplication>(entity =>
        {
            entity.ToTable("UsersApplication");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.ProfilId).HasColumnName("ProfilID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UsersCampagne>(entity =>
        {
            entity.ToTable("UsersCampagne");

            entity.Property(e => e.CampagneId).HasMaxLength(64);
            entity.Property(e => e.CampagneOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SiteOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<UsersRole>(entity =>
        {
            entity.ToTable("UsersRole");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsersSite>(entity =>
        {
            entity.ToTable("UsersSite");

            entity.Property(e => e.SiteOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<UsersType>(entity =>
        {
            entity.ToTable("UsersType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewListHermesAdmin>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesAdmins");

            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("createat");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("customer");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("customerOid");
            entity.Property(e => e.Deleteat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("deleteat");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsSuperAdmin).HasColumnName("isSuperAdmin");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Login)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ViewListHermesAdminsSite>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesAdminsSites");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Login)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UserOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
        });

        modelBuilder.Entity<ViewListHermesAgent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesAgents");

            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("createat");
            entity.Property(e => e.Ctiskills)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text")
                .HasColumnName("CTISkills");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("customer");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("customerOid");
            entity.Property(e => e.Deleteat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("deleteat");
            entity.Property(e => e.IsSupervisor).HasColumnName("isSupervisor");
            entity.Property(e => e.LoginName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Options)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OutCampaigns)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Profile)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ReceptionGrp)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.Rights)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScriptFramesetName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScriptFramesetUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WorkspaceOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
        });

        modelBuilder.Entity<ViewListHermesCampaign>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesCampaigns");

            entity.Property(e => e.AcdFax)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AgentMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AnsweringMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AutoRecord)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AvayaVdn).HasColumnName("AvayaVDN");
            entity.Property(e => e.CallAbandonParam)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.CallAnsweringMachineParam)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.CallBackDeadLineDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CallBusyParam)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.CallClassificationNumbers)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CallDisturbedParam)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.CallMissedParam)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.CallNoAnswerParam)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CallTransitionNumbers)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg3)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg4)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg5)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg6)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClosingMsg7)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Crmoid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("CRMOid");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("CustomerOID");
            entity.Property(e => e.Dbname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DBName");
            entity.Property(e => e.DefLanguage)
                .HasMaxLength(8)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Did)
                .HasMaxLength(64)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DID");
            entity.Property(e => e.HoldMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.HolidayId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("HolidayID");
            entity.Property(e => e.MailPwd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MailTemplateOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.MailUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MenuMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.NoAgentMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.OpeningId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("OpeningID");
            entity.Property(e => e.OutExclude)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OutOperator)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OutPrefix)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OverflowMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Patience)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PhoneDisplaySpecific)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.ProfilCampaign)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("Profil_Campaign");
            entity.Property(e => e.Profile)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.QuerySchedulerId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.QueueId).HasColumnName("QueueID");
            entity.Property(e => e.QueueSecondId).HasColumnName("QueueSecondID");
            entity.Property(e => e.RouteDest)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RouteFailedMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RouteMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScriptAddress)
                .HasMaxLength(512)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScriptName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ScriptParams)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TelByeMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TelCheckMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TelConfirmMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TelErrorMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TelInputMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TransferMode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoiceByeMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoiceCheckMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoiceConfirmMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoiceInputMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoiceScript)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoiceScriptName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VoicemailConnection)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.VoicemailTable)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WaitMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WaitTimeMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WelcomeMsg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ViewListHermesSalesMan>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesSalesMan");

            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Cp)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CP");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("customer");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Expr1)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Expr3)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.IsPrimaryLastChange)
                .HasMaxLength(8)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone1)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone2)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ViewListHermesSupervisorAgent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesSupervisorAgents");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.SupervisorOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
        });

        modelBuilder.Entity<ViewListHermesSupervisorCampaign>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ListHermesSupervisorCampaigns");

            entity.Property(e => e.CampaignId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CampaignOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.SupervisorOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
