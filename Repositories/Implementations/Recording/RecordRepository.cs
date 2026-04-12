using System.IO.Compression;
using System.Text;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Admin;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Repositories.Implementations.Evaluation
{
    public class RecordRepository : IRecordRepository
    {
        private readonly SqrScoringContext _db;
        private readonly SqrAdminContext   _adminDb;

        public RecordRepository(SqrScoringContext db, SqrAdminContext adminDb)
        {
            _db      = db;
            _adminDb = adminDb;
        }

      public async Task<PagedRecordsDto> GetRecordsByFilterAsync(
    RecordFilterDto filter, int userId, string userRole)
{
    Console.WriteLine($"=== DEBUG === userId={userId}, userRole='{userRole}'");

    var query = _db.RecordData.AsQueryable();

    // ── Filtrage Agent (SQL) ──────────────────────────────────────────────
    if (userRole == "Agent")
    {
        var agentOid = await _adminDb.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Oid)
            .FirstOrDefaultAsync();

        query = agentOid != null
            ? query.Where(r => r.AgentOid == agentOid)
            : query.Where(r => false);
    }

    // ── Filtres date ──────────────────────────────────────────────────────
   var dtFrom = filter.DateDebut.HasValue
    ? new DateTime(filter.DateDebut.Value.Year, filter.DateDebut.Value.Month, filter.DateDebut.Value.Day, 0, 0, 0)
    : (DateTime?)null;

var dtTo = filter.DateFin.HasValue
    ? new DateTime(filter.DateFin.Value.Year, filter.DateFin.Value.Month, filter.DateFin.Value.Day, 23, 59, 59)
    : (DateTime?)null;

if (dtFrom.HasValue)
    query = query.Where(r => r.CallLocalTime >= dtFrom.Value);

if (dtTo.HasValue)
    query = query.Where(r => r.CallLocalTime <= dtTo.Value);
    query = query.OrderByDescending(r => r.CallLocalTime);

    var allRecords = await query.ToListAsync();

    // ── Filtrage Admin / Supervisor par site (en mémoire) ─────────────────
    if (userRole == "Admin" || userRole == "Supervisor")
    {
        var userSiteId = await _adminDb.Users
            .Where(u => u.Id == userId)
            .Select(u => u.SiteId)
            .FirstOrDefaultAsync();

        Console.WriteLine($"=== DEBUG SITE === userSiteId={userSiteId}");

        if (userSiteId == null)
        {
            Console.WriteLine("=== DEBUG SITE === Aucun site trouvé → 0 enregistrements");
            allRecords = new List<RecordDatum>();
        }
        else
        {
            var siteAgentOids = await _adminDb.Users
                .Where(u => u.SiteId == userSiteId && u.Oid != null)
                .Select(u => u.Oid!)
                .ToListAsync();

            Console.WriteLine($"=== DEBUG SITE === siteAgentOids count={siteAgentOids.Count}");

            var siteAgentOidsSet = new HashSet<string>(
                siteAgentOids, StringComparer.OrdinalIgnoreCase);

            allRecords = siteAgentOidsSet.Any()
                ? allRecords
                    .Where(r => r.AgentOid != null && siteAgentOidsSet.Contains(r.AgentOid))
                    .ToList()
                : new List<RecordDatum>();
        }
    }
    // SuperAdmin / SuperUser → allRecords non filtré, tout est visible

    // ── Filtre AgentOids ──────────────────────────────────────────────────
    if (filter.AgentOids != null && filter.AgentOids.Any())
    {
        var agentSet = new HashSet<string>(
            filter.AgentOids.Select(a => a.Trim()),
            StringComparer.OrdinalIgnoreCase);
        allRecords = allRecords
            .Where(r => r.AgentOid != null && agentSet.Contains(r.AgentOid.Trim()))
            .ToList();
    }

    // ── Filtre colonnes dynamiques (MantineReactTable) ────────────────────
    if (filter.ColumnFilters != null && filter.ColumnFilters.Any())
    {
        foreach (var cf in filter.ColumnFilters)
        {
            var value = cf.Value?.ToLower()?.Trim();
            if (string.IsNullOrEmpty(value)) continue;

            allRecords = cf.Id switch
            {
                "nomAgent"            => allRecords.Where(r => r.NomAgent?.ToLower().Contains(value) == true).ToList(),
                "prenomAgent"         => allRecords.Where(r => r.PrenomAgent?.ToLower().Contains(value) == true).ToList(),
                "campaignDescription" => allRecords.Where(r => r.CampaignDescription?.ToLower().Contains(value) == true).ToList(),
                "callTypeDescription" => allRecords.Where(r => r.CallTypeDescription?.ToLower().Contains(value) == true).ToList(),
                "numeroTel"           => allRecords.Where(r => r.NumeroTel?.ToLower().Contains(value) == true).ToList(),
                "statusRequal"        => allRecords.Where(r => r.StatusRequal?.ToLower().Contains(value) == true).ToList(),
                "agentOid"            => allRecords.Where(r => r.AgentOid?.ToLower().Contains(value) == true).ToList(),
                "callLocalTime"       => allRecords.Where(r => r.CallLocalTimeString?.ToLower().Contains(value) == true).ToList(),
                "agentId"             => allRecords.Where(r => r.LastAgent != null && r.LastAgent.ToString()!.Contains(value)).ToList(),
                "lastAgent"           => allRecords.Where(r => r.LastAgent != null && r.LastAgent.ToString()!.Contains(value)).ToList(),
                "duration"            => allRecords.Where(r => r.Duration  != null && r.Duration.ToString()!.Contains(value)).ToList(),
                "typeRequalif"        => allRecords.Where(r => r.TypeRequalif?.ToString().ToLower().Contains(value) == true).ToList(),
                "lsId"                => allRecords.Where(r => r.LsId?.ToString().Contains(value) == true).ToList(),
                _                     => allRecords
            };
        }
    }

    // ── Filtre utilisateur sauvegardé (FilterId) ──────────────────────────
    if (filter.FilterId.HasValue && filter.FilterId.Value > 0)
    {
        var filtre = await _db.Filtres
            .Where(f => f.Id == filter.FilterId.Value)
            .FirstOrDefaultAsync();

        Console.WriteLine($"=== FILTER === FilterId={filter.FilterId}, found={filtre != null}, expression='{filtre?.Expression}'");

        if (filtre != null && !string.IsNullOrWhiteSpace(filtre.Expression))
            allRecords = ApplyUserFilterExpression(allRecords, filtre.Expression);
    }

    var totalCount = allRecords.Count;

    int pageSize = filter.NbRecords.HasValue ? filter.NbRecords.Value : filter.PageSize;
    int page     = filter.NbRecords.HasValue ? 1                      : filter.Page;

    var recordsList = allRecords
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    // ── HasEvaluation ─────────────────────────────────────────────────────
    var allSurveyIds = await _db.LsSurveys
.Where(s => s.RecordDataId != null && s.IsSaved == 1)
        .Select(s => s.RecordDataId!.Value)
        .ToListAsync();
    var evaluatedSet = new HashSet<int>(allSurveyIds);

    // ── HasHistory (ap_action) ────────────────────────────────────────────
    var allActionRecordIds = await _db.ApActions
        .Where(a => a.RecordedId != null)
        .Select(a => a.RecordedId!.Value)
        .Distinct()
        .ToListAsync();
    var historySet = new HashSet<int>(allActionRecordIds);

    // ── HasHistoryScreen (vp_action) ──────────────────────────────────────
var allScreenActions = await _db.VpActions
    .Where(a => a.RecordedId != null && a.ScreenFileName != null)
    .Select(a => new { a.RecordedId, a.ScreenFileName })
    .ToListAsync();

var screenSet = new HashSet<int>(
    allScreenActions.Select(a => a.RecordedId!.Value).Distinct());

// ── ScreenSource depuis VWListRecordGV ────────────────────────────────────
var recordIds = recordsList.Select(r => r.Id).ToList();


    var recordsDto = recordsList.Select(r => new RecordDataDto
    {
        Id                  = r.Id,
        CampaignDescription = r.CampaignDescription,
        AgentOid            = r.AgentOid,
        NomAgent            = r.NomAgent,
        PrenomAgent         = r.PrenomAgent,
        CallLocalTime       = r.CallLocalTime,
        CallLocalTimeString = r.CallLocalTimeString,
        StatusRequal        = r.StatusRequal,
        StatusDescription   = r.StatusDescription,
        CallTypeDescription = r.CallTypeDescription,
        NumeroTel           = r.NumeroTel,
        AgentId             = r.LastAgent,
        Duration            = r.Duration,
        HasEvaluation       = evaluatedSet.Contains(r.Id),
        HasHistory          = historySet.Contains(r.Id),
        HasHistoryScreen    = screenSet.Contains(r.Id),
        LsId                = r.LsId,
        TypeRequalif        = r.TypeRequalif,
        RecIdLink           = r.RecIdLink,   // ← cette ligne manque

    }).ToList();

    return new PagedRecordsDto
    {
        Records    = recordsDto,
        TotalCount = totalCount,
        Page       = page,
        PageSize   = pageSize
    };
}

        // ══════════════════════════════════════════════
        //  APPLIQUE L'EXPRESSION DU FILTRE UTILISATEUR
        //
        //  Format nouveau (CreateFilterModal) :
        //    "nomAgent contains Ahmed And campaignDescription equals OUTBOUND"
        //
        //  Format ancien (Access/SQL) :
        //    "[NomAgent] LIKE '%Ahmed%'" ou "NomAgent LIKE '%ahmed%'"
        //    → appliqué via SqlWhere directement (non supporté en mémoire)
        //    → dans ce cas on utilise la colonne Expression si elle contient
        //      le format "field operator value"
        // ══════════════════════════════════════════════
        private static List<RecordDatum> ApplyUserFilterExpression(
            List<RecordDatum> records, string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return records;

            var tokens = expression.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // ── Détection du format ───────────────────────────────────────
            // Format nouveau : 3+ tokens dont le 2e est un opérateur connu
            var knownOperators = new HashSet<string>
                { "contains", "equals", "startswith", "endswith", "notcontains" };

            bool isNewFormat = tokens.Length >= 3 &&
                               knownOperators.Contains(tokens[1].ToLower());

            if (!isNewFormat)
            {
                // Format ancien non supporté en mémoire → retourner tel quel
                Console.WriteLine($"=== FILTER === format ancien non supporté en mémoire: '{expression}'");
                return records;
            }

            // ── Parsing format nouveau ────────────────────────────────────
            var result  = records;
            int i       = 0;
            bool isFirst = true;

            while (i < tokens.Length)
            {
                string logic = "And";
                if (!isFirst)
                {
                    if (i >= tokens.Length) break;
                    logic = tokens[i].ToLower() == "or" ? "Or" : "And";
                    i++;
                }

                if (i + 2 > tokens.Length) break;

                var field  = tokens[i];
                var oper   = i + 1 < tokens.Length ? tokens[i + 1] : "";
                var value  = i + 2 < tokens.Length ? tokens[i + 2] : "";
                i += 3;

                var filtered = ApplySingleCondition(records, field, oper, value);

                if (isFirst)
                {
                    result  = filtered;
                    isFirst = false;
                }
                else if (logic == "Or")
                {
                    result = result.Union(filtered, new RecordDatumIdComparer()).ToList();
                }
                else
                {
                    var ids = new HashSet<int>(filtered.Select(r => r.Id));
                    result  = result.Where(r => ids.Contains(r.Id)).ToList();
                }
            }

            return result;
        }

        private static List<RecordDatum> ApplySingleCondition(
            List<RecordDatum> records, string field, string oper, string value)
        {
            var v = value.ToLower();
            Func<RecordDatum, bool> pred = (field.ToLower(), oper.ToLower()) switch
            {
                ("nomagent",            "contains")    => r => r.NomAgent?.ToLower().Contains(v) == true,
                ("nomagent",            "equals")      => r => r.NomAgent?.ToLower() == v,
                ("nomagent",            "startswith")  => r => r.NomAgent?.ToLower().StartsWith(v) == true,
                ("nomagent",            "endswith")    => r => r.NomAgent?.ToLower().EndsWith(v) == true,
                ("nomagent",            "notcontains") => r => r.NomAgent?.ToLower().Contains(v) != true,
                ("prenomagent",         "contains")    => r => r.PrenomAgent?.ToLower().Contains(v) == true,
                ("prenomagent",         "equals")      => r => r.PrenomAgent?.ToLower() == v,
                ("campaigndescription", "contains")    => r => r.CampaignDescription?.ToLower().Contains(v) == true,
                ("campaigndescription", "equals")      => r => r.CampaignDescription?.ToLower() == v,
                ("calltypedescription", "contains")    => r => r.CallTypeDescription?.ToLower().Contains(v) == true,
                ("calltypedescription", "equals")      => r => r.CallTypeDescription?.ToLower() == v,
                ("numerotel",           "contains")    => r => r.NumeroTel?.ToLower().Contains(v) == true,
                ("numerotel",           "equals")      => r => r.NumeroTel?.ToLower() == v,
                ("statusrequalif",      "contains")    => r => r.StatusRequal?.ToLower().Contains(v) == true,
                ("statusrequalif",      "equals")      => r => r.StatusRequal?.ToLower() == v,
                ("agentoid",            "contains")    => r => r.AgentOid?.ToLower().Contains(v) == true,
                ("agentoid",            "equals")      => r => r.AgentOid?.ToLower() == v,
                _                                      => _ => true
            };
            return records.Where(pred).ToList();
        }

        private sealed class RecordDatumIdComparer : IEqualityComparer<RecordDatum>
        {
            public bool Equals(RecordDatum? x, RecordDatum? y) => x?.Id == y?.Id;
            public int  GetHashCode(RecordDatum r)              => r.Id.GetHashCode();
        }

        // ══════════════════════════════════════════════
        //  GET BY ID
        // ══════════════════════════════════════════════
        public async Task<RecordDataDto?> GetByIdAsync(int recordId)
        {
            var r = await _db.RecordData.FirstOrDefaultAsync(x => x.Id == recordId);
            if (r == null) return null;

            var hasEval    = await _db.LsSurveys.AnyAsync(s => s.RecordDataId == recordId);
            var hasHistory = await _db.ApActions.AnyAsync(a => a.RecordedId   == recordId);

            return new RecordDataDto
            {
                Id                  = r.Id,
                CampaignDescription = r.CampaignDescription,
                AgentOid            = r.AgentOid,
                NomAgent            = r.NomAgent,
                PrenomAgent         = r.PrenomAgent,
                CallLocalTime       = r.CallLocalTime,
                CallLocalTimeString = r.CallLocalTimeString,
                StatusRequal        = r.StatusRequal,
                StatusDescription   = r.StatusDescription,
                CallTypeDescription = r.CallTypeDescription,
                NumeroTel           = r.NumeroTel,
                Duration            = r.Duration,
                HasEvaluation       = hasEval,
                HasHistory          = hasHistory,
                LsId                = r.LsId,
                TypeRequalif        = r.TypeRequalif
            };
        }

        // ══════════════════════════════════════════════
        //  GET RECORD SOURCE PATH
        // ══════════════════════════════════════════════
        public async Task<string?> GetRecordSourceAsync(int recordId)
        {
            var r = await _db.RecordData
                .Where(x => x.Id == recordId)
                .Select(x => new { x.RecFilename, x.FullRecFilename })
                .FirstOrDefaultAsync();
            if (r == null) return null;
            return !string.IsNullOrWhiteSpace(r.FullRecFilename) ? r.FullRecFilename : r.RecFilename;
        }

        // ══════════════════════════════════════════════
        //  REQUALIFICATION
        // ══════════════════════════════════════════════
        public async Task SaveRequalificationAsync(RequalificationDto dto)
        {
            var record = await _db.RecordData.FindAsync(dto.RecordId)
                ?? throw new Exception($"Record {dto.RecordId} introuvable");
            record.StatusRequal       = dto.StatusRequal;
            record.StatusGroupeRequal = dto.StatusGroupeRequal;
            record.StatusNumRequal    = dto.StatusNumRequal;
            record.StatusDetailRequal = dto.StatusDetailRequal;
            if (int.TryParse(dto.TypeRequalif, out var typeInt))
                record.TypeRequalif = typeInt;
            await _db.SaveChangesAsync();
        }

        // ══════════════════════════════════════════════
        //  TRACE ACTION
        // ══════════════════════════════════════════════
        public async Task SaveTraceAsync(TraceActionDto dto, int userId)
        {
            var action = new ApAction
            {
                RecordedId     = dto.RecordId,
                UserId         = userId,
                RecordPosition = dto.Position,
                Duration       = dto.Duration,
                CreateDate     = DateTime.Now,
                CodeEvent      = MapEventCode(dto.EventType),
                RecordFileName = dto.FileName
            };
            _db.ApActions.Add(action);
            await _db.SaveChangesAsync();
        }

        private static int MapEventCode(string eventType) => eventType switch
        {
            "Loaded" => 0, "Play" => 1, "Pause" => 2, "Ended" => 3,
            "SeekReversed" => 4, "SeekForward" => 5,
            "Mute" => 8, "Stop" => 9, "Unmute" => 10, _ => 0
        };
// ══════════════════════════════════════════════
//  TRACE SCREEN ACTION (vp_action)
// ══════════════════════════════════════════════
public async Task SaveScreenTraceAsync(TraceActionDto dto, int userId)
{
    // ── Récupérer le ScreenSource depuis VwlistRecordGvs ──────────────────
    var screenSource = await _db.VwlistRecordGvs
        .Where(v => v.Id == dto.RecordId)
        .Select(v => v.ScreenSource)
        .FirstOrDefaultAsync();

    var action = new VpAction
    {
        RecordedId     = dto.RecordId,
        UserId         = userId,
        ScreenPosition = FormatSeconds(dto.Position),
        Duration       = FormatSeconds(dto.Duration),
        CreateDate     = DateTime.Now,
        CodeEvent      = MapEventCode(dto.EventType),
        ScreenFileName = screenSource ?? dto.FileName  // fallback si vue retourne null
    };

    _db.VpActions.Add(action);
    await _db.SaveChangesAsync();
}

private static string FormatSeconds(string? raw)
{
    if (string.IsNullOrWhiteSpace(raw)) return "00:00";
    if (!double.TryParse(raw, out var secs) || double.IsNaN(secs)) return "00:00";
    var ts = TimeSpan.FromSeconds(secs);
    return $"{(int)ts.TotalMinutes:D2}:{ts.Seconds:D2}";
}
        // ══════════════════════════════════════════════
        //  EXPORT CSV
        // ══════════════════════════════════════════════
        public async Task<byte[]> ExportToCsvAsync(
            RecordFilterDto filter, int userId, string userRole)
        {
            var fullFilter = new RecordFilterDto
            {
                DateDebut = filter.DateDebut,
                DateFin   = filter.DateFin,
                AgentOids = filter.AgentOids,
                FilterId  = filter.FilterId,   // ← propager le FilterId
                Page      = 1,
                PageSize  = 100_000
            };
            var result = await GetRecordsByFilterAsync(fullFilter, userId, userRole);
            var sb = new StringBuilder();
            sb.AppendLine("Id;Date;Campagne;AgentOid;Nom;Prénom;Heure;Statut;Description Statut;Type Appel;Téléphone;Durée;Évaluation;LsId");
            foreach (var rec in result.Records)
                sb.AppendLine(string.Join(";",
                    rec.Id, rec.CallLocalTime?.ToString("dd/MM/yyyy"),
                    Escape(rec.CampaignDescription), Escape(rec.AgentOid),
                    Escape(rec.NomAgent), Escape(rec.PrenomAgent),
                    Escape(rec.CallLocalTimeString), Escape(rec.StatusRequal),
                    Escape(rec.StatusDescription), Escape(rec.CallTypeDescription),
                    Escape(rec.NumeroTel), rec.Duration,
                    rec.HasEvaluation ? "Oui" : "Non",
                    rec.LsId?.ToString() ?? ""));
            return Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
        }

        // ══════════════════════════════════════════════
        //  HISTORIQUE D'ÉCOUTE (ap_action)
        // ══════════════════════════════════════════════
        public async Task<List<ListenHistoryDto>> GetListenHistoryAsync(int recordId)
        {
            var eventNames = new Dictionary<int, string>
            { {0,"Load Player"},{1,"Play"},{2,"Pause"},{3,"Ended"},{4,"Seek Reverse"},{5,"Seek Forward"},{8,"Mute"},{9,"Stop"},{10,"Unmute"} };
            var roleNames = new Dictionary<int, string>
            { {1,"Super Admin"},{2,"Admin"},{3,"Supervisor"},{4,"Agent"},{5,"Super User"} };

            var actions = await _db.ApActions
                .Where(a => a.RecordedId == recordId)
                .OrderBy(a => a.CreateDate)
                .ToListAsync();
            if (!actions.Any()) return new List<ListenHistoryDto>();

            var userIds      = actions.Select(a => a.UserId).Distinct().ToList();
            var parameters   = userIds.Select((id, i) => new Microsoft.Data.SqlClient.SqlParameter($"@p{i}", id)).ToArray();
            var placeholders = string.Join(", ", userIds.Select((_, i) => $"@p{i}"));
            var users        = await _adminDb.Users.FromSqlRaw($"SELECT * FROM Users WHERE Id IN ({placeholders})", parameters).ToListAsync();
            var userDict     = users.ToDictionary(u => u.Id);

            return actions.Select(a =>
            {
                var user      = userDict.TryGetValue(a.UserId, out var u) ? u : null;
                return new ListenHistoryDto
                {
                    Login     = user?.Login     ?? "",
                    FirstName = user?.FirstName ?? "",
                    LastName  = user?.LastName  ?? "",
                    Profil    = roleNames.TryGetValue(user?.Role ?? 0, out var rn) ? rn : (user?.Role ?? 0).ToString(),
                    Action    = eventNames.TryGetValue(a.CodeEvent, out var ev) ? ev : a.CodeEvent.ToString(),
                    Position  = a.RecordPosition ?? "00:00",
                    Date      = a.CreateDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    Statut    = a.Duration ?? "",
                };
            }).ToList();
        }

        // ══════════════════════════════════════════════
        //  HISTORIQUE SCREEN (vp_action)
        // ══════════════════════════════════════════════
        public async Task<List<ListenHistoryDto>> GetScreenHistoryAsync(int recordId)
        {
            var eventNames = new Dictionary<int, string>
            { {0,"Load Player"},{1,"Play"},{2,"Pause"},{3,"Ended"},{4,"Seek Reverse"},{5,"Seek Forward"},{8,"Mute"},{9,"Stop"},{10,"Unmute"} };
            var roleNames = new Dictionary<int, string>
            { {1,"Super Admin"},{2,"Admin"},{3,"Supervisor"},{4,"Agent"},{5,"Super User"} };

            var actions = await _db.VpActions
                .Where(a => a.RecordedId == recordId)
                .OrderBy(a => a.CreateDate)
                .ToListAsync();
            if (!actions.Any()) return new List<ListenHistoryDto>();

            var userIds      = actions.Select(a => a.UserId).Distinct().ToList();
            var parameters   = userIds.Select((id, i) => new Microsoft.Data.SqlClient.SqlParameter($"@p{i}", id)).ToArray();
            var placeholders = string.Join(", ", userIds.Select((_, i) => $"@p{i}"));
            var users        = await _adminDb.Users.FromSqlRaw($"SELECT * FROM Users WHERE Id IN ({placeholders})", parameters).ToListAsync();
            var userDict     = users.ToDictionary(u => u.Id);

            return actions.Select(a =>
            {
                var user = userDict.TryGetValue(a.UserId, out var u) ? u : null;
                return new ListenHistoryDto
                {
                    Login     = user?.Login     ?? "",
                    FirstName = user?.FirstName ?? "",
                    LastName  = user?.LastName  ?? "",
                    Profil    = roleNames.TryGetValue(user?.Role ?? 0, out var rn) ? rn : (user?.Role ?? 0).ToString(),
                    Action    = eventNames.TryGetValue(a.CodeEvent, out var ev) ? ev : a.CodeEvent.ToString(),
                    Position  = a.ScreenPosition ?? "00:00",
                    Date      = a.CreateDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    Statut    = a.Duration ?? "",
                };
            }).ToList();
        }



        // ══════════════════════════════════════════════
        //  SUPPRESSION
        // ══════════════════════════════════════════════
        public async Task DeleteRecordAsync(int recordId)
        {
            var exists = await _db.RecordData.AnyAsync(r => r.Id == recordId);
            if (!exists) throw new Exception($"Record {recordId} introuvable.");
            await _db.Database.ExecuteSqlRawAsync("DELETE FROM ap_action WHERE RecordedId = {0}", recordId);
            await _db.Database.ExecuteSqlRawAsync("DELETE FROM ls_survey WHERE RecordDataId = {0}", recordId);
            await _db.Database.ExecuteSqlRawAsync("DELETE FROM RecordData WHERE Id = {0}", recordId);
        }

        private static string Escape(string? value)
            => value == null ? "" : $"\"{value.Replace("\"", "\"\"")}\"";

    }
}