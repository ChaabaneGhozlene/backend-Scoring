using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scoring_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDashboardConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ap_action",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeEvent = table.Column<int>(type: "int", nullable: false),
                    RecordFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "path of the audio file"),
                    RecordPosition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "current audio position in seconde"),
                    RecordedId = table.Column<int>(type: "int", nullable: true, comment: "audio file Id on RecordingTool"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ap_action", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "ap_event",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ap_event", x => x.Code)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "CallTypes",
                columns: table => new
                {
                    CallType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomConfig = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Layout = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    UserLogin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Groupe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "ConfigGroupe",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigGroupe", x => x.ID)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "EvalType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalType", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "EvalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IDAuditor = table.Column<int>(type: "int", nullable: true),
                    NumberEval = table.Column<int>(type: "int", nullable: true),
                    Date_Eval = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FiltreGroupe",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiltreGroupe", x => x.ID)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "FiltreType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiltreType", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "HistoriqueDIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RowImported = table.Column<int>(type: "int", nullable: true),
                    TypeImport = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriqueImport_1", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "HistoriqueFIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ImportCount = table.Column<int>(type: "int", nullable: true),
                    ImportMax = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ImportSatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportSatus", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "ImportType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportType", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "Ls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentOid = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Auditor = table.Column<int>(type: "int", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Score = table.Column<float>(type: "real", nullable: true),
                    CalledCampaignId = table.Column<int>(type: "int", nullable: false),
                    StartPeriode = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndPeriode = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    Agent = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    MemoActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_Categories = table.Column<int>(type: "int", nullable: true),
                    Id_CallReason = table.Column<int>(type: "int", nullable: true),
                    userName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "Ls_CallReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Des_CallReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_CallReason", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "Ls_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Des_Categories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_categories", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "ls_categories_bkp_20190130",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Des_Categories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Ls_EvalAuditor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: true),
                    IdAuditor = table.Column<int>(type: "int", nullable: true),
                    NumberEval = table.Column<int>(type: "int", nullable: true),
                    Date_Eval = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Ls_ScoreSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Ls_survey = table.Column<int>(type: "int", nullable: false),
                    Id_Ls_templateItemGroup = table.Column<int>(type: "int", nullable: false),
                    ScoreGroup = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_ScoreSection", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "ls_survey_bkp_20190130",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordDataId = table.Column<int>(type: "int", nullable: true),
                    LsId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Is_saved = table.Column<int>(type: "int", nullable: true),
                    MemoActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_Categories = table.Column<int>(type: "int", nullable: true),
                    Id_CallReason = table.Column<int>(type: "int", nullable: true),
                    AgentIdLs = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Ls_template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Min = table.Column<int>(type: "int", nullable: false),
                    Max = table.Column<int>(type: "int", nullable: false),
                    PeriodeId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ScriptUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    UrlScript = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActiveMinMax = table.Column<int>(type: "int", nullable: true),
                    Site = table.Column<int>(type: "int", nullable: true),
                    CampagneDID = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    CampaignDescription = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    related_template = table.Column<int>(type: "int", nullable: true),
                    version = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_template", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "Ls_templatePeriode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_templatePeriode1", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "Ls_templateType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_templateType", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "mgrh_conf_dock",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    layout = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_created = table.Column<int>(type: "int", nullable: true),
                    date_time_created = table.Column<DateTime>(type: "datetime", nullable: true),
                    user_updated = table.Column<int>(type: "int", nullable: true),
                    date_time_updated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dock", x => x.ID)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "odcalls",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    CtiID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    Indice = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    CallType = table.Column<int>(type: "int", nullable: true),
                    CallUniversalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CallLocalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CallUniversalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    CallLocalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    AcceptDuration = table.Column<int>(type: "int", nullable: true),
                    IvrDuration = table.Column<int>(type: "int", nullable: true),
                    WaitDuration = table.Column<int>(type: "int", nullable: true),
                    ConvDuration = table.Column<int>(type: "int", nullable: true),
                    RerouteDuration = table.Column<int>(type: "int", nullable: true),
                    OverflowDuration = table.Column<int>(type: "int", nullable: true),
                    ANI = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DNIS = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FirstCampaign = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastCampaign = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UUI = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Memo = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    AssociatedData = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    OutTel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OutDialed = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Closed = table.Column<int>(type: "int", nullable: true),
                    NoAgent = table.Column<int>(type: "int", nullable: true),
                    Overflow = table.Column<int>(type: "int", nullable: true),
                    Abandon = table.Column<int>(type: "int", nullable: true),
                    FirstIVR = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    LastIVR = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    FirstQueue = table.Column<int>(type: "int", nullable: true),
                    LastQueue = table.Column<int>(type: "int", nullable: true),
                    InitPriority = table.Column<int>(type: "int", nullable: true),
                    FirstAgent = table.Column<int>(type: "int", nullable: true),
                    LastAgent = table.Column<int>(type: "int", nullable: true),
                    LastTransfer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CallStatusGroup = table.Column<int>(type: "int", nullable: true),
                    CallStatusNum = table.Column<int>(type: "int", nullable: true),
                    CallStatusDetail = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ContactID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    WrapupDuration = table.Column<int>(type: "int", nullable: true),
                    EndByAgent = table.Column<int>(type: "int", nullable: true),
                    AgentListen = table.Column<int>(type: "int", nullable: true),
                    CallDuration = table.Column<int>(type: "int", nullable: true),
                    TotalWaitDuration = table.Column<int>(type: "int", nullable: true),
                    EndReason = table.Column<int>(type: "int", nullable: true),
                    RefID = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true),
                    ProactiveReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odcalls", x => x.ID)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "odcallsback",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    CtiID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    Indice = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    CallType = table.Column<int>(type: "int", nullable: true),
                    CallUniversalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CallLocalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CallUniversalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    CallLocalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    AcceptDuration = table.Column<int>(type: "int", nullable: true),
                    IvrDuration = table.Column<int>(type: "int", nullable: true),
                    WaitDuration = table.Column<int>(type: "int", nullable: true),
                    ConvDuration = table.Column<int>(type: "int", nullable: true),
                    RerouteDuration = table.Column<int>(type: "int", nullable: true),
                    OverflowDuration = table.Column<int>(type: "int", nullable: true),
                    ANI = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DNIS = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FirstCampaign = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastCampaign = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UUI = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Memo = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    AssociatedData = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    OutTel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OutDialed = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Closed = table.Column<int>(type: "int", nullable: true),
                    NoAgent = table.Column<int>(type: "int", nullable: true),
                    Overflow = table.Column<int>(type: "int", nullable: true),
                    Abandon = table.Column<int>(type: "int", nullable: true),
                    FirstIVR = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    LastIVR = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    FirstQueue = table.Column<int>(type: "int", nullable: true),
                    LastQueue = table.Column<int>(type: "int", nullable: true),
                    InitPriority = table.Column<int>(type: "int", nullable: true),
                    FirstAgent = table.Column<int>(type: "int", nullable: true),
                    LastAgent = table.Column<int>(type: "int", nullable: true),
                    LastTransfer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CallStatusGroup = table.Column<int>(type: "int", nullable: true),
                    CallStatusNum = table.Column<int>(type: "int", nullable: true),
                    CallStatusDetail = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ContactID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    WrapupDuration = table.Column<int>(type: "int", nullable: true),
                    EndByAgent = table.Column<int>(type: "int", nullable: true),
                    AgentListen = table.Column<int>(type: "int", nullable: true),
                    CallDuration = table.Column<int>(type: "int", nullable: true),
                    TotalWaitDuration = table.Column<int>(type: "int", nullable: true),
                    EndReason = table.Column<int>(type: "int", nullable: true),
                    RefID = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true),
                    ProactiveReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "record_bkp",
                columns: table => new
                {
                    RecordStatusId = table.Column<int>(type: "int", nullable: false),
                    Rec_Date = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Rec_Time = table.Column<string>(type: "varchar(8000)", unicode: false, maxLength: 8000, nullable: true),
                    Rec_Ident = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    Rec_CampType = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    Rec_NumCamp = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    Rec_IdLink = table.Column<int>(type: "int", nullable: false),
                    Rec_Filename = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Rec_Comment = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Rec_CallId = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: false),
                    Rec_Filenametmp = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Rec_CustomerId = table.Column<int>(type: "int", nullable: false),
                    Rec_CampID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    rec_c = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Rec_AgentId = table.Column<int>(type: "int", nullable: false),
                    Rec_Date__ = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "recordcheck",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    recordstatusid = table.Column<int>(type: "int", nullable: true),
                    recordsource = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    vlastagent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastagent = table.Column<int>(type: "int", nullable: true),
                    c1 = table.Column<int>(type: "int", nullable: false),
                    v = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rec_date = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    c2 = table.Column<int>(type: "int", nullable: false),
                    vrec_idlink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rec_idlink = table.Column<int>(type: "int", nullable: true),
                    c3 = table.Column<int>(type: "int", nullable: false),
                    vNumeroTel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroTel = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    c4 = table.Column<int>(type: "int", nullable: false),
                    vagent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    agent = table.Column<string>(type: "varchar(8000)", unicode: false, maxLength: 8000, nullable: true),
                    c5 = table.Column<int>(type: "int", nullable: false),
                    vcampaignid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    campaignid = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    c6 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RecordData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rec_Date = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    Rec_Time = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    Rec_Ident = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    Rec_CampType = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    Rec_NumCamp = table.Column<int>(type: "int", nullable: true),
                    CampaignDescription = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    Rec_IdLink = table.Column<int>(type: "int", nullable: true),
                    Rec_Type = table.Column<int>(type: "int", nullable: true),
                    CallTypeDescription = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Rec_Comment = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Rec_Filename = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    CustomerDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastAgent = table.Column<int>(type: "int", nullable: true),
                    NomAgent = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: true),
                    PrenomAgent = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: true),
                    AgentOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    NumeroTel = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    CallLocalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    CallDuration = table.Column<int>(type: "int", nullable: true),
                    ConvDuration = table.Column<int>(type: "int", nullable: true),
                    WaitDuration = table.Column<int>(type: "int", nullable: true),
                    TotalWaitDuration = table.Column<int>(type: "int", nullable: true),
                    CallStatusGroup = table.Column<int>(type: "int", nullable: true),
                    CallStatusNum = table.Column<int>(type: "int", nullable: true),
                    CallStatusDetail = table.Column<int>(type: "int", nullable: true),
                    StatusText = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    StatusGroupeRequal = table.Column<int>(type: "int", nullable: true),
                    StatusNumRequal = table.Column<int>(type: "int", nullable: true),
                    StatusDetailRequal = table.Column<int>(type: "int", nullable: true),
                    StatusRequal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TypeRequalif = table.Column<int>(type: "int", nullable: true),
                    DateImport = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    TypeImport = table.Column<int>(type: "int", nullable: true),
                    ANI = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    DNIS = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RecordSource = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    RecordArchive = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    RecordStatusId = table.Column<int>(type: "int", nullable: true),
                    DateTransfert = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    DateArchive = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    Record_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Record_Time = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    CallLocalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    RecordSourceOld = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Rec_CallId = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    Rec_CampaignDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullRec_filename = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    FullRec_filenameBackup = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    rec_exec = table.Column<int>(type: "int", nullable: true),
                    StatusDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    StatusGroupDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallStatusGroupDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallStatusNumDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallStatusDetailDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CampaignId = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    LsId = table.Column<int>(type: "int", nullable: true),
                    Rec_FilenameTmp = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Rec_Date_Local = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RecordFile",
                columns: table => new
                {
                    rec_source_filename = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    rec_source_creation_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    rec_source_size = table.Column<long>(type: "bigint", nullable: true),
                    rec_destination_filename = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    rec_destination_creation_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    rec_destination_size = table.Column<int>(type: "int", nullable: true),
                    rec_status_id = table.Column<int>(type: "int", nullable: true),
                    rec_exec_duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordFile", x => x.rec_source_filename)
                        .Annotation("SqlServer:FillFactor", 80);
                });

            migrationBuilder.CreateTable(
                name: "RecordStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordStatus", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "ReportTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Group = table.Column<int>(type: "int", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "St_fileTemplateKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Evaluate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_fileTemplateKey", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_frequency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_frequency", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_FTP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<int>(type: "int", nullable: false),
                    Sever = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Destination = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Login = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_FTP", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FiltreId = table.Column<int>(type: "int", nullable: false),
                    FrequencyId = table.Column<int>(type: "int", nullable: false),
                    SchedulingTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SourcePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DestinationPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RecordFileTemplate = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    Notification = table.Column<int>(type: "int", nullable: true),
                    NotificationEvent = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LastRunDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastRunStatus = table.Column<int>(type: "int", nullable: true),
                    NextRunDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaxRecord = table.Column<int>(type: "int", nullable: true),
                    ActiveRandom = table.Column<int>(type: "int", nullable: false),
                    ActiveConversion = table.Column<int>(type: "int", nullable: false),
                    ActiveFTP = table.Column<int>(type: "int", nullable: false),
                    LevelConversionId = table.Column<int>(type: "int", nullable: true),
                    ServerFtp = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    DestinationFtp = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    LoginFtp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PasswordFtp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PortFtp = table.Column<int>(type: "int", nullable: true),
                    ActiveJournee = table.Column<int>(type: "int", nullable: true),
                    Journee = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_job1", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_LevelConversion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelConversion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_LevelConversion", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    RunDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartRunDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndRunDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_log", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_notificationEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_notificationEvent", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "St_status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_St_logStatus", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "Tag_ecoute",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRecord = table.Column<int>(type: "int", nullable: true),
                    IdUser = table.Column<int>(type: "int", nullable: true),
                    TagListened = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag_ecoute", x => x.ID)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "tCallTypes",
                columns: table => new
                {
                    CallType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tListAgentEmail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAgent = table.Column<int>(type: "int", nullable: false),
                    OIDAgent = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    Email = table.Column<string>(type: "nchar(150)", fixedLength: true, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tListAgents",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false),
                    customer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Oid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    Ident = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: false),
                    Prenom = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: true),
                    ReceptionGrp = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Queues = table.Column<string>(type: "ntext", nullable: true),
                    EntreCom = table.Column<int>(type: "int", nullable: true),
                    Options = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LoginName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Profile = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Rights = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    GroupMember = table.Column<int>(type: "int", nullable: true),
                    ScriptFramesetUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ScriptFramesetName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MaxChatSessions = table.Column<int>(type: "int", nullable: false),
                    MaxEmailSessions = table.Column<int>(type: "int", nullable: false),
                    AlwaysOnTop = table.Column<bool>(type: "bit", nullable: false),
                    FullScreen = table.Column<bool>(type: "bit", nullable: false),
                    WorkspaceWidth = table.Column<int>(type: "int", nullable: false),
                    WorkspaceHeight = table.Column<int>(type: "int", nullable: false),
                    ContextOptions = table.Column<string>(type: "ntext", nullable: true),
                    ReadyAtLogin = table.Column<bool>(type: "bit", nullable: false),
                    WorkspaceOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    CTISkills = table.Column<string>(type: "text", nullable: true),
                    OutCampaigns = table.Column<string>(type: "text", nullable: true),
                    CoachRightLevel = table.Column<int>(type: "int", nullable: false),
                    CurrentOutQueue = table.Column<int>(type: "int", nullable: false),
                    customerOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    createat = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    deleteat = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tListAgentTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdSite = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tListAgentTeam", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "tListCallStatus",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false),
                    customer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Oid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    StatusGroup = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    StatusDetail = table.Column<int>(type: "int", nullable: false),
                    StatusText = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "ntext", nullable: true),
                    Positive = table.Column<bool>(type: "bit", nullable: false),
                    Argued = table.Column<bool>(type: "bit", nullable: false),
                    Defaut = table.Column<bool>(type: "bit", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    Currency = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ValidQuota = table.Column<bool>(type: "bit", nullable: true),
                    customerOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    createat = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    deleteat = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tListCampaigns",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false),
                    customer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Oid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    DID = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    DBName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TableName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MaxChannels = table.Column<int>(type: "int", nullable: true),
                    MaxChannelsPerAgent = table.Column<double>(type: "float", nullable: true),
                    VoiceScript = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    VoiceScriptName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    QueueID = table.Column<int>(type: "int", nullable: true),
                    AvayaVDN = table.Column<int>(type: "int", nullable: false),
                    QueueSecondID = table.Column<int>(type: "int", nullable: true),
                    QueueSecondTps = table.Column<int>(type: "int", nullable: true),
                    AvgDuration = table.Column<int>(type: "int", nullable: true),
                    WelcomeMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    WaitTimeMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MenuMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AgentMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    WaitMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    WaitMsgDuration = table.Column<int>(type: "int", nullable: true),
                    OverflowMode = table.Column<int>(type: "int", nullable: true),
                    OverflowWaitTime = table.Column<int>(type: "int", nullable: true),
                    OverflowWaitLoop = table.Column<int>(type: "int", nullable: true),
                    OverflowChannels = table.Column<int>(type: "int", nullable: true),
                    NoAgentMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg1 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg2 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg3 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg4 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg5 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg6 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ClosingMsg7 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    RouteDest = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    RouteMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    RouteFailedMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    OverflowMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TelInputMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TelCheckMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TelConfirmMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TelByeMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TelErrorMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    VoiceInputMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    VoiceCheckMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    VoiceConfirmMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    VoiceByeMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    OpeningID = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    HolidayID = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DefLanguage = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    OutOperator = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OutPrefix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OutRing = table.Column<int>(type: "int", nullable: true),
                    OutAbandon = table.Column<int>(type: "int", nullable: true),
                    OutMode = table.Column<int>(type: "int", nullable: true),
                    OutWait = table.Column<int>(type: "int", nullable: true),
                    OutRetries = table.Column<int>(type: "int", nullable: true),
                    ScriptMode = table.Column<int>(type: "int", nullable: true),
                    ScriptAddress = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    ScriptParams = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Profile = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    StatusGroup = table.Column<int>(type: "int", nullable: true),
                    ScriptName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ScriptId = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<int>(type: "int", nullable: true),
                    MailUser = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    MailPwd = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Modif = table.Column<int>(type: "int", nullable: true),
                    QuerySchedulerId = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true, collation: "French_BIN"),
                    PreviewCallAfter = table.Column<int>(type: "int", nullable: true),
                    CallClassification = table.Column<bool>(type: "bit", nullable: true),
                    CallClassificationNumbers = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallTransition = table.Column<bool>(type: "bit", nullable: true),
                    CallTransitionNumbers = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallNoAnswer = table.Column<int>(type: "int", nullable: true),
                    CallNoAnswerParam = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CallBusy = table.Column<int>(type: "int", nullable: true),
                    CallBusyParam = table.Column<string>(type: "text", nullable: true),
                    CallDisturbed = table.Column<int>(type: "int", nullable: true),
                    CallDisturbedParam = table.Column<string>(type: "text", nullable: true),
                    CallAnsweringMachine = table.Column<int>(type: "int", nullable: true),
                    CallAnsweringMachineParam = table.Column<string>(type: "text", nullable: true),
                    CallAbandon = table.Column<int>(type: "int", nullable: true),
                    CallAbandonParam = table.Column<string>(type: "text", nullable: true),
                    CallMissed = table.Column<int>(type: "int", nullable: true),
                    CallMissedParam = table.Column<string>(type: "text", nullable: true),
                    PhoneDisplay = table.Column<int>(type: "int", nullable: true),
                    PhoneDisplaySpecific = table.Column<string>(type: "text", nullable: true),
                    AcdFax = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Patience = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    HoldMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AutoRecord = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AnsweringMsg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CRMOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true, collation: "French_BIN"),
                    PlanningId = table.Column<int>(type: "int", nullable: true),
                    TransferMode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Profil_Campaign = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Virtual = table.Column<int>(type: "int", nullable: true),
                    CallBackDeadLineDays = table.Column<int>(type: "int", nullable: true),
                    CallBackDeadLineDate = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    VoiceMailQueue = table.Column<int>(type: "int", nullable: true),
                    AttachmentGroupOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true, collation: "French_BIN"),
                    MailTemplateOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true, collation: "French_BIN"),
                    VoicemailConnection = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true, collation: "French_BIN"),
                    VoicemailTable = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    VirtualCampDuration = table.Column<int>(type: "int", nullable: true),
                    OutExclude = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    AcdOid = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true, collation: "French_BIN"),
                    customerOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false, collation: "French_BIN"),
                    createat = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    deleteat = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tODCalls",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    indice = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    CallType = table.Column<int>(type: "int", nullable: true),
                    CallLocalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    CallDuration = table.Column<int>(type: "int", nullable: true),
                    ConvDuration = table.Column<int>(type: "int", nullable: true),
                    WaitDuration = table.Column<int>(type: "int", nullable: true),
                    TotalWaitDuration = table.Column<int>(type: "int", nullable: true),
                    ANI = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    DNIS = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Memo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    CallLocalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CallStatusGroup = table.Column<int>(type: "int", nullable: true),
                    CallStatusNum = table.Column<int>(type: "int", nullable: true),
                    CallStatusDetail = table.Column<int>(type: "int", nullable: true),
                    LastCampaign = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    firstCampaign = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LastAgent = table.Column<int>(type: "int", nullable: true),
                    OutTel = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    FirstAgent = table.Column<int>(type: "int", nullable: true),
                    CallUniversalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "transfert",
                columns: table => new
                {
                    source = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    destination = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "trecorddata",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rec_Date = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    Rec_Time = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    Rec_Ident = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    Rec_CampType = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    Rec_NumCamp = table.Column<int>(type: "int", nullable: true),
                    CampaignDescription = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    Rec_IdLink = table.Column<int>(type: "int", nullable: true),
                    Rec_Type = table.Column<int>(type: "int", nullable: true),
                    CallTypeDescription = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Rec_Comment = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Rec_Filename = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    CustomerDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastAgent = table.Column<int>(type: "int", nullable: true),
                    NomAgent = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: true),
                    PrenomAgent = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: true),
                    AgentOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    NumeroTel = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    CallLocalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    CallDuration = table.Column<int>(type: "int", nullable: true),
                    ConvDuration = table.Column<int>(type: "int", nullable: true),
                    WaitDuration = table.Column<int>(type: "int", nullable: true),
                    TotalWaitDuration = table.Column<int>(type: "int", nullable: true),
                    CallStatusGroup = table.Column<int>(type: "int", nullable: true),
                    CallStatusNum = table.Column<int>(type: "int", nullable: true),
                    CallStatusDetail = table.Column<int>(type: "int", nullable: true),
                    StatusText = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    StatusGroupeRequal = table.Column<int>(type: "int", nullable: true),
                    StatusNumRequal = table.Column<int>(type: "int", nullable: true),
                    StatusDetailRequal = table.Column<int>(type: "int", nullable: true),
                    StatusRequal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TypeRequalif = table.Column<int>(type: "int", nullable: true),
                    DateImport = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    TypeImport = table.Column<int>(type: "int", nullable: true),
                    ANI = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    DNIS = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RecordSource = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    RecordArchive = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    RecordStatusId = table.Column<int>(type: "int", nullable: true),
                    DateTransfert = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    DateArchive = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    Record_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Record_Time = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    CallLocalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    RecordSourceOld = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Rec_CallId = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    Rec_CampaignDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullRec_filename = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    FullRec_filenameBackup = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    rec_exec = table.Column<int>(type: "int", nullable: true),
                    StatusDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    StatusGroupDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallStatusGroupDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallStatusNumDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CallStatusDetailDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CampaignId = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    LsId = table.Column<int>(type: "int", nullable: true),
                    Rec_FilenameTmp = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Rec_Date_Local = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ttODcalls",
                columns: table => new
                {
                    ID = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    indice = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    CallType = table.Column<int>(type: "int", nullable: true),
                    CallLocalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    CallDuration = table.Column<int>(type: "int", nullable: true),
                    ConvDuration = table.Column<int>(type: "int", nullable: true),
                    WaitDuration = table.Column<int>(type: "int", nullable: true),
                    TotalWaitDuration = table.Column<int>(type: "int", nullable: true),
                    ANI = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    DNIS = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Memo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    CallLocalTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CallStatusGroup = table.Column<int>(type: "int", nullable: true),
                    CallStatusNum = table.Column<int>(type: "int", nullable: true),
                    CallStatusDetail = table.Column<int>(type: "int", nullable: true),
                    LastCampaign = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    firstCampaign = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LastAgent = table.Column<int>(type: "int", nullable: true),
                    OutTel = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    FirstAgent = table.Column<int>(type: "int", nullable: true),
                    CallUniversalTimeString = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TypeRequalif",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeRequalif", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                });

            migrationBuilder.CreateTable(
                name: "vp_action",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeEvent = table.Column<int>(type: "int", nullable: false),
                    ScreenFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ScreenPosition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecordedId = table.Column<int>(type: "int", nullable: true, comment: "audio file Id on RecordingTool"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vp_action", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 80);
                });

            migrationBuilder.CreateTable(
                name: "Filtre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomFiltre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Expression = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserLogin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false),
                    Groupe = table.Column<int>(type: "int", nullable: false),
                    SqlWhere = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filtre", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_Type",
                        column: x => x.Groupe,
                        principalTable: "FiltreGroupe",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ls_survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordDataId = table.Column<int>(type: "int", nullable: true),
                    LsId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Is_saved = table.Column<int>(type: "int", nullable: true),
                    MemoActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_Categories = table.Column<int>(type: "int", nullable: true),
                    Id_CallReason = table.Column<int>(type: "int", nullable: true),
                    AgentIdLs = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_survey", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_Ls_survey_Ls",
                        column: x => x.LsId,
                        principalTable: "Ls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ls_survey_Ls_CallReason",
                        column: x => x.Id_CallReason,
                        principalTable: "Ls_CallReason",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ls_survey_Ls_categories",
                        column: x => x.Id_Categories,
                        principalTable: "Ls_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ls_CalledCampaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Site = table.Column<int>(type: "int", nullable: true),
                    CampagneDID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CampagneDescription = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IdLsTemplate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_CalledCampaign", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_Ls_CalledCampaign_Ls_template",
                        column: x => x.IdLsTemplate,
                        principalTable: "Ls_template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ls_templateItemGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Coef = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_templateItemGroup1", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_Ls_templateItemGroup_Ls_template",
                        column: x => x.TemplateId,
                        principalTable: "Ls_template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportTemplateId = table.Column<int>(type: "int", nullable: false),
                    ChartType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XAxisField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YAxisField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChartConfigurations_ReportTemplates_ReportTemplateId",
                        column: x => x.ReportTemplateId,
                        principalTable: "ReportTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColumnDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportTemplateId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaIndex = table.Column<int>(type: "int", nullable: false),
                    AggregateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormatString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColumnDefinitions_ReportTemplates_ReportTemplateId",
                        column: x => x.ReportTemplateId,
                        principalTable: "ReportTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilterDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportTemplateId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilterGroup = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterDefinitions_ReportTemplates_ReportTemplateId",
                        column: x => x.ReportTemplateId,
                        principalTable: "ReportTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tAgentTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    AgentOid = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tAgentTeams", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_tAgentTeams_tAgentTeams",
                        column: x => x.TeamId,
                        principalTable: "tListAgentTeam",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ls_surveyItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_surveyItem", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_Ls_surveyItem_Ls_survey",
                        column: x => x.SurveyId,
                        principalTable: "Ls_survey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ls_templateItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Min = table.Column<int>(type: "int", nullable: false),
                    Max = table.Column<int>(type: "int", nullable: false),
                    Coef = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Order = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    is_NA = table.Column<int>(type: "int", nullable: true),
                    is_killer_question = table.Column<int>(type: "int", nullable: true),
                    is_killer_section = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ls_templateItem1", x => x.Id)
                        .Annotation("SqlServer:FillFactor", 1);
                    table.ForeignKey(
                        name: "FK_Ls_templateItem_Ls_templateItemGroup",
                        column: x => x.GroupId,
                        principalTable: "Ls_templateItemGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ap_action_indx",
                table: "ap_action",
                column: "RecordedId")
                .Annotation("SqlServer:FillFactor", 1);

            migrationBuilder.CreateIndex(
                name: "ap_action_indx1",
                table: "ap_action",
                column: "CodeEvent");

            migrationBuilder.CreateIndex(
                name: "IX_ChartConfigurations_ReportTemplateId",
                table: "ChartConfigurations",
                column: "ReportTemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ColumnDefinitions_ReportTemplateId",
                table: "ColumnDefinitions",
                column: "ReportTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterDefinitions_ReportTemplateId",
                table: "FilterDefinitions",
                column: "ReportTemplateId");

            migrationBuilder.CreateIndex(
                name: "iType_Filtre",
                table: "Filtre",
                column: "Type")
                .Annotation("SqlServer:FillFactor", 1);

            migrationBuilder.CreateIndex(
                name: "IX_Filtre_Groupe",
                table: "Filtre",
                column: "Groupe");

            migrationBuilder.CreateIndex(
                name: "IX_Ls_CalledCampaign_IdLsTemplate",
                table: "Ls_CalledCampaign",
                column: "IdLsTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_Ls_survey_Id_CallReason",
                table: "Ls_survey",
                column: "Id_CallReason");

            migrationBuilder.CreateIndex(
                name: "IX_Ls_survey_Id_Categories",
                table: "Ls_survey",
                column: "Id_Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Ls_survey_LsId",
                table: "Ls_survey",
                column: "LsId");

            migrationBuilder.CreateIndex(
                name: "Ls_survey_indx",
                table: "Ls_survey",
                columns: new[] { "RecordDataId", "Is_saved" })
                .Annotation("SqlServer:FillFactor", 1);

            migrationBuilder.CreateIndex(
                name: "Ls_survey_indx1",
                table: "Ls_survey",
                column: "Is_saved");

            migrationBuilder.CreateIndex(
                name: "Ls_surveyItem_surveyID",
                table: "Ls_surveyItem",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "lssurveyItemCreateDate",
                table: "Ls_surveyItem",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Ls_templateItem_GroupId",
                table: "Ls_templateItem",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Ls_templateItemGroup_TemplateId",
                table: "Ls_templateItemGroup",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "inx",
                table: "odcalls",
                columns: new[] { "CustomerID", "Indice", "CallLocalTimeString" })
                .Annotation("SqlServer:FillFactor", 1);

            migrationBuilder.CreateIndex(
                name: "IDX_NOM_AG",
                table: "RecordData",
                columns: new[] { "NomAgent", "RecordStatusId", "Rec_Date" });

            migrationBuilder.CreateIndex(
                name: "INDEX_CusID_REC8Date_AGOID_RECSTAID_CAMPID_WITH_INC_20190117",
                table: "RecordData",
                columns: new[] { "CustomerID", "Rec_Date", "AgentOid", "RecordStatusId", "CampaignId" });

            migrationBuilder.CreateIndex(
                name: "INDEX_CusID_RECDate_AGOID_NUMTEL_RECSTATID_CAMPID_WITH_INC_20190117",
                table: "RecordData",
                columns: new[] { "CustomerID", "Rec_Date", "AgentOid", "NumeroTel", "RecordStatusId", "CampaignId" });

            migrationBuilder.CreateIndex(
                name: "indx_id",
                table: "RecordData",
                column: "ID")
                .Annotation("SqlServer:Clustered", true)
                .Annotation("SqlServer:FillFactor", 80);

            migrationBuilder.CreateIndex(
                name: "indx_rec_date",
                table: "RecordData",
                column: "Rec_Date")
                .Annotation("SqlServer:FillFactor", 80);

            migrationBuilder.CreateIndex(
                name: "indx_rec_filename",
                table: "RecordData",
                column: "Rec_Filename")
                .Annotation("SqlServer:FillFactor", 80);

            migrationBuilder.CreateIndex(
                name: "indx_rec_filename_tmp",
                table: "RecordData",
                column: "Rec_FilenameTmp")
                .Annotation("SqlServer:FillFactor", 80);

            migrationBuilder.CreateIndex(
                name: "indx_recdqteTel",
                table: "RecordData",
                columns: new[] { "Rec_Date", "NumeroTel", "RecordStatusId" });

            migrationBuilder.CreateIndex(
                name: "indx_stqtus_time_RecordData",
                table: "RecordData",
                columns: new[] { "RecordStatusId", "CallLocalTime" });

            migrationBuilder.CreateIndex(
                name: "indxtobedelete",
                table: "RecordData",
                columns: new[] { "RecordStatusId", "Rec_IdLink" });

            migrationBuilder.CreateIndex(
                name: "RecordData_CampaignDescription",
                table: "RecordData",
                column: "CampaignDescription");

            migrationBuilder.CreateIndex(
                name: "RecordData_coumt2",
                table: "RecordData",
                columns: new[] { "CustomerID", "Rec_Date", "AgentOid", "RecordStatusId", "CampaignId" });

            migrationBuilder.CreateIndex(
                name: "RecordData_count",
                table: "RecordData",
                columns: new[] { "CustomerID", "LastAgent", "Rec_Date", "AgentOid", "RecordStatusId", "CampaignId" });

            migrationBuilder.CreateIndex(
                name: "RecordData_rec_callid",
                table: "RecordData",
                column: "Rec_CallId");

            migrationBuilder.CreateIndex(
                name: "RecordData_rec_date_statusid",
                table: "RecordData",
                columns: new[] { "Rec_Date", "RecordStatusId" });

            migrationBuilder.CreateIndex(
                name: "RecordData_RecordSource",
                table: "RecordData",
                column: "RecordStatusId");

            migrationBuilder.CreateIndex(
                name: "RecorddataIndx_Idlink",
                table: "RecordData",
                column: "Rec_IdLink");

            migrationBuilder.CreateIndex(
                name: "RecordDataIndxLocaltime",
                table: "RecordData",
                column: "RecordStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tAgentTeams_TeamId",
                table: "tAgentTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IDX_INDEX",
                table: "tListAgents",
                columns: new[] { "customerId", "Ident" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ap_action");

            migrationBuilder.DropTable(
                name: "ap_event");

            migrationBuilder.DropTable(
                name: "CallTypes");

            migrationBuilder.DropTable(
                name: "ChartConfigurations");

            migrationBuilder.DropTable(
                name: "ColumnDefinitions");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "ConfigGroupe");

            migrationBuilder.DropTable(
                name: "EvalType");

            migrationBuilder.DropTable(
                name: "EvalUsers");

            migrationBuilder.DropTable(
                name: "FilterDefinitions");

            migrationBuilder.DropTable(
                name: "Filtre");

            migrationBuilder.DropTable(
                name: "FiltreType");

            migrationBuilder.DropTable(
                name: "HistoriqueDIA");

            migrationBuilder.DropTable(
                name: "HistoriqueFIA");

            migrationBuilder.DropTable(
                name: "ImportSatus");

            migrationBuilder.DropTable(
                name: "ImportType");

            migrationBuilder.DropTable(
                name: "Ls_CalledCampaign");

            migrationBuilder.DropTable(
                name: "ls_categories_bkp_20190130");

            migrationBuilder.DropTable(
                name: "Ls_EvalAuditor");

            migrationBuilder.DropTable(
                name: "Ls_ScoreSection");

            migrationBuilder.DropTable(
                name: "ls_survey_bkp_20190130");

            migrationBuilder.DropTable(
                name: "Ls_surveyItem");

            migrationBuilder.DropTable(
                name: "Ls_templateItem");

            migrationBuilder.DropTable(
                name: "Ls_templatePeriode");

            migrationBuilder.DropTable(
                name: "Ls_templateType");

            migrationBuilder.DropTable(
                name: "mgrh_conf_dock");

            migrationBuilder.DropTable(
                name: "odcalls");

            migrationBuilder.DropTable(
                name: "odcallsback");

            migrationBuilder.DropTable(
                name: "record_bkp");

            migrationBuilder.DropTable(
                name: "recordcheck");

            migrationBuilder.DropTable(
                name: "RecordData");

            migrationBuilder.DropTable(
                name: "RecordFile");

            migrationBuilder.DropTable(
                name: "RecordStatus");

            migrationBuilder.DropTable(
                name: "St_fileTemplateKey");

            migrationBuilder.DropTable(
                name: "St_frequency");

            migrationBuilder.DropTable(
                name: "St_FTP");

            migrationBuilder.DropTable(
                name: "St_job");

            migrationBuilder.DropTable(
                name: "St_LevelConversion");

            migrationBuilder.DropTable(
                name: "St_log");

            migrationBuilder.DropTable(
                name: "St_notificationEvent");

            migrationBuilder.DropTable(
                name: "St_status");

            migrationBuilder.DropTable(
                name: "Tag_ecoute");

            migrationBuilder.DropTable(
                name: "tAgentTeams");

            migrationBuilder.DropTable(
                name: "tCallTypes");

            migrationBuilder.DropTable(
                name: "tListAgentEmail");

            migrationBuilder.DropTable(
                name: "tListAgents");

            migrationBuilder.DropTable(
                name: "tListCallStatus");

            migrationBuilder.DropTable(
                name: "tListCampaigns");

            migrationBuilder.DropTable(
                name: "tODCalls");

            migrationBuilder.DropTable(
                name: "transfert");

            migrationBuilder.DropTable(
                name: "trecorddata");

            migrationBuilder.DropTable(
                name: "ttODcalls");

            migrationBuilder.DropTable(
                name: "TypeRequalif");

            migrationBuilder.DropTable(
                name: "vp_action");

            migrationBuilder.DropTable(
                name: "ReportTemplates");

            migrationBuilder.DropTable(
                name: "FiltreGroupe");

            migrationBuilder.DropTable(
                name: "Ls_survey");

            migrationBuilder.DropTable(
                name: "Ls_templateItemGroup");

            migrationBuilder.DropTable(
                name: "tListAgentTeam");

            migrationBuilder.DropTable(
                name: "Ls");

            migrationBuilder.DropTable(
                name: "Ls_CallReason");

            migrationBuilder.DropTable(
                name: "Ls_categories");

            migrationBuilder.DropTable(
                name: "Ls_template");
        }
    }
}
