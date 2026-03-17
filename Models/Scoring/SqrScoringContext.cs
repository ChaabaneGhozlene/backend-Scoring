using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace scoring_Backend.Models.Scoring;

public partial class SqrScoringContext : DbContext
{
    public SqrScoringContext()
    {
    }

    public SqrScoringContext(DbContextOptions<SqrScoringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApAction> ApActions { get; set; }

    public virtual DbSet<ApEvent> ApEvents { get; set; }

    public virtual DbSet<CallType> CallTypes { get; set; }

    public virtual DbSet<Config> Configs { get; set; }

    public virtual DbSet<ConfigGroupe> ConfigGroupes { get; set; }

    public virtual DbSet<EvalType> EvalTypes { get; set; }

    public virtual DbSet<EvalUser> EvalUsers { get; set; }

    public virtual DbSet<Filtre> Filtres { get; set; }

    public virtual DbSet<FiltreGroupe> FiltreGroupes { get; set; }

    public virtual DbSet<FiltreType> FiltreTypes { get; set; }

    public virtual DbSet<HistoriqueDium> HistoriqueDia { get; set; }

    public virtual DbSet<HistoriqueFium> HistoriqueFia { get; set; }

    public virtual DbSet<ImportSatu> ImportSatus { get; set; }

    public virtual DbSet<ImportType> ImportTypes { get; set; }

    public virtual DbSet<L> Ls { get; set; }

    public virtual DbSet<ListAgent> ListAgents { get; set; }

    public virtual DbSet<ListCallStatus> ListCallStatuses { get; set; }

    public virtual DbSet<ListCallType> ListCallTypes { get; set; }

    public virtual DbSet<ListCampaign> ListCampaigns { get; set; }

    public virtual DbSet<ListCustomer> ListCustomers { get; set; }

    public virtual DbSet<LsCallReason> LsCallReasons { get; set; }

    public virtual DbSet<LsCalledCampaign> LsCalledCampaigns { get; set; }

    public virtual DbSet<LsCategoriesBkp20190130> LsCategoriesBkp20190130s { get; set; }

    public virtual DbSet<LsCategory> LsCategories { get; set; }

    public virtual DbSet<LsEvalAuditor> LsEvalAuditors { get; set; }

    public virtual DbSet<LsScoreSection> LsScoreSections { get; set; }

    public virtual DbSet<LsSurvey> LsSurveys { get; set; }

    public virtual DbSet<LsSurveyBkp20190130> LsSurveyBkp20190130s { get; set; }

    public virtual DbSet<LsSurveyItem> LsSurveyItems { get; set; }

    public virtual DbSet<LsTemplate> LsTemplates { get; set; }

    public virtual DbSet<LsTemplateItem> LsTemplateItems { get; set; }

    public virtual DbSet<LsTemplateItemGroup> LsTemplateItemGroups { get; set; }

    public virtual DbSet<LsTemplatePeriode> LsTemplatePeriodes { get; set; }

    public virtual DbSet<LsTemplateType> LsTemplateTypes { get; set; }

    public virtual DbSet<MgrhConfDock> MgrhConfDocks { get; set; }

    public virtual DbSet<Odcall> Odcalls { get; set; }

    public virtual DbSet<Odcallsback> Odcallsbacks { get; set; }

    public virtual DbSet<RecordBkp> RecordBkps { get; set; }

    public virtual DbSet<RecordDataRef> RecordDataRefs { get; set; }

    public virtual DbSet<RecordDataRef1> RecordDataRef1s { get; set; }

    public virtual DbSet<RecordDataRefAddc> RecordDataRefAddcs { get; set; }

    public virtual DbSet<RecordDataRefAddc201812> RecordDataRefAddc201812s { get; set; }

    public virtual DbSet<RecordDataRefOld> RecordDataRefOlds { get; set; }

    public virtual DbSet<RecordDatum> RecordData { get; set; }

    public virtual DbSet<RecordFile> RecordFiles { get; set; }

    public virtual DbSet<RecordStatus> RecordStatuses { get; set; }

    public virtual DbSet<Recordcheck> Recordchecks { get; set; }

    public virtual DbSet<StFileTemplateKey> StFileTemplateKeys { get; set; }

    public virtual DbSet<StFrequency> StFrequencies { get; set; }

    public virtual DbSet<StFtp> StFtps { get; set; }

    public virtual DbSet<StJob> StJobs { get; set; }

    public virtual DbSet<StLevelConversion> StLevelConversions { get; set; }

    public virtual DbSet<StLog> StLogs { get; set; }

    public virtual DbSet<StNotificationEvent> StNotificationEvents { get; set; }

    public virtual DbSet<StStatus> StStatuses { get; set; }

    public virtual DbSet<TAgentTeam> TAgentTeams { get; set; }

    public virtual DbSet<TCallType> TCallTypes { get; set; }

    public virtual DbSet<TListAgent> TListAgents { get; set; }

    public virtual DbSet<TListAgentEmail> TListAgentEmails { get; set; }

    public virtual DbSet<TListAgentTeam> TListAgentTeams { get; set; }

    public virtual DbSet<TListCallStatus> TListCallStatuses { get; set; }

    public virtual DbSet<TListCampaign> TListCampaigns { get; set; }

    public virtual DbSet<TOdcall> TOdcalls { get; set; }

    public virtual DbSet<TagEcoute> TagEcoutes { get; set; }

    public virtual DbSet<Transfert> Transferts { get; set; }

    public virtual DbSet<Trecorddatum> Trecorddata { get; set; }

    public virtual DbSet<TtOdcall> TtOdcalls { get; set; }

    public virtual DbSet<TypeRequalif> TypeRequalifs { get; set; }

    public virtual DbSet<ViewRecordSync> ViewRecordSyncs { get; set; }

    public virtual DbSet<ViewRecordSync1> ViewRecordSync1s { get; set; }

    public virtual DbSet<ViewSyncImoprt> ViewSyncImoprts { get; set; }

    public virtual DbSet<VpAction> VpActions { get; set; }

    public virtual DbSet<VwlistLsSurveyGv> VwlistLsSurveyGvs { get; set; }

    public virtual DbSet<VwlistRecordGv> VwlistRecordGvs { get; set; }

    public virtual DbSet<VwlistRecordGvsource> VwlistRecordGvsources { get; set; }

    public virtual DbSet<XViewDailyImport> XViewDailyImports { get; set; }

    public virtual DbSet<XViewRecordDaily> XViewRecordDailies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SQR_REC;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("ap_action");

            entity.HasIndex(e => e.RecordedId, "ap_action_indx").HasFillFactor(1);

            entity.HasIndex(e => e.CodeEvent, "ap_action_indx1");

            entity.Property(e => e.Comment).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.RecordFileName)
                .HasMaxLength(200)
                .HasComment("path of the audio file");
            entity.Property(e => e.RecordPosition)
                .HasMaxLength(50)
                .HasComment("current audio position in seconde");
            entity.Property(e => e.RecordedId).HasComment("audio file Id on RecordingTool");
        });

        modelBuilder.Entity<ApEvent>(entity =>
        {
            entity.HasKey(e => e.Code).HasFillFactor(1);

            entity.ToTable("ap_event");

            entity.Property(e => e.Code).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<CallType>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CallType1).HasColumnName("CallType");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Config>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Config");

            entity.Property(e => e.Layout).IsUnicode(false);
            entity.Property(e => e.NomConfig)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ConfigGroupe>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("ConfigGroupe");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EvalType>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("EvalType");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EvalUser>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.DateEval)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Date_Eval");
            entity.Property(e => e.Idauditor).HasColumnName("IDAuditor");
        });

        modelBuilder.Entity<Filtre>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Filtre");

            entity.HasIndex(e => e.Type, "iType_Filtre").HasFillFactor(1);

            entity.Property(e => e.DateCreation)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Expression)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NomFiltre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SqlWhere)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.GroupeNavigation).WithMany(p => p.Filtres)
                .HasForeignKey(d => d.Groupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Type");
        });

        modelBuilder.Entity<FiltreGroupe>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("FiltreGroupe");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FiltreType>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("FiltreType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HistoriqueDium>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_HistoriqueImport_1")
                .HasFillFactor(1);

            entity.ToTable("HistoriqueDIA");

            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<HistoriqueFium>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HistoriqueFIA");

            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ImportSatu>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ImportType>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("ImportType");

            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<L>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.Property(e => e.Agent)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AgentOid).HasMaxLength(8);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EndPeriode).HasColumnType("datetime");
            entity.Property(e => e.IdCallReason).HasColumnName("Id_CallReason");
            entity.Property(e => e.IdCategories).HasColumnName("Id_Categories");
            entity.Property(e => e.Memo).HasMaxLength(200);
            entity.Property(e => e.StartPeriode).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<ListAgent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListAgents");

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

        modelBuilder.Entity<ListCallStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListCallStatus");

            entity.Property(e => e.Comment)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("ntext");
            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("createat");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
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
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.StatusText)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ListCallType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListCallTypes");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ListCampaign>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListCampaigns");

            entity.Property(e => e.AcdFax)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AcdOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .UseCollation("French_BIN");
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
            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("createat");
            entity.Property(e => e.Crmoid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("CRMOid");
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
            entity.Property(e => e.Dbname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DBName");
            entity.Property(e => e.DefLanguage)
                .HasMaxLength(8)
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
            entity.Property(e => e.RecordsPathVideo)
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

        modelBuilder.Entity<LsCallReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_CallReason");

            entity.Property(e => e.DesCallReason).HasColumnName("Des_CallReason");
            entity.Property(e => e.SiteId).HasColumnName("SiteID");
        });

        modelBuilder.Entity<LsCalledCampaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_CalledCampaign");

            entity.Property(e => e.CampagneDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CampagneDid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CampagneDID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLsTemplateNavigation).WithMany(p => p.LsCalledCampaigns)
                .HasForeignKey(d => d.IdLsTemplate)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Ls_CalledCampaign_Ls_template");
        });

        modelBuilder.Entity<LsCategoriesBkp20190130>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ls_categories_bkp_20190130");

            entity.Property(e => e.DesCategories).HasColumnName("Des_Categories");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<LsCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_categories");

            entity.Property(e => e.DesCategories).HasColumnName("Des_Categories");
        });

        modelBuilder.Entity<LsEvalAuditor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Ls_EvalAuditor");

            entity.Property(e => e.DateEval)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Date_Eval");
        });

        modelBuilder.Entity<LsScoreSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_ScoreSection");

            entity.Property(e => e.IdLsSurvey).HasColumnName("Id_Ls_survey");
            entity.Property(e => e.IdLsTemplateItemGroup).HasColumnName("Id_Ls_templateItemGroup");
        });

        modelBuilder.Entity<LsSurvey>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_survey");

            entity.HasIndex(e => new { e.RecordDataId, e.IsSaved }, "Ls_survey_indx").HasFillFactor(1);

            entity.HasIndex(e => e.IsSaved, "Ls_survey_indx1");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdCallReason).HasColumnName("Id_CallReason");
            entity.Property(e => e.IdCategories).HasColumnName("Id_Categories");
            entity.Property(e => e.IsSaved).HasColumnName("Is_saved");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCallReasonNavigation).WithMany(p => p.LsSurveys)
                .HasForeignKey(d => d.IdCallReason)
                .HasConstraintName("FK_Ls_survey_Ls_CallReason");

            entity.HasOne(d => d.IdCategoriesNavigation).WithMany(p => p.LsSurveys)
                .HasForeignKey(d => d.IdCategories)
                .HasConstraintName("FK_Ls_survey_Ls_categories");

            entity.HasOne(d => d.Ls).WithMany(p => p.LsSurveys)
                .HasForeignKey(d => d.LsId)
                .HasConstraintName("FK_Ls_survey_Ls");
        });

        modelBuilder.Entity<LsSurveyBkp20190130>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ls_survey_bkp_20190130");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IdCallReason).HasColumnName("Id_CallReason");
            entity.Property(e => e.IdCategories).HasColumnName("Id_Categories");
            entity.Property(e => e.IsSaved).HasColumnName("Is_saved");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LsSurveyItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_surveyItem");

            entity.HasIndex(e => e.SurveyId, "Ls_surveyItem_surveyID");

            entity.HasIndex(e => e.CreateDate, "lssurveyItemCreateDate");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Survey).WithMany(p => p.LsSurveyItems)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK_Ls_surveyItem_Ls_survey");
        });

        modelBuilder.Entity<LsTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_template");

            entity.Property(e => e.CampagneDid)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("CampagneDID");
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.RelatedTemplate).HasColumnName("related_template");
            entity.Property(e => e.ScriptUrl).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UrlScript).HasMaxLength(500);
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<LsTemplateItem>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_Ls_templateItem1")
                .HasFillFactor(1);

            entity.ToTable("Ls_templateItem");

            entity.Property(e => e.Coef).HasDefaultValue(1);
            entity.Property(e => e.IsKillerQuestion).HasColumnName("is_killer_question");
            entity.Property(e => e.IsKillerSection).HasColumnName("is_killer_section");
            entity.Property(e => e.IsNa).HasColumnName("is_NA");

            entity.HasOne(d => d.Group).WithMany(p => p.LsTemplateItems)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Ls_templateItem_Ls_templateItemGroup");
        });

        modelBuilder.Entity<LsTemplateItemGroup>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_Ls_templateItemGroup1")
                .HasFillFactor(1);

            entity.ToTable("Ls_templateItemGroup");

            entity.Property(e => e.Coef).HasDefaultValue(1);
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.HasOne(d => d.Template).WithMany(p => p.LsTemplateItemGroups)
                .HasForeignKey(d => d.TemplateId)
                .HasConstraintName("FK_Ls_templateItemGroup_Ls_template");
        });

        modelBuilder.Entity<LsTemplatePeriode>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_Ls_templatePeriode1")
                .HasFillFactor(1);

            entity.ToTable("Ls_templatePeriode");

            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<LsTemplateType>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Ls_templateType");

            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<MgrhConfDock>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_Dock")
                .HasFillFactor(1);

            entity.ToTable("mgrh_conf_dock");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateTimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("date_time_created");
            entity.Property(e => e.DateTimeUpdated)
                .HasColumnType("datetime")
                .HasColumnName("date_time_updated");
            entity.Property(e => e.Layout).HasColumnName("layout");
            entity.Property(e => e.UserCreated).HasColumnName("user_created");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserUpdated).HasColumnName("user_updated");
        });

        modelBuilder.Entity<Odcall>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("odcalls");

            entity.HasIndex(e => new { e.CustomerId, e.Indice, e.CallLocalTimeString }, "inx").HasFillFactor(1);

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID");
            entity.Property(e => e.Ani)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.AssociatedData)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallUniversalTime).HasColumnType("datetime");
            entity.Property(e => e.CallUniversalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ContactId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ContactID");
            entity.Property(e => e.CtiId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CtiID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Dnis)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FirstCampaign)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstIvr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FirstIVR");
            entity.Property(e => e.Indice).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.LastCampaign)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastIvr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("LastIVR");
            entity.Property(e => e.LastTransfer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Memo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OutDialed)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OutTel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProactiveReason).HasMaxLength(100);
            entity.Property(e => e.RefId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("RefID");
            entity.Property(e => e.Uui)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("UUI");
        });

        modelBuilder.Entity<Odcallsback>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("odcallsback");

            entity.Property(e => e.Ani)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.AssociatedData)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallUniversalTime).HasColumnType("datetime");
            entity.Property(e => e.CallUniversalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ContactId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ContactID");
            entity.Property(e => e.CtiId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CtiID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Dnis)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FirstCampaign)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstIvr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FirstIVR");
            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ID");
            entity.Property(e => e.Indice).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.LastCampaign)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastIvr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("LastIVR");
            entity.Property(e => e.LastTransfer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Memo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OutDialed)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OutTel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProactiveReason).HasMaxLength(100);
            entity.Property(e => e.RefId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("RefID");
            entity.Property(e => e.Uui)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("UUI");
        });

        modelBuilder.Entity<RecordBkp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("record_bkp");

            entity.Property(e => e.RecAgentId).HasColumnName("Rec_AgentId");
            entity.Property(e => e.RecC)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("rec_c");
            entity.Property(e => e.RecCallId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Rec_CallId");
            entity.Property(e => e.RecCampId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Rec_CampID");
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecComment)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecCustomerId).HasColumnName("Rec_CustomerId");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecDate1)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date__");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecFilenametmp)
                .HasMaxLength(250)
                .HasColumnName("Rec_Filenametmp");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
        });

        modelBuilder.Entity<RecordDataRef>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecordDataRef");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("INDICE");
            entity.Property(e => e.VAgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("vAgentOid");
            entity.Property(e => e.VAni)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vANI");
            entity.Property(e => e.VCallDuration).HasColumnName("vCallDuration");
            entity.Property(e => e.VCallLocalTime)
                .HasColumnType("datetime")
                .HasColumnName("vCallLocalTime");
            entity.Property(e => e.VCallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallLocalTimeString");
            entity.Property(e => e.VCallStatusDetail).HasColumnName("vCallStatusDetail");
            entity.Property(e => e.VCallStatusDetailDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusDetailDescription");
            entity.Property(e => e.VCallStatusGroup).HasColumnName("vCallStatusGroup");
            entity.Property(e => e.VCallStatusGroupDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusGroupDescription");
            entity.Property(e => e.VCallStatusNum).HasColumnName("vCallStatusNum");
            entity.Property(e => e.VCallStatusNumDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusNumDescription");
            entity.Property(e => e.VCallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallTypeDescription");
            entity.Property(e => e.VCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("vCampaignDescription");
            entity.Property(e => e.VConvDuration).HasColumnName("vConvDuration");
            entity.Property(e => e.VCustomerDescription)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCustomerDescription");
            entity.Property(e => e.VCustomerId).HasColumnName("vCustomerID");
            entity.Property(e => e.VDnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vDNIS");
            entity.Property(e => e.VDuration).HasColumnName("vDuration");
            entity.Property(e => e.VMemo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vMemo");
            entity.Property(e => e.VNomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vNomAgent");
            entity.Property(e => e.VNumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vNumeroTel");
            entity.Property(e => e.VPrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vPrenomAgent");
            entity.Property(e => e.VStatusText)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vStatusText");
            entity.Property(e => e.VTotalWaitDuration).HasColumnName("vTotalWaitDuration");
            entity.Property(e => e.VWaitDuration).HasColumnName("vWaitDuration");
        });

        modelBuilder.Entity<RecordDataRef1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecordDataRef1");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("INDICE");
            entity.Property(e => e.VAgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("vAgentOid");
            entity.Property(e => e.VAni)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vANI");
            entity.Property(e => e.VCallDuration).HasColumnName("vCallDuration");
            entity.Property(e => e.VCallLocalTime)
                .HasColumnType("datetime")
                .HasColumnName("vCallLocalTime");
            entity.Property(e => e.VCallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallLocalTimeString");
            entity.Property(e => e.VCallStatusDetail).HasColumnName("vCallStatusDetail");
            entity.Property(e => e.VCallStatusDetailDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusDetailDescription");
            entity.Property(e => e.VCallStatusGroup).HasColumnName("vCallStatusGroup");
            entity.Property(e => e.VCallStatusGroupDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusGroupDescription");
            entity.Property(e => e.VCallStatusNum).HasColumnName("vCallStatusNum");
            entity.Property(e => e.VCallStatusNumDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusNumDescription");
            entity.Property(e => e.VCallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallTypeDescription");
            entity.Property(e => e.VCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("vCampaignDescription");
            entity.Property(e => e.VConvDuration).HasColumnName("vConvDuration");
            entity.Property(e => e.VCustomerDescription)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCustomerDescription");
            entity.Property(e => e.VCustomerId).HasColumnName("vCustomerID");
            entity.Property(e => e.VDnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vDNIS");
            entity.Property(e => e.VDuration).HasColumnName("vDuration");
            entity.Property(e => e.VMemo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vMemo");
            entity.Property(e => e.VNomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vNomAgent");
            entity.Property(e => e.VNumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vNumeroTel");
            entity.Property(e => e.VPrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vPrenomAgent");
            entity.Property(e => e.VStatusText)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vStatusText");
            entity.Property(e => e.VTotalWaitDuration).HasColumnName("vTotalWaitDuration");
            entity.Property(e => e.VWaitDuration).HasColumnName("vWaitDuration");
        });

        modelBuilder.Entity<RecordDataRefAddc>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecordDataRef_ADDC");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("INDICE");
            entity.Property(e => e.VAgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("vAgentOid");
            entity.Property(e => e.VAni)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vANI");
            entity.Property(e => e.VCallDuration).HasColumnName("vCallDuration");
            entity.Property(e => e.VCallLocalTime)
                .HasColumnType("datetime")
                .HasColumnName("vCallLocalTime");
            entity.Property(e => e.VCallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallLocalTimeString");
            entity.Property(e => e.VCallStatusDetail).HasColumnName("vCallStatusDetail");
            entity.Property(e => e.VCallStatusDetailDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusDetailDescription");
            entity.Property(e => e.VCallStatusGroup).HasColumnName("vCallStatusGroup");
            entity.Property(e => e.VCallStatusGroupDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusGroupDescription");
            entity.Property(e => e.VCallStatusNum).HasColumnName("vCallStatusNum");
            entity.Property(e => e.VCallStatusNumDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusNumDescription");
            entity.Property(e => e.VCallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallTypeDescription");
            entity.Property(e => e.VCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("vCampaignDescription");
            entity.Property(e => e.VConvDuration).HasColumnName("vConvDuration");
            entity.Property(e => e.VCustomerDescription)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCustomerDescription");
            entity.Property(e => e.VCustomerId).HasColumnName("vCustomerID");
            entity.Property(e => e.VDnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vDNIS");
            entity.Property(e => e.VDuration).HasColumnName("vDuration");
            entity.Property(e => e.VMemo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vMemo");
            entity.Property(e => e.VNomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vNomAgent");
            entity.Property(e => e.VNumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vNumeroTel");
            entity.Property(e => e.VPrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vPrenomAgent");
            entity.Property(e => e.VStatusText)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vStatusText");
            entity.Property(e => e.VTotalWaitDuration).HasColumnName("vTotalWaitDuration");
            entity.Property(e => e.VWaitDuration).HasColumnName("vWaitDuration");
        });

        modelBuilder.Entity<RecordDataRefAddc201812>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecordDataRef_ADDC_2018_12");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("INDICE");
            entity.Property(e => e.VAgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("vAgentOid");
            entity.Property(e => e.VAni)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vANI");
            entity.Property(e => e.VCallDuration).HasColumnName("vCallDuration");
            entity.Property(e => e.VCallLocalTime)
                .HasColumnType("datetime")
                .HasColumnName("vCallLocalTime");
            entity.Property(e => e.VCallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallLocalTimeString");
            entity.Property(e => e.VCallStatusDetail).HasColumnName("vCallStatusDetail");
            entity.Property(e => e.VCallStatusDetailDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusDetailDescription");
            entity.Property(e => e.VCallStatusGroup).HasColumnName("vCallStatusGroup");
            entity.Property(e => e.VCallStatusGroupDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusGroupDescription");
            entity.Property(e => e.VCallStatusNum).HasColumnName("vCallStatusNum");
            entity.Property(e => e.VCallStatusNumDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusNumDescription");
            entity.Property(e => e.VCallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallTypeDescription");
            entity.Property(e => e.VCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("vCampaignDescription");
            entity.Property(e => e.VConvDuration).HasColumnName("vConvDuration");
            entity.Property(e => e.VCustomerDescription)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCustomerDescription");
            entity.Property(e => e.VCustomerId).HasColumnName("vCustomerID");
            entity.Property(e => e.VDnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vDNIS");
            entity.Property(e => e.VDuration).HasColumnName("vDuration");
            entity.Property(e => e.VMemo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vMemo");
            entity.Property(e => e.VNomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vNomAgent");
            entity.Property(e => e.VNumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vNumeroTel");
            entity.Property(e => e.VPrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vPrenomAgent");
            entity.Property(e => e.VStatusText)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vStatusText");
            entity.Property(e => e.VTotalWaitDuration).HasColumnName("vTotalWaitDuration");
            entity.Property(e => e.VWaitDuration).HasColumnName("vWaitDuration");
        });

        modelBuilder.Entity<RecordDataRefOld>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecordDataRef_OLD");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("INDICE");
            entity.Property(e => e.VAgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("vAgentOid");
            entity.Property(e => e.VAni)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vANI");
            entity.Property(e => e.VCallDuration).HasColumnName("vCallDuration");
            entity.Property(e => e.VCallLocalTime)
                .HasColumnType("datetime")
                .HasColumnName("vCallLocalTime");
            entity.Property(e => e.VCallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallLocalTimeString");
            entity.Property(e => e.VCallStatusDetail).HasColumnName("vCallStatusDetail");
            entity.Property(e => e.VCallStatusDetailDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusDetailDescription");
            entity.Property(e => e.VCallStatusGroup).HasColumnName("vCallStatusGroup");
            entity.Property(e => e.VCallStatusGroupDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusGroupDescription");
            entity.Property(e => e.VCallStatusNum).HasColumnName("vCallStatusNum");
            entity.Property(e => e.VCallStatusNumDescription)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallStatusNumDescription");
            entity.Property(e => e.VCallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCallTypeDescription");
            entity.Property(e => e.VCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("vCampaignDescription");
            entity.Property(e => e.VConvDuration).HasColumnName("vConvDuration");
            entity.Property(e => e.VCustomerDescription)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vCustomerDescription");
            entity.Property(e => e.VCustomerId).HasColumnName("vCustomerID");
            entity.Property(e => e.VDnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vDNIS");
            entity.Property(e => e.VDuration).HasColumnName("vDuration");
            entity.Property(e => e.VMemo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vMemo");
            entity.Property(e => e.VNomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vNomAgent");
            entity.Property(e => e.VNumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vNumeroTel");
            entity.Property(e => e.VPrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false)
                .HasColumnName("vPrenomAgent");
            entity.Property(e => e.VStatusText)
                .HasMaxLength(400)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("vStatusText");
            entity.Property(e => e.VTotalWaitDuration).HasColumnName("vTotalWaitDuration");
            entity.Property(e => e.VWaitDuration).HasColumnName("vWaitDuration");
        });

        modelBuilder.Entity<RecordDatum>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => new { e.NomAgent, e.RecordStatusId, e.RecDate }, "IDX_NOM_AG");

            entity.HasIndex(e => new { e.CustomerId, e.RecDate, e.AgentOid, e.RecordStatusId, e.CampaignId }, "INDEX_CusID_REC8Date_AGOID_RECSTAID_CAMPID_WITH_INC_20190117");

            entity.HasIndex(e => new { e.CustomerId, e.RecDate, e.AgentOid, e.NumeroTel, e.RecordStatusId, e.CampaignId }, "INDEX_CusID_RECDate_AGOID_NUMTEL_RECSTATID_CAMPID_WITH_INC_20190117");

            entity.HasIndex(e => e.RecordStatusId, "RecordDataIndxLocaltime");

            entity.HasIndex(e => e.CampaignDescription, "RecordData_CampaignDescription");

            entity.HasIndex(e => e.RecordStatusId, "RecordData_RecordSource");

            entity.HasIndex(e => new { e.CustomerId, e.RecDate, e.AgentOid, e.RecordStatusId, e.CampaignId }, "RecordData_coumt2");

            entity.HasIndex(e => new { e.CustomerId, e.LastAgent, e.RecDate, e.AgentOid, e.RecordStatusId, e.CampaignId }, "RecordData_count");

            entity.HasIndex(e => e.RecCallId, "RecordData_rec_callid");

            entity.HasIndex(e => new { e.RecDate, e.RecordStatusId }, "RecordData_rec_date_statusid");

            entity.HasIndex(e => e.RecIdLink, "RecorddataIndx_Idlink");

            entity.HasIndex(e => e.Id, "indx_id")
                .IsClustered()
                .HasFillFactor(80);

            entity.HasIndex(e => e.RecDate, "indx_rec_date").HasFillFactor(80);

            entity.HasIndex(e => e.RecFilename, "indx_rec_filename").HasFillFactor(80);

            entity.HasIndex(e => e.RecFilenameTmp, "indx_rec_filename_tmp").HasFillFactor(80);

            entity.HasIndex(e => new { e.RecDate, e.NumeroTel, e.RecordStatusId }, "indx_recdqteTel");

            entity.HasIndex(e => new { e.RecordStatusId, e.CallLocalTime }, "indx_stqtus_time_RecordData");

            entity.HasIndex(e => new { e.RecordStatusId, e.RecIdLink }, "indxtobedelete");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusDetailDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusNumDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CampaignId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateArchive)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateTransfert)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FullRecFilename)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filename");
            entity.Property(e => e.FullRecFilenameBackup)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filenameBackup");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCallId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Rec_CallId");
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("Rec_CampaignDescription");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecDateLocal)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date_Local");
            entity.Property(e => e.RecExec).HasColumnName("rec_exec");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecFilenameTmp)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_FilenameTmp");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordArchive)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordSourceOld)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusRequal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        modelBuilder.Entity<RecordFile>(entity =>
        {
            entity.HasKey(e => e.RecSourceFilename).HasFillFactor(80);

            entity.ToTable("RecordFile");

            entity.Property(e => e.RecSourceFilename)
                .HasMaxLength(300)
                .HasColumnName("rec_source_filename");
            entity.Property(e => e.RecDestinationCreationDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_destination_creation_date");
            entity.Property(e => e.RecDestinationFilename)
                .HasMaxLength(300)
                .HasColumnName("rec_destination_filename");
            entity.Property(e => e.RecDestinationSize).HasColumnName("rec_destination_size");
            entity.Property(e => e.RecExecDuration).HasColumnName("rec_exec_duration");
            entity.Property(e => e.RecSourceCreationDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_source_creation_date");
            entity.Property(e => e.RecSourceSize).HasColumnName("rec_source_size");
            entity.Property(e => e.RecStatusId).HasColumnName("rec_status_id");
        });

        modelBuilder.Entity<RecordStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("RecordStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Recordcheck>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("recordcheck");

            entity.Property(e => e.Agent)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("agent");
            entity.Property(e => e.C1).HasColumnName("c1");
            entity.Property(e => e.C2).HasColumnName("c2");
            entity.Property(e => e.C3).HasColumnName("c3");
            entity.Property(e => e.C4).HasColumnName("c4");
            entity.Property(e => e.C5).HasColumnName("c5");
            entity.Property(e => e.C6).HasColumnName("c6");
            entity.Property(e => e.Campaignid)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("campaignid");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Lastagent).HasColumnName("lastagent");
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("rec_date");
            entity.Property(e => e.RecIdlink).HasColumnName("rec_idlink");
            entity.Property(e => e.Recordsource)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("recordsource");
            entity.Property(e => e.Recordstatusid).HasColumnName("recordstatusid");
            entity.Property(e => e.V).HasColumnName("v");
            entity.Property(e => e.VNumeroTel).HasColumnName("vNumeroTel");
            entity.Property(e => e.Vagent).HasColumnName("vagent");
            entity.Property(e => e.Vcampaignid).HasColumnName("vcampaignid");
            entity.Property(e => e.Vlastagent).HasColumnName("vlastagent");
            entity.Property(e => e.VrecIdlink).HasColumnName("vrec_idlink");
        });

        modelBuilder.Entity<StFileTemplateKey>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("St_fileTemplateKey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Value).HasMaxLength(200);
        });

        modelBuilder.Entity<StFrequency>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("St_frequency");

            entity.Property(e => e.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<StFtp>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("St_FTP");

            entity.Property(e => e.Destination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sever)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StJob>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_St_job1")
                .HasFillFactor(1);

            entity.ToTable("St_job");

            entity.Property(e => e.ActiveFtp).HasColumnName("ActiveFTP");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DestinationFtp)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DestinationPath).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.LastRunDate).HasColumnType("datetime");
            entity.Property(e => e.LoginFtp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Memo).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NextRunDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordFtp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecordFileTemplate).HasMaxLength(255);
            entity.Property(e => e.SchedulingTime).HasColumnType("datetime");
            entity.Property(e => e.ServerFtp)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SourcePath).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<StLevelConversion>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("St_LevelConversion");

            entity.Property(e => e.LevelConversion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("St_log");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EndRunDate).HasColumnType("datetime");
            entity.Property(e => e.Memo).HasMaxLength(255);
            entity.Property(e => e.RunDate).HasColumnType("datetime");
            entity.Property(e => e.StartRunDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<StNotificationEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("St_notificationEvent");

            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<StStatus>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_St_logStatus")
                .HasFillFactor(1);

            entity.ToTable("St_status");

            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<TAgentTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("tAgentTeams");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Team).WithMany(p => p.TAgentTeams)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_tAgentTeams_tAgentTeams");
        });

        modelBuilder.Entity<TCallType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tCallTypes");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TListAgent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tListAgents");

            entity.HasIndex(e => new { e.CustomerId, e.Ident }, "IDX_INDEX");

            entity.Property(e => e.ContextOptions).HasColumnType("ntext");
            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("createat");
            entity.Property(e => e.Ctiskills)
                .HasColumnType("text")
                .HasColumnName("CTISkills");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .HasColumnName("customer");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("customerOid");
            entity.Property(e => e.Deleteat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("deleteat");
            entity.Property(e => e.LoginName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Options)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OutCampaigns).HasColumnType("text");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prenom)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.Profile).IsUnicode(false);
            entity.Property(e => e.Queues).HasColumnType("ntext");
            entity.Property(e => e.ReceptionGrp).IsUnicode(false);
            entity.Property(e => e.Rights).IsUnicode(false);
            entity.Property(e => e.ScriptFramesetName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ScriptFramesetUrl).IsUnicode(false);
            entity.Property(e => e.WorkspaceOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
        });

        modelBuilder.Entity<TListAgentEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tListAgentEmail");

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Oidagent)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("OIDAgent");
        });

        modelBuilder.Entity<TListAgentTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("tListAgentTeam");

            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<TListCallStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tListCallStatus");

            entity.Property(e => e.Comment).HasColumnType("ntext");
            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("createat");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customer");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("customerOid");
            entity.Property(e => e.Deleteat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("deleteat");
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.StatusText)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TListCampaign>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tListCampaigns");

            entity.Property(e => e.AcdFax)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AcdOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .UseCollation("French_BIN");
            entity.Property(e => e.AgentMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AnsweringMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AttachmentGroupOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.AutoRecord)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AvayaVdn).HasColumnName("AvayaVDN");
            entity.Property(e => e.CallAbandonParam).HasColumnType("text");
            entity.Property(e => e.CallAnsweringMachineParam).HasColumnType("text");
            entity.Property(e => e.CallBackDeadLineDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.CallBusyParam).HasColumnType("text");
            entity.Property(e => e.CallClassificationNumbers)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallDisturbedParam).HasColumnType("text");
            entity.Property(e => e.CallMissedParam).HasColumnType("text");
            entity.Property(e => e.CallNoAnswerParam)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CallTransitionNumbers)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg1)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg2)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg3)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg4)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg5)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg6)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClosingMsg7)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Createat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("createat");
            entity.Property(e => e.Crmoid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("CRMOid");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .HasColumnName("customer");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.CustomerOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN")
                .HasColumnName("customerOid");
            entity.Property(e => e.Dbname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DBName");
            entity.Property(e => e.DefLanguage)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Deleteat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("deleteat");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Did)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("DID");
            entity.Property(e => e.HoldMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HolidayId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("HolidayID");
            entity.Property(e => e.MailPwd)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MailTemplateOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.MailUser)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MenuMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NoAgentMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Oid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.OpeningId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("OpeningID");
            entity.Property(e => e.OutExclude)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OutOperator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OutPrefix)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OverflowMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Patience)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneDisplaySpecific).HasColumnType("text");
            entity.Property(e => e.ProfilCampaign)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Profil_Campaign");
            entity.Property(e => e.Profile)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.QuerySchedulerId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.QueueId).HasColumnName("QueueID");
            entity.Property(e => e.QueueSecondId).HasColumnName("QueueSecondID");
            entity.Property(e => e.RouteDest)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RouteFailedMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RouteMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ScriptAddress)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.ScriptName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ScriptParams)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TelByeMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TelCheckMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TelConfirmMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TelErrorMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TelInputMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TransferMode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VoiceByeMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoiceCheckMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoiceConfirmMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoiceInputMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoiceScript)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoiceScriptName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VoicemailConnection)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.VoicemailTable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WaitMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.WaitTimeMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.WelcomeMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TOdcall>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tODCalls");

            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CallUniversalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DNIS");
            entity.Property(e => e.FirstCampaign)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("firstCampaign");
            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("indice");
            entity.Property(e => e.LastCampaign)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Memo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OutTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TagEcoute>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("Tag_ecoute");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Transfert>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("transfert");

            entity.Property(e => e.Destination)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("destination");
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("source");
        });

        modelBuilder.Entity<Trecorddatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("trecorddata");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusDetailDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusNumDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CampaignId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateArchive)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateTransfert)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FullRecFilename)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filename");
            entity.Property(e => e.FullRecFilenameBackup)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filenameBackup");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCallId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Rec_CallId");
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("Rec_CampaignDescription");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecDateLocal)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date_Local");
            entity.Property(e => e.RecExec).HasColumnName("rec_exec");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecFilenameTmp)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_FilenameTmp");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordArchive)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordSourceOld)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusRequal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        modelBuilder.Entity<TtOdcall>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ttODcalls");

            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CallUniversalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DNIS");
            entity.Property(e => e.FirstCampaign)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("firstCampaign");
            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ID");
            entity.Property(e => e.Indice)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("indice");
            entity.Property(e => e.LastCampaign)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Memo)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OutTel)
                .HasMaxLength(60)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<TypeRequalif>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(1);

            entity.ToTable("TypeRequalif");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewRecordSync>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_RecordSync");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        modelBuilder.Entity<ViewRecordSync1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_RecordSync1");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        modelBuilder.Entity<ViewSyncImoprt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_SyncImoprt");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Did)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("DID");
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.Nom)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Prénom)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Site)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VpAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.ToTable("vp_action");

            entity.Property(e => e.Comment).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.RecordedId).HasComment("audio file Id on RecordingTool");
            entity.Property(e => e.ScreenFileName).HasMaxLength(200);
            entity.Property(e => e.ScreenPosition).HasMaxLength(50);
        });

        modelBuilder.Entity<VwlistLsSurveyGv>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWListLsSurveyGV");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusDetailDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusNumDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CampaignId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateArchive)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateTransfert)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FullRecFilename)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filename");
            entity.Property(e => e.FullRecFilenameBackup)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filenameBackup");
            entity.Property(e => e.GvEvalImgName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GvEvalToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvListenedImgName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GvListenedToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvTrashImgName)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.GvTrashToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvWatchedImgName)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.GvWatchedToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ListCallReasonDesCallReason).HasColumnName("ListCallReasonDes_CallReason");
            entity.Property(e => e.LsCcampagneDid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ls_CCampagneDID");
            entity.Property(e => e.LsCid).HasColumnName("Ls_CId");
            entity.Property(e => e.LsCsite).HasColumnName("Ls_CSite");
            entity.Property(e => e.LsLsId).HasColumnName("ls_lsId");
            entity.Property(e => e.LsLsuserName)
                .HasMaxLength(100)
                .HasColumnName("ls_lsuserName");
            entity.Property(e => e.LsSurveyAgentIdLs).HasColumnName("ls_surveyAgentIdLs");
            entity.Property(e => e.LsSurveyCreateBy).HasColumnName("ls_surveyCreateBy");
            entity.Property(e => e.LsSurveyCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("ls_surveyCreateDate");
            entity.Property(e => e.LsSurveyId).HasColumnName("ls_surveyId");
            entity.Property(e => e.LsSurveyIdCallReason).HasColumnName("ls_surveyId_CallReason");
            entity.Property(e => e.LsSurveyIdCategories).HasColumnName("ls_surveyId_Categories");
            entity.Property(e => e.LsSurveyIsSaved).HasColumnName("ls_surveyIs_saved");
            entity.Property(e => e.LsSurveyLsId).HasColumnName("ls_surveyLsId");
            entity.Property(e => e.LsSurveyMemo).HasColumnName("ls_surveyMemo");
            entity.Property(e => e.LsSurveyMemoActionTaken).HasColumnName("ls_surveyMemoActionTaken");
            entity.Property(e => e.LsSurveyScore).HasColumnName("ls_surveyScore");
            entity.Property(e => e.LsSurveyUpdateBy).HasColumnName("ls_surveyUpdateBy");
            entity.Property(e => e.LsSurveyUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("ls_surveyUpdateDate");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCallId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Rec_CallId");
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("Rec_CampaignDescription");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecExec).HasColumnName("rec_exec");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecFilenameTmp)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_FilenameTmp");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordArchive)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordSourceOld)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.ScreenSource)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusRequal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusText).HasMaxLength(400);
            entity.Property(e => e.TListCategoriesDesCategories).HasColumnName("tListCategoriesDes_Categories");
        });

        modelBuilder.Entity<VwlistRecordGv>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWListRecordGV");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusDetailDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusNumDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CampaignId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateArchive)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateTransfert)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FullRecFilename)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filename");
            entity.Property(e => e.FullRecFilenameBackup)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filenameBackup");
            entity.Property(e => e.GvEvalImgName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GvEvalToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvListenedImgName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GvListenedToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvTrashImgName)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.GvTrashToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvWatchedImgName)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.GvWatchedToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCallId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Rec_CallId");
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("Rec_CampaignDescription");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecExec).HasColumnName("rec_exec");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecFilenameTmp)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_FilenameTmp");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordArchive)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordSourceOld)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.ScreenSource)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusRequal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        modelBuilder.Entity<VwlistRecordGvsource>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWListRecordGVSource");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusDetailDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallStatusNumDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CampaignId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateArchive)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DateTransfert)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.FullRecFilename)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filename");
            entity.Property(e => e.FullRecFilenameBackup)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("FullRec_filenameBackup");
            entity.Property(e => e.GvEvalImgName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GvEvalToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvListenedImgName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GvListenedToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.GvTrashImgName)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.GvTrashToolTip)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCallId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Rec_CallId");
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecCampaignDescription)
                .HasMaxLength(50)
                .HasColumnName("Rec_CampaignDescription");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecExec).HasColumnName("rec_exec");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecFilenameTmp)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_FilenameTmp");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordArchive)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordSourceOld)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusRequal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        modelBuilder.Entity<XViewDailyImport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("xView_DailyImport");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ani)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Did)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("DID");
            entity.Property(e => e.Dnis)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.Memo).HasMaxLength(250);
            entity.Property(e => e.Nom)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Prénom)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordSource)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Site).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(400);
        });

        modelBuilder.Entity<XViewRecordDaily>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("xView_RecordDaily");

            entity.Property(e => e.AgentOid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("French_BIN");
            entity.Property(e => e.Ani)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ANI");
            entity.Property(e => e.CallLocalTime).HasColumnType("datetime");
            entity.Property(e => e.CallLocalTimeString)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CallTypeDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CampaignDescription)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDescription).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateImport)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Dnis)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DNIS");
            entity.Property(e => e.Memo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.NomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrenomAgent)
                .HasMaxLength(240)
                .IsUnicode(false);
            entity.Property(e => e.RecCampType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Rec_CampType");
            entity.Property(e => e.RecComment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rec_Comment");
            entity.Property(e => e.RecDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Rec_Date");
            entity.Property(e => e.RecFilename)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Rec_Filename");
            entity.Property(e => e.RecIdLink).HasColumnName("Rec_IdLink");
            entity.Property(e => e.RecIdent)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Rec_Ident");
            entity.Property(e => e.RecNumCamp).HasColumnName("Rec_NumCamp");
            entity.Property(e => e.RecTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Rec_Time");
            entity.Property(e => e.RecType).HasColumnName("Rec_Type");
            entity.Property(e => e.RecordDate)
                .HasColumnType("datetime")
                .HasColumnName("Record_Date");
            entity.Property(e => e.RecordTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Record_Time");
            entity.Property(e => e.StatusText).HasMaxLength(400);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
