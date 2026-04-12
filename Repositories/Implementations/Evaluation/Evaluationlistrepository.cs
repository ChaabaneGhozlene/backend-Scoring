using System.Data;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Repositories.Implementations.Evaluation
{
    public class EvaluationListRepository : IEvaluationListRepository
    {
        private readonly SqrScoringContext _db;

        public EvaluationListRepository(SqrScoringContext db) => _db = db;

        public async Task<(List<LsFicheDto> Items, int TotalCount)> GetLsFichesAsync(
            int userId, int userRole, int userType, int userSiteId, string userLogin,
            EvaluationListFilterDto filter)
        {
            var dtFrom = new DateTime(
                (filter.DateDebut ?? DateTime.Today).Year,
                (filter.DateDebut ?? DateTime.Today).Month,
                (filter.DateDebut ?? DateTime.Today).Day, 0, 0, 0);

            var dtTo = new DateTime(
                (filter.DateFin ?? DateTime.Today).Year,
                (filter.DateFin ?? DateTime.Today).Month,
                (filter.DateFin ?? DateTime.Today).Day, 23, 59, 59);

            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            // ── Étape 1 : SurveyIds sauvegardés dans la période ──
            var validSurveyIds = new List<int>();
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT s.Id
                    FROM Ls_survey s
                    WHERE s.Is_saved     = 1
                      AND s.RecordDataId IS NOT NULL
                      AND s.CreateDate  >= @dtFrom
                      AND s.CreateDate  <= @dtTo";
                AddParam(cmd, "@dtFrom", dtFrom);
                AddParam(cmd, "@dtTo",   dtTo);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    validSurveyIds.Add(reader.GetInt32(0));
            }

            if (!validSurveyIds.Any())
                return (new List<LsFicheDto>(), 0);

            // ── Étape 1b : Appliquer FilterId si fourni ──
            if (filter.FilterId.HasValue)
            {
                string? sqlWhere = null;
                await using (var cmdF = conn.CreateCommand())
                {
                    cmdF.CommandText = @"
                        SELECT SqlWhere FROM Filtre
                        WHERE Id = @id AND UserLogin = @uid";
                    AddParam(cmdF, "@id",  filter.FilterId.Value);
                    AddParam(cmdF, "@uid", userLogin);
                    var result = await cmdF.ExecuteScalarAsync();
                    sqlWhere = (result == null || result == DBNull.Value) ? null : result.ToString();
                }

                if (!string.IsNullOrWhiteSpace(sqlWhere))
                {
                    var surveyIdList = string.Join(",", validSurveyIds);
                    var filteredIds  = new List<int>();

                    await using var cmdW = conn.CreateCommand();
                    cmdW.CommandText = $@"
                        SELECT s.Id FROM Ls_survey s
                        JOIN Ls l ON l.Id = s.LsId
                        WHERE s.Id IN ({surveyIdList})
                          AND ({sqlWhere})";

                    await using var readerW = await cmdW.ExecuteReaderAsync();
                    while (await readerW.ReadAsync())
                        filteredIds.Add(readerW.GetInt32(0));

                    validSurveyIds = filteredIds;
                    if (!validSurveyIds.Any())
                        return (new List<LsFicheDto>(), 0);
                }
            }

            // ── Étape 1c : Filtres colonnes ──
            if (filter.ColumnFilters != null && filter.ColumnFilters.Any())
            {
                var surveyIdList = string.Join(",", validSurveyIds);
                var conditions   = new List<string>();

                foreach (var cf in filter.ColumnFilters)
                {
                    switch (cf.Id)
                    {
                        case "agent":
                            conditions.Add($"l.Agent LIKE '%{cf.Value.Replace("'", "''")}%'");
                            break;
                        case "agentId" when int.TryParse(cf.Value, out var agId):
                            conditions.Add($"l.AgentId = {agId}");
                            break;
                        case "score" when double.TryParse(cf.Value, out var sc):
                            conditions.Add($"s.Score >= {sc}");
                            break;
                    }
                }

                if (conditions.Any())
                {
                    var filteredIds = new List<int>();
                    await using var cmd = conn.CreateCommand();
                    cmd.CommandText = $@"
                        SELECT s.Id FROM Ls_survey s
                        JOIN Ls l ON l.Id = s.LsId
                        WHERE s.Id IN ({surveyIdList})
                          AND {string.Join(" AND ", conditions)}";

                    await using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                        filteredIds.Add(reader.GetInt32(0));

                    validSurveyIds = filteredIds;
                    if (!validSurveyIds.Any())
                        return (new List<LsFicheDto>(), 0);
                }
            }

            var surveyList = string.Join(",", validSurveyIds);

            // ── Étape 2 : Filtre rôle ──
            string roleFilter = (userRole, userType) switch
            {
                (2, 2) => "AND l.CalledCampaignId IN (SELECT Id FROM Ls_CalledCampaign WHERE Site = @pSite)",
                (3, 2) => "AND l.Auditor = @pUser",
                (5, 1) => "AND l.CalledCampaignId IN (SELECT Id FROM Ls_CalledCampaign WHERE Site = @pSite)",
                _      => ""
            };
            bool needsSite = (userRole, userType) is (2, 2) or (5, 1);
            bool needsUser = (userRole, userType) is (3, 2);

            int offset = (filter.Page - 1) * filter.PageSize;

            // ── Étape 3 : Total ──
            int total = 0;
            await using (var cmdTotal = conn.CreateCommand())
            {
                cmdTotal.CommandText = $@"
                    SELECT COUNT(*) FROM (
                        SELECT s.RecordDataId
                        FROM Ls_survey s
                        JOIN Ls l ON l.Id = s.LsId
                        WHERE s.Id IN ({surveyList})
                        {roleFilter}
                        GROUP BY
                            l.Id, l.Agent, l.AgentOid, l.AgentId, l.Auditor,
                            l.StartPeriode, l.EndPeriode, l.CreateDate, l.UpdateDate,
                            l.CalledCampaignId, s.RecordDataId
                    ) AS grouped";
                if (needsSite) AddParam(cmdTotal, "@pSite", userSiteId);
                if (needsUser) AddParam(cmdTotal, "@pUser", userId);
                total = Convert.ToInt32(await cmdTotal.ExecuteScalarAsync());
            }

            // ── Étape 4 : UNE LIGNE PAR SURVEY ──
            var items        = new List<LsFicheDto>();
            var campIds      = new List<int>();
            var campIdPerRow = new List<int>();

            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $@"
                    SELECT
                        l.Id               AS LsId,            -- 0
                        l.Agent,                               -- 1
                        l.AgentOid,                            -- 2
                        l.AgentId,                             -- 3
                        l.Auditor,                             -- 4
                        l.StartPeriode,                        -- 5
                        l.EndPeriode,                          -- 6
                        l.CreateDate       AS LsCreateDate,    -- 7
                        l.UpdateDate       AS LsUpdateDate,    -- 8
                        l.CalledCampaignId,                    -- 9
                        MAX(s.Id)          AS SurveyId,        -- 10
                        AVG(CAST(s.Score AS FLOAT)) AS SurveyScore, -- 11
                        MAX(s.CreateDate)  AS SurveyDate,      -- 12
                        s.RecordDataId,                        -- 13
                        -- ✅ nom complet depuis SQR_Admin.dbo.Users
                        ISNULL(u.FirstName + ' ' + u.LastName, l.userName) AS AuditorName -- 14
                    FROM Ls_survey s
                    JOIN Ls l ON l.Id = s.LsId
                    LEFT JOIN SQR_Admin.dbo.Users u ON u.ID = l.Auditor
                    WHERE s.Id IN ({surveyList})
                    {roleFilter}
                    GROUP BY
                        l.Id, l.Agent, l.AgentOid, l.AgentId, l.Auditor,
                        l.StartPeriode, l.EndPeriode, l.CreateDate, l.UpdateDate,
                        l.CalledCampaignId, s.RecordDataId,
                        l.userName,
                        u.FirstName, u.LastName
                    ORDER BY l.AgentId ASC, MAX(s.CreateDate) DESC
                    OFFSET {offset} ROWS FETCH NEXT {filter.PageSize} ROWS ONLY";

                if (needsSite) AddParam(cmd, "@pSite", userSiteId);
                if (needsUser) AddParam(cmd, "@pUser", userId);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int campId = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    campIdPerRow.Add(campId);
                    if (campId > 0 && !campIds.Contains(campId))
                        campIds.Add(campId);

                    items.Add(new LsFicheDto
                    {
                        Id           = reader.GetInt32(0),
                        Agent        = reader.IsDBNull(1)  ? "" : reader.GetString(1),
                        AgentOid     = reader.IsDBNull(2)  ? "" : reader.GetString(2),
                        AgentId      = reader.IsDBNull(3)  ? 0  : reader.GetInt32(3),
                        Auditor      = reader.IsDBNull(4)  ? 0  : reader.GetInt32(4),
                        StartPeriode = reader.IsDBNull(5)  ? DateTime.MinValue : reader.GetDateTime(5),
                        EndPeriode   = reader.IsDBNull(6)  ? DateTime.MinValue : reader.GetDateTime(6),
                        CreateDate   = reader.IsDBNull(7)  ? DateTime.MinValue : reader.GetDateTime(7),
                        UpdateDate   = reader.IsDBNull(8)  ? DateTime.MinValue : reader.GetDateTime(8),
                        SurveyId     = reader.GetInt32(10),
                        Score        = reader.IsDBNull(11) ? 0 : Math.Round(Convert.ToDouble(reader.GetValue(11)), 2),
                        RecordDataId = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                        AuditorName  = reader.IsDBNull(14) ? "" : reader.GetString(14), // ✅
                    });
                }
            }

            if (!items.Any())
                return (new List<LsFicheDto>(), total);

            // ── Étape 5 : RecordDate depuis RecordData ──
            var recordIds = items
                .Where(x => x.RecordDataId > 0)
                .Select(x => x.RecordDataId)
                .Distinct().ToList();

            if (recordIds.Any())
            {
                var recIdList   = string.Join(",", recordIds);
                var recordDates = new Dictionary<int, string>();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = $@"
                    SELECT ID, Record_Date
                    FROM RecordData
                    WHERE ID IN ({recIdList})";

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int    id   = reader.GetInt32(0);
                    string date = reader.IsDBNull(1) ? ""
                                : reader.GetDateTime(1).ToString("dd/MM/yyyy");
                    recordDates[id] = date;
                }

                foreach (var item in items)
                    item.RecordDate = recordDates.GetValueOrDefault(item.RecordDataId, "");
            }

            // ── Étape 6 : Campaign / ModeleName ──
            if (campIds.Any())
            {
                var campIdList       = string.Join(",", campIds);
                var campDescriptions = new Dictionary<int, string>();
                var campModelNames   = new Dictionary<int, string>();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = $@"
                    SELECT c.Id, c.Description, t.Description AS ModeleName
                    FROM Ls_CalledCampaign c
                    LEFT JOIN Ls_template t ON t.Id = c.IdLsTemplate
                    WHERE c.Id IN ({campIdList})";

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int    id     = reader.GetInt32(0);
                    string desc   = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string modele = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    campDescriptions[id] = desc;
                    campModelNames[id]   = modele;
                }

                for (int i = 0; i < items.Count; i++)
                {
                    int cid = campIdPerRow[i];
                    items[i].CampaignName = campDescriptions.GetValueOrDefault(cid, "");
                    items[i].ModeleName   = campModelNames.GetValueOrDefault(cid, "");
                }
            }

            return (items, total);
        }

        // ─────────────────────────────────────────────────────
        // 2. Surveys d'une fiche
        // ─────────────────────────────────────────────────────
        public async Task<List<LsSurveyDto>> GetSurveysByLsIdAsync(int lsId)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            var surveys = new List<LsSurveyDto>();

            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT
                        s.Id, s.LsId, s.CreateDate, s.UpdateDate,
                        s.Score, s.Is_saved, s.Memo, s.MemoActionTaken,
                        s.RecordDataId,
                        cat.Des_Categories,
                        cr.Des_CallReason
                    FROM Ls_survey s
                    LEFT JOIN Ls_categories  cat ON cat.Id = s.Id_Categories
                    LEFT JOIN Ls_CallReason  cr  ON cr.Id  = s.Id_CallReason
                    WHERE s.LsId         = @lsId
                      AND s.RecordDataId IS NOT NULL
                      AND s.Is_saved     = 1
                    ORDER BY s.CreateDate ASC";

                AddParam(cmd, "@lsId", lsId);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    surveys.Add(new LsSurveyDto
                    {
                        Id              = reader.GetInt32(0),
                        LsId            = reader.GetInt32(1),
                        CreateDate      = reader.IsDBNull(2)  ? DateTime.MinValue : reader.GetDateTime(2),
                        UpdateDate      = reader.IsDBNull(3)  ? DateTime.MinValue : reader.GetDateTime(3),
                        Score           = reader.IsDBNull(4)  ? 0 : Math.Round((double)reader.GetFloat(4), 2),
                        IsSaved         = reader.IsDBNull(5)  ? 0 : reader.GetInt32(5),
                        Memo            = reader.IsDBNull(6)  ? "" : reader.GetString(6),
                        MemoActionTaken = reader.IsDBNull(7)  ? "" : reader.GetString(7),
                        RecordDataId    = reader.IsDBNull(8)  ? 0 : reader.GetInt32(8),
                        CategoryName    = reader.IsDBNull(9)  ? "" : reader.GetValue(9).ToString()!,
                        CallReasonName  = reader.IsDBNull(10) ? "" : reader.GetValue(10).ToString()!,
                        Items           = new List<SurveyItemDto>()
                    });
                }
            }

            if (surveys.Any())
            {
                var recordIds = surveys
                    .Where(s => s.RecordDataId > 0)
                    .Select(s => s.RecordDataId)
                    .Distinct().ToList();

                var idList = string.Join(",", recordIds);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = $@"
                    SELECT ID, Record_Date
                    FROM RecordData
                    WHERE ID IN ({idList})";

                var recordDates = new Dictionary<int, string>();
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int    id   = reader.GetInt32(0);
                    string date = reader.IsDBNull(1) ? ""
                                : reader.GetDateTime(1).ToString("dd/MM/yyyy");
                    recordDates[id] = date;
                }

                foreach (var s in surveys)
                    s.RecordDate = recordDates.GetValueOrDefault(s.RecordDataId, "");
            }

            return surveys;
        }

        // ─────────────────────────────────────────────────────
        // 3. Items d'une survey
        // ─────────────────────────────────────────────────────
        public async Task<List<SurveyItemDto>> GetSurveyItemsAsync(int surveyId)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            var items = new List<SurveyItemDto>();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    si.Id, si.SurveyId, si.ItemId,
                    si.Value, si.Memo,
                    grp.Description  AS SectionName,
                    grp.Id           AS SectionId,
                    grp.[Order]      AS SectionOrder,
                    ti.Question,
                    ti.Description,
                    ti.[Order]       AS ItemOrder,
                    ti.Min           AS MinValue,
                    ti.Max           AS MaxValue,
                    ti.is_NA         AS AllowNA
                FROM Ls_surveyItem si
                LEFT JOIN Ls_templateItem      ti  ON ti.Id  = si.ItemId
                LEFT JOIN Ls_templateItemGroup grp ON grp.Id = ti.GroupId
                WHERE si.SurveyId = @surveyId
                ORDER BY grp.[Order], ti.[Order]";

            AddParam(cmd, "@surveyId", surveyId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new SurveyItemDto
                {
                    Id             = reader.GetInt32(0),
                    SurveyId       = reader.GetInt32(1),
                    TemplateItemId = reader.GetInt32(2),
                    Value          = reader.IsDBNull(3)  ? 0f : Convert.ToSingle(reader.GetValue(3)),
                    Memo           = reader.IsDBNull(4)  ? null : reader.GetString(4),
                    SectionName    = reader.IsDBNull(5)  ? null : reader.GetString(5),
                    SectionId      = reader.IsDBNull(6)  ? 0   : reader.GetInt32(6),
                    SectionOrder   = reader.IsDBNull(7)  ? 0   : reader.GetInt32(7),
                    Question       = reader.IsDBNull(8)  ? null : reader.GetString(8),
                    Description    = reader.IsDBNull(9)  ? null : reader.GetString(9),
                    ItemOrder      = reader.IsDBNull(10) ? 0   : reader.GetInt32(10),
                    MinValue       = reader.IsDBNull(11) ? 0f : Convert.ToSingle(reader.GetValue(11)),
                    MaxValue       = reader.IsDBNull(12) ? 0f : Convert.ToSingle(reader.GetValue(12)),
                    AllowNA        = !reader.IsDBNull(13) && Convert.ToInt32(reader.GetValue(13)) == 1
                });
            }

            return items;
        }

        // ─────────────────────────────────────────────────────
        // 4. Mise à jour d'une survey
        // ─────────────────────────────────────────────────────
        public async Task<LsSurveyDto> UpdateSurveyAsync(
            int surveyId, int auditorId, UpdateSurveyDto dto)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            int lsId = 0;
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT LsId FROM Ls_survey WHERE Id = @id";
                AddParam(cmd, "@id", surveyId);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null) throw new KeyNotFoundException($"Survey {surveyId} introuvable.");
                lsId = Convert.ToInt32(result);
            }

            bool periodValid = await IsLsPeriodValidAsync(lsId);
            if (!periodValid)
                throw new InvalidOperationException("La période d'évaluation n'est plus valide.");

            var date = DateTime.Now;

            foreach (var itemDto in dto.Items)
            {
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE Ls_surveyItem
                    SET Value = @val, Memo = @memo, UpdateTime = @dt, UpdateBy = @by
                    WHERE Id = @id AND SurveyId = @sid";

                AddParam(cmd, "@val",  itemDto.Value);
                AddParam(cmd, "@memo", (object)(itemDto.Memo ?? ""));
                AddParam(cmd, "@dt",   date);
                AddParam(cmd, "@by",   auditorId);
                AddParam(cmd, "@id",   itemDto.Id);
                AddParam(cmd, "@sid",  surveyId);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = conn.CreateCommand())
            {
                string catSql = dto.CategoryId.HasValue   ? ", Id_Categories = @cat" : "";
                string crSql  = dto.CallReasonId.HasValue ? ", Id_CallReason = @cr"  : "";

                cmd.CommandText = $@"
                    UPDATE Ls_survey
                    SET Memo = @memo, MemoActionTaken = @mat,
                        UpdateBy = @by, UpdateDate = @dt
                        {catSql} {crSql}
                    WHERE Id = @id";

                AddParam(cmd, "@memo", (object?)dto.Memo            ?? DBNull.Value);
                AddParam(cmd, "@mat",  (object?)dto.MemoActionTaken ?? DBNull.Value);
                AddParam(cmd, "@by",   auditorId);
                AddParam(cmd, "@dt",   date);
                AddParam(cmd, "@id",   surveyId);

                if (dto.CategoryId.HasValue)
                    AddParam(cmd, "@cat", dto.CategoryId.Value);
                if (dto.CallReasonId.HasValue)
                    AddParam(cmd, "@cr",  dto.CallReasonId.Value);

                await cmd.ExecuteNonQueryAsync();
            }

            int site = await GetSiteAsync(lsId);
            double newScore = site == 4
                ? await GetScalarAsync("SELECT [dbo].[Fn_Ls_getSurveyScoreAADC](@p0)", surveyId)
                : await GetScalarAsync("SELECT [dbo].[Fn_Ls_getSurveyScore](@p0)",     surveyId);

            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE Ls_survey SET Score = @score WHERE Id = @id";
                AddParam(cmd, "@score", newScore);
                AddParam(cmd, "@id",    surveyId);
                await cmd.ExecuteNonQueryAsync();
            }

            var groupIds = new List<int>();
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT DISTINCT ti.GroupId
                    FROM Ls_surveyItem si
                    JOIN Ls_templateItem ti ON ti.Id = si.ItemId
                    WHERE si.SurveyId = @sid AND ti.GroupId IS NOT NULL";
                AddParam(cmd, "@sid", surveyId);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    groupIds.Add(reader.GetInt32(0));
            }

            foreach (var groupId in groupIds)
            {
                double gs = await GetScalarAsync(
                    "SELECT [dbo].[Fn_Ls_getSurveyGroupScore](@p0, @p1)",
                    surveyId, groupId);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    IF EXISTS (SELECT 1 FROM Ls_ScoreSection
                               WHERE Id_Ls_survey = @sid AND Id_Ls_templateItemGroup = @gid)
                        UPDATE Ls_ScoreSection SET ScoreGroup = @score
                        WHERE Id_Ls_survey = @sid AND Id_Ls_templateItemGroup = @gid
                    ELSE
                        INSERT INTO Ls_ScoreSection (Id_Ls_survey, Id_Ls_templateItemGroup, ScoreGroup)
                        VALUES (@sid, @gid, @score)";

                AddParam(cmd, "@sid",   surveyId);
                AddParam(cmd, "@gid",   groupId);
                AddParam(cmd, "@score", (float)gs);
                await cmd.ExecuteNonQueryAsync();
            }

            double lsScore = site == 4
                ? await GetScalarAsync("SELECT [dbo].[Fn_Ls_getLsScoreAADC](@p0)", lsId)
                : await GetScalarAsync("SELECT [dbo].[Fn_Ls_getLsScore](@p0, 0)",  lsId);

            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE Ls SET Score = @score, UpdateBy = @by, UpdateDate = @dt WHERE Id = @id";
                AddParam(cmd, "@score", (float)lsScore);
                AddParam(cmd, "@by",    auditorId);
                AddParam(cmd, "@dt",    date);
                AddParam(cmd, "@id",    lsId);
                await cmd.ExecuteNonQueryAsync();
            }

            LsSurveyDto updated = new();
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT s.Id, s.LsId, s.CreateDate, s.UpdateDate,
                           s.Is_saved, s.Memo, s.MemoActionTaken, s.RecordDataId,
                           cat.Des_Categories, cr.Des_CallReason,
                           rd.Record_Date
                    FROM Ls_survey s
                    LEFT JOIN Ls_categories cat ON cat.Id = s.Id_Categories
                    LEFT JOIN Ls_CallReason cr  ON cr.Id  = s.Id_CallReason
                    LEFT JOIN RecordData    rd  ON rd.ID  = s.RecordDataId
                    WHERE s.Id = @id";
                AddParam(cmd, "@id", surveyId);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    updated = new LsSurveyDto
                    {
                        Id              = reader.GetInt32(0),
                        LsId            = reader.GetInt32(1),
                        CreateDate      = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        UpdateDate      = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                        Score           = Math.Round(newScore, 2),
                        IsSaved         = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        Memo            = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        MemoActionTaken = reader.IsDBNull(6) ? "" : reader.GetString(6),
                        RecordDataId    = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                        CategoryName    = reader.IsDBNull(8) ? "" : reader.GetValue(8).ToString()!,
                        CallReasonName  = reader.IsDBNull(9) ? "" : reader.GetValue(9).ToString()!,
                        RecordDate      = reader.IsDBNull(10) ? ""
                                        : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    };
                }
            }

            return updated;
        }

        // ─────────────────────────────────────────────────────
        // 5. Suppression d'une survey individuelle
        // ─────────────────────────────────────────────────────
        public async Task DeleteSurveyAsync(int surveyId)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var tx = await conn.BeginTransactionAsync();
            try
            {
                int? recordDataId = null;
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "SELECT RecordDataId FROM Ls_survey WHERE Id = @id";
                    AddParam(cmd, "@id", surveyId);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                        throw new KeyNotFoundException($"Survey {surveyId} introuvable.");
                    if (result != DBNull.Value)
                        recordDataId = Convert.ToInt32(result);
                }

                if (recordDataId.HasValue)
                {
                    await using var cmd = conn.CreateCommand();
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "UPDATE RecordData SET LsId = NULL WHERE ID = @id";
                    AddParam(cmd, "@id", recordDataId.Value);
                    await cmd.ExecuteNonQueryAsync();
                }

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "DELETE FROM Ls_surveyItem WHERE SurveyId = @id";
                    AddParam(cmd, "@id", surveyId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "DELETE FROM Ls_ScoreSection WHERE Id_Ls_survey = @id";
                    AddParam(cmd, "@id", surveyId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "DELETE FROM Ls_survey WHERE Id = @id";
                    AddParam(cmd, "@id", surveyId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ─────────────────────────────────────────────────────
        // 6. Suppression d'une fiche complète (cascade)
        // ─────────────────────────────────────────────────────
        public async Task DeleteLsFicheAsync(int lsId)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var tx = await conn.BeginTransactionAsync();
            try
            {
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "SELECT COUNT(*) FROM Ls WHERE Id = @id";
                    AddParam(cmd, "@id", lsId);
                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    if (count == 0)
                        throw new KeyNotFoundException($"Fiche {lsId} introuvable.");
                }

                var surveyIds     = new List<int>();
                var recordDataIds = new List<int>();

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "SELECT Id, RecordDataId FROM Ls_survey WHERE LsId = @id";
                    AddParam(cmd, "@id", lsId);
                    await using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        surveyIds.Add(reader.GetInt32(0));
                        if (!reader.IsDBNull(1))
                            recordDataIds.Add(reader.GetInt32(1));
                    }
                }

                foreach (var recId in recordDataIds)
                {
                    await using var cmd = conn.CreateCommand();
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "UPDATE RecordData SET LsId = NULL WHERE ID = @rid";
                    AddParam(cmd, "@rid", recId);
                    await cmd.ExecuteNonQueryAsync();
                }

                if (surveyIds.Any())
                {
                    var sIds = string.Join(",", surveyIds);

                    await using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                        cmd.CommandText = $"DELETE FROM Ls_surveyItem WHERE SurveyId IN ({sIds})";
                        await cmd.ExecuteNonQueryAsync();
                    }

                    await using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                        cmd.CommandText = $"DELETE FROM Ls_ScoreSection WHERE Id_Ls_survey IN ({sIds})";
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "DELETE FROM Ls_survey WHERE LsId = @id";
                    AddParam(cmd, "@id", lsId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = (System.Data.Common.DbTransaction)tx;
                    cmd.CommandText = "DELETE FROM Ls WHERE Id = @id";
                    AddParam(cmd, "@id", lsId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ─────────────────────────────────────────────────────
        // 7. Rapport complet d'une fiche agent
        // ─────────────────────────────────────────────────────
        public async Task<AgentReportDto> GetAgentReportAsync(int lsId, int recordDataId = 0)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            string agentName = "", auditorName = "", createDate = "", periodLabel = "";
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT l.Agent,
                           ISNULL(u.FirstName + ' ' + u.LastName, l.userName) AS AuditorName,
                           l.CreateDate, l.StartPeriode, l.EndPeriode
                    FROM Ls l
                    LEFT JOIN SQR_Admin.dbo.Users u ON u.ID = l.Auditor
                    WHERE l.Id = @id";
                AddParam(cmd, "@id", lsId);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    agentName   = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    auditorName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    createDate  = reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToString("dd/MM/yyyy");
                    string start = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("dd/MM/yyyy");
                    string end   = reader.IsDBNull(4) ? "" : reader.GetDateTime(4).ToString("dd/MM/yyyy");
                    periodLabel = $"Du {start} au {end}";
                }
                else throw new KeyNotFoundException($"Fiche {lsId} introuvable.");
            }

            var surveyIds    = new List<int>();
            var surveyScores = new Dictionary<int, double>();
            var surveyMemos  = new Dictionary<int, string>();

            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT Id, Score, Memo
                    FROM Ls_survey
                    WHERE LsId         = @id
                      AND RecordDataId IS NOT NULL
                      AND Is_saved     = 1"
                      + (recordDataId > 0 ? " AND RecordDataId = @rid" : "") + @"
                    ORDER BY CreateDate ASC";

                AddParam(cmd, "@id", lsId);
                if (recordDataId > 0)
                    AddParam(cmd, "@rid", recordDataId);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int sid = reader.GetInt32(0);
                    surveyIds.Add(sid);
                    surveyScores[sid] = reader.IsDBNull(1) ? 0 : Math.Round((double)reader.GetFloat(1), 2);
                    surveyMemos[sid]  = reader.IsDBNull(2) ? "" : reader.GetString(2);
                }
            }

            if (!surveyIds.Any())
                throw new InvalidOperationException("Aucune évaluation trouvée pour cette fiche.");

            double totalScore = Math.Round(
                await GetScalarAsync("SELECT [dbo].[Fn_Ls_getLsScore](@p0, @p1)", lsId, recordDataId),
                2);

            var surveyReports = new List<SurveyReportDto>();
            var sectionScores = new List<SectionScoreDto>();
            int idx = 1;

            foreach (var surveyId in surveyIds)
            {
                string label = $"E{idx++}";
                var items = await GetSurveyItemsAsync(surveyId);

                surveyReports.Add(new SurveyReportDto
                {
                    SurveyId    = surveyId,
                    SurveyLabel = label,
                    Memo        = surveyMemos.GetValueOrDefault(surveyId, ""),
                    Score       = surveyScores.GetValueOrDefault(surveyId, 0),
                    Items       = items
                });

                var sections = items
                    .GroupBy(i => i.SectionId)
                    .Select(g => g.First())
                    .ToList();

                foreach (var section in sections)
                {
                    double gs = Math.Round(
                        await GetScalarAsync(
                            "SELECT [dbo].[Fn_Ls_getSurveyGroupScore](@p0, @p1)",
                            surveyId, section.SectionId),
                        2);

                    sectionScores.Add(new SectionScoreDto
                    {
                        SectionName = section.SectionName ?? "",
                        Score       = gs,
                        SurveyLabel = label
                    });
                }
            }

            return new AgentReportDto
            {
                CreateDate    = createDate,
                AuditorName   = auditorName,
                AuditorLogin  = string.Empty,
                AgentName     = agentName,
                PeriodLabel   = periodLabel,
                TotalScore    = totalScore,
                Surveys       = surveyReports,
                SectionScores = sectionScores
            };
        }

        // ─────────────────────────────────────────────────────
        // 8. Vérification de la validité de la période
        // ─────────────────────────────────────────────────────
        public async Task<bool> IsLsPeriodValidAsync(int lsId)
        {
            var result = await GetScalarAsync(
                "SELECT [dbo].[Fn_Ls_isValidLsPeriode](@p0)", lsId);
            return (int)result == 1;
        }

        // ─────────────────────────────────────────────────────
        // HELPER : Exécute une fonction scalaire SQL
        // ─────────────────────────────────────────────────────
        private async Task<double> GetScalarAsync(string sql, params object[] parameters)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();

            for (int i = 0; i < parameters.Length; i++)
            {
                sql = sql.Replace($"{{{i}}}", $"@p{i}");
                var p = cmd.CreateParameter();
                p.ParameterName = $"@p{i}";
                p.Value = parameters[i] ?? DBNull.Value;
                cmd.Parameters.Add(p);
            }

            cmd.CommandText = sql;
            var result = await cmd.ExecuteScalarAsync();
            return (result == null || result == DBNull.Value)
                ? 0.0
                : Convert.ToDouble(result);
        }

        // ─────────────────────────────────────────────────────
        // HELPER : Récupère le site d'une fiche
        // ─────────────────────────────────────────────────────
        private async Task<int> GetSiteAsync(int lsId)
        {
            var conn = _db.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT c.Site
                FROM Ls l
                JOIN Ls_CalledCampaign c ON c.Id = l.CalledCampaignId
                WHERE l.Id = @id";
            AddParam(cmd, "@id", lsId);

            var result = await cmd.ExecuteScalarAsync();
            return (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);
        }

        // ─────────────────────────────────────────────────────
        // HELPER : Ajouter un paramètre
        // ─────────────────────────────────────────────────────
        private static void AddParam(System.Data.Common.DbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
        public async Task<AgentReportDto> GetAgentReportByRecordDataIdAsync(int recordDataId)
{
    var conn = _db.Database.GetDbConnection();
    if (conn.State != ConnectionState.Open)
        await conn.OpenAsync();

    int lsId = 0;
    await using (var cmd = conn.CreateCommand())
    {
        cmd.CommandText = @"
            SELECT TOP 1 LsId
            FROM Ls_survey
            WHERE RecordDataId = @rid
              AND Is_saved = 1
            ORDER BY CreateDate DESC";
        AddParam(cmd, "@rid", recordDataId);
        var result = await cmd.ExecuteScalarAsync();
        if (result == null || result == DBNull.Value)
            throw new KeyNotFoundException(
                $"Aucune fiche trouvée pour recordDataId {recordDataId}.");
        lsId = Convert.ToInt32(result);
    }

    return await GetAgentReportAsync(lsId, recordDataId);
}
    }
}