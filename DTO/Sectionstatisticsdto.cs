namespace scoring_Backend.DTO.Statistique
{
    /// <summary>
    /// Filtres envoyés par le frontend pour interroger les données.
    /// </summary>
    public class StatistiqueFilterDto
    {
        public DateTime DateDebut { get; set; } = DateTime.Today;
        public DateTime DateFin { get; set; } = DateTime.Today;

        /// <summary>Id de la campagne qualité (optionnel — null = toutes)</summary>
        public int? CampaignId { get; set; }

        /// <summary>Id de l'agent (optionnel — null = tous)</summary>
        public int? AgentId { get; set; }

        /// <summary>Id de l'auditeur/superviseur (optionnel)</summary>
        public int? AuditorId { get; set; }

        /// <summary>Rôle de l'utilisateur connecté (1=admin, 2=manager, 3=superviseur)</summary>
        public int UserRole { get; set; } = 1;

        public int UserId { get; set; }
        public int SiteId { get; set; }

        /// <summary>Si true, inclut tous les superviseurs du site</summary>
        public bool AllSupervisors { get; set; } = true;
    }

    /// <summary>
    /// Une ligne de données plate renvoyée au frontend.
    /// Le frontend fait le pivot dynamiquement.
    /// </summary>
    public class StatistiqueRowDto
    {
        // ── Identifiants ────────────────────────────────────────────────
        public int SurveyId { get; set; }
        public int? RecordId { get; set; }
        public string? RecordLink { get; set; }

        // ── Dimensions temporelles ───────────────────────────────────────
        public DateTime CreateDate { get; set; }
        public int Year => CreateDate.Year;
        public int Month => CreateDate.Month;
        public string MonthYear => CreateDate.ToString("MM/yyyy");
        public string WeekYear => $"S{System.Globalization.ISOWeek.GetWeekOfYear(CreateDate):D2}/{CreateDate.Year}";

        // ── Dimensions agent / auditeur ──────────────────────────────────
        public int AgentId { get; set; }
        public string Agent { get; set; } = "";
        public int AuditorId { get; set; }
        public string Auditor { get; set; } = "";

        // ── Campagne ─────────────────────────────────────────────────────
        public int? CampaignId { get; set; }
        public string? Campaign { get; set; }
        public string? FullPeriode { get; set; }

        // ── Score global ─────────────────────────────────────────────────
        public double Score { get; set; }
        public string? Memo { get; set; }

        // ── Détail item (null si pas de jointure surveyItem) ─────────────
        public int? ItemId { get; set; }
        public string? Section { get; set; }
        public int? SectionId { get; set; }
        public string? Question { get; set; }
        public double? ItemValue { get; set; }
        public string? ItemMemo { get; set; }
    }

    public class AgentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class CampaignDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
    }

    public class StatistiqueExportDto
    {
        public string Format { get; set; } = "PDF"; // PDF | XLS | CSV | RTF
        public StatistiqueFilterDto Filter { get; set; } = new();
        /// <summary>Colonnes visibles côté frontend au moment de l'export</summary>
        public List<string> Columns { get; set; } = new();
    }
}