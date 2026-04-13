
// ============================================================
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Evaluation;
using System.Net.Mail;
using System.Text;
using scoring_Backend.Models.Admin;

namespace scoring_Backend.Repositories.Implementations.Evaluation
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly SqrScoringContext             _db;
        private readonly IConfiguration                _cfg;
        private readonly ILogger<EvaluationRepository> _log;

        private readonly SqrAdminContext   _adminDb;

        public EvaluationRepository(
            SqrScoringContext db,
            IConfiguration cfg,
            ILogger<EvaluationRepository> log,
            SqrAdminContext adminDb)
        {
            _db  = db;
            _cfg = cfg;
            _log = log;
                        _adminDb = adminDb;

        }

        // ══════════════════════════════════════════════════════
        // HELPER — exécute une fonction SQL scalaire (double)
        // ══════════════════════════════════════════════════════
        private async Task<double> ExecuteScalarFunctionAsync(string sql)
        {
            var conn    = _db.Database.GetDbConnection();
            var wasOpen = conn.State == System.Data.ConnectionState.Open;
            try
            {
                if (!wasOpen) await conn.OpenAsync();
                using var cmd   = conn.CreateCommand();
                cmd.CommandText = sql;
                var result      = await cmd.ExecuteScalarAsync();
                return result == null || result == DBNull.Value
                    ? 0d
                    : Convert.ToDouble(result);
            }
            finally
            {
                if (!wasOpen) await conn.CloseAsync();
            }
        }

        // ══════════════════════════════════════════════════════
        // HELPER — exécute une SP avec paramètres nommés
        // ══════════════════════════════════════════════════════
        private async Task<int> ExecuteSpWithNamedParamsAsync(
            string spName, params (string Name, object? Value)[] parameters)
        {
            var conn    = _db.Database.GetDbConnection();
            var wasOpen = conn.State == System.Data.ConnectionState.Open;
            try
            {
                if (!wasOpen) await conn.OpenAsync();
                using var cmd   = conn.CreateCommand();
                cmd.CommandText = spName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (var (name, value) in parameters)
                {
                    var p           = cmd.CreateParameter();
                    p.ParameterName = name;
                    p.Value         = value ?? DBNull.Value;
                    cmd.Parameters.Add(p);
                }

                _log.LogDebug("SP {SpName} params: {Params}",
                    spName,
                    string.Join(", ", parameters.Select(p => $"{p.Name}={p.Value}")));

                var result = await cmd.ExecuteScalarAsync();

                _log.LogDebug("SP {SpName} → result: {Result}",
                    spName, result?.ToString() ?? "NULL");

                return result == null || result == DBNull.Value
                    ? 0
                    : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "SP {SpName} FAILED", spName);
                throw;
            }
            finally
            {
                if (!wasOpen) await conn.CloseAsync();
            }
        }

        // ══════════════════════════════════════════════════════
        // HELPER — SP avec paramètre RETURN VALUE
        // ══════════════════════════════════════════════════════
        private async Task<int> ExecuteSpReturnValueAsync(
            string spName, params (string Name, object? Value)[] parameters)
        {
            var conn    = _db.Database.GetDbConnection();
            var wasOpen = conn.State == System.Data.ConnectionState.Open;
            try
            {
                if (!wasOpen) await conn.OpenAsync();
                using var cmd   = conn.CreateCommand();
                cmd.CommandText = spName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var returnParam           = cmd.CreateParameter();
                returnParam.ParameterName = "@ReturnValue";
                returnParam.DbType        = System.Data.DbType.Int32;
                returnParam.Direction     = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParam);

                foreach (var (name, value) in parameters)
                {
                    var p           = cmd.CreateParameter();
                    p.ParameterName = name;
                    p.Value         = value ?? DBNull.Value;
                    cmd.Parameters.Add(p);
                }

                await cmd.ExecuteNonQueryAsync();

                var result = returnParam.Value;
                return result == null || result == DBNull.Value
                    ? 0
                    : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "SP {SpName} FAILED", spName);
                throw;
            }
            finally
            {
                if (!wasOpen) await conn.CloseAsync();
            }
        }

        // ══════════════════════════════════════════════════════
        // OPEN EVALUATION
        // ══════════════════════════════════════════════════════
        public async Task<OpenEvaluationResponseDto> OpenEvaluationAsync(
            int recordId, int userId, string userLogin)
        {
            var resp = new OpenEvaluationResponseDto();

            var record = await _db.RecordData
                .FirstOrDefaultAsync(r => r.Id == recordId);

            if (record == null)
            {
                resp.ErrorMessage = "Enregistrement introuvable.";
                return resp;
            }

            // Bloquer uniquement les doublons en cours de saisie (non sauvegardés)
            var inProgress = await _db.LsSurveys
                .AnyAsync(s => s.RecordDataId == recordId && s.IsSaved == 0);

            if (inProgress)
            {
                resp.ErrorMessage = "Une évaluation est déjà en cours pour cet enregistrement.";
                return resp;
            }

            var recDate = record.RecordDate.HasValue
                ? record.RecordDate.Value.Date
                : DateTime.Today;

            _log.LogDebug("OpenEval → recordId={RecordId} campaignId={CampaignId} recDate={RecDate}",
                recordId, record.CampaignId, recDate);

            var campaign = await _db.LsCalledCampaigns
                .Include(c => c.IdLsTemplateNavigation)
                .FirstOrDefaultAsync(c =>
                    c.CampagneDid == record.CampaignId.ToString() &&
                    c.IdLsTemplateNavigation != null &&
                    c.IdLsTemplateNavigation.StartDate.HasValue &&
                    c.IdLsTemplateNavigation.EndDate.HasValue &&
                    c.IdLsTemplateNavigation.StartDate!.Value.Date <= recDate &&
                    c.IdLsTemplateNavigation.EndDate!.Value.Date   >= recDate &&
                    c.Status == 1);

            _log.LogDebug("OpenEval → campaign={CampaignId}", campaign?.Id.ToString() ?? "NULL");

            if (campaign == null)
            {
                resp.ErrorMessage = "Aucune campagne qualité active pour cet enregistrement.";
                return resp;
            }

            var template = campaign.IdLsTemplateNavigation;
            if (template == null)
            {
                resp.ErrorMessage = "Le modèle d'évaluation est introuvable.";
                return resp;
            }

            var today = DateTime.Today;

            var ls = await _db.Ls
                .FirstOrDefaultAsync(l =>
                    l.AgentOid == record.AgentOid &&
                    l.Auditor  == userId &&
                    l.StartPeriode.HasValue && l.StartPeriode.Value.Date <= today &&
                    l.EndPeriode.HasValue   && l.EndPeriode.Value.Date   >= today);

            _log.LogDebug("OpenEval → ls={LsId}", ls?.Id.ToString() ?? "NULL");

            int surveyId;

            if (ls == null)
            {
                _log.LogDebug("OpenEval → appel SP_Ls_CreateLs agentOid={AgentOid} campaignId={CampaignId} recordId={RecordId}",
                    record.AgentOid, campaign.Id, record.Id);

                surveyId = await ExecuteSpReturnValueAsync(
                    "SP_Ls_CreateLs",
                    ("@vAgentOid",         (object)(record.AgentOid ?? "")),
                    ("@vAuditor",          userId),
                    ("@vMemo",             ""),
                    ("@vCalledCampaignId", campaign.Id),
                    ("@vCreateBy",         userId),
                    ("@vAgentId",          (object?)record.LastAgent ?? DBNull.Value),
                    ("@vAgent",            $"{record.PrenomAgent} {record.NomAgent}".Trim()),
                    ("@vRecordDataId",     record.Id),
                    ("@vMemoActionTaken",  ""),
                    ("@vId_Categories",    0),
                    ("@vId_CallReason",    0));

                _log.LogDebug("OpenEval → SP_Ls_CreateLs surveyId={SurveyId}", surveyId);

                if (surveyId == 0)
                {
                    _db.ChangeTracker.Clear();
                    surveyId = await _db.LsSurveys
                        .Where(s =>
                            s.Ls!.AgentOid == record.AgentOid &&
                            s.Ls.Auditor   == userId &&
                            s.IsSaved      == 0)
                        .OrderByDescending(s => s.Id)
                        .Select(s => s.Id)
                        .FirstOrDefaultAsync();

                    _log.LogDebug("OpenEval → fallback EF (CreateLs) surveyId={SurveyId}", surveyId);
                }
            }
            else
            {
                var savedCount = await _db.LsSurveys
                    .CountAsync(s => s.LsId == ls.Id && s.IsSaved == 1);

                _log.LogDebug("OpenEval → savedCount={SavedCount} max={Max}", savedCount, template.Max);

                if (savedCount >= template.Max)
                {
                    resp.ErrorMessage = "Le quota d'évaluations pour cet agent est atteint.";
                    return resp;
                }

                _log.LogDebug("OpenEval → appel SP_Ls_CreateSurvey lsId={LsId} recordId={RecordId}",
                    ls.Id, record.Id);

                surveyId = await ExecuteSpReturnValueAsync(
                    "SP_Ls_CreateSurvey",
                    ("@vLsId",         ls.Id),
                    ("@vRecordDataId", record.Id),
                    ("@vCreateBy",     userId));

                _log.LogDebug("OpenEval → SP_Ls_CreateSurvey surveyId={SurveyId}", surveyId);

                if (surveyId == 0)
                {
                    _db.ChangeTracker.Clear();
                    surveyId = await _db.LsSurveys
                        .Where(s => s.LsId == ls.Id && s.IsSaved == 0)
                        .OrderByDescending(s => s.Id)
                        .Select(s => s.Id)
                        .FirstOrDefaultAsync();

                    _log.LogDebug("OpenEval → fallback EF (CreateSurvey) surveyId={SurveyId}", surveyId);
                }
            }

            if (surveyId == 0)
            {
                resp.ErrorMessage = "Impossible de créer ou de retrouver l'évaluation.";
                return resp;
            }

            var surveyItems = await _db.LsSurveyItems
                .Where(i => i.SurveyId == surveyId)
                .ToListAsync();

            var templateItems = await _db.LsSurveyItems
                .Where(i => i.SurveyId == surveyId)
                .Join(_db.LsTemplateItems.Include(t => t.Group),
                      si => si.ItemId,
                      ti => ti.Id,
                      (si, ti) => ti)
                .Distinct()
                .ToListAsync();

            var templateItemDict = templateItems.ToDictionary(t => t.Id);

            var gridRows = surveyItems
                .Where(i => templateItemDict.ContainsKey(i.ItemId))
                .OrderBy(i => templateItemDict[i.ItemId].Group?.Order ?? 0)
                .ThenBy(i => templateItemDict[i.ItemId].Order)
                .Select(i =>
                {
                    var ti    = templateItemDict[i.ItemId];
                    var group = ti.Group;
                    return new EvalGridRowDto
                    {
                        Id             = i.Id,
                        SurveyId       = surveyId,
                        TemplateItemId = i.ItemId,   // ← templateItemId envoyé au frontend
                        GroupId        = group?.Id          ?? 0,
                        GroupName      = group?.Description ?? "",
                        GroupOrder     = group?.Order       ?? 0,
                        Question       = ti.Question        ?? "",
                        Definition     = ti.Description     ?? "",
                        Value          = i.Value            ?? 0f,
                        Memo           = i.Memo             ?? "",
                        ScaleMax       = ti.Max,
                        ScaleMin       = ti.Min,
                        IsNA           = ti.IsNa == 1,
                        ItemOrder      = ti.Order
                    };
                })
                .ToList();

            resp.SurveyId    = surveyId;
            resp.RecordDate  = record.RecordDate.HasValue
                ? record.RecordDate.Value.ToString("MM/dd/yyyy")
                : "";
            resp.EvalDate    = DateTime.Now.ToString("MM/dd/yyyy");
            resp.CallIndex   = record.RecIdLink?.ToString() ?? "";
            resp.GridRows    = gridRows;
         
            resp.Categories  = await GetCategoriesAsync();
            resp.CallReasons = await GetCallReasonsAsync();
var auditorUser = await _adminDb.Users
    .FirstOrDefaultAsync(u => u.Id == userId);

resp.Auditor = auditorUser != null
    ? $"{auditorUser.FirstName} {auditorUser.LastName}".Trim()
    : "";

            return resp;
        }
        // ══════════════════════════════════════════════════════
        // SAVE EVALUATION
        // ══════════════════════════════════════════════════════
        public async Task<SaveEvaluationResultDto> SaveEvaluationAsync(
    SaveEvaluationDto dto, int userId, string userLogin)
{
    // ══════════════════════════════════════════════════════
    // 1. Charger le survey
    // ══════════════════════════════════════════════════════
    var survey = await _db.LsSurveys
        .Include(s => s.Ls)
        .FirstOrDefaultAsync(s => s.Id == dto.SurveyId);

    if (survey == null)
    {
        _log.LogWarning("SaveEval → ❌ Survey {SurveyId} introuvable", dto.SurveyId);
        return new SaveEvaluationResultDto
        {
            Success = false,
            Message = "Évaluation introuvable."
        };
    }

    _log.LogInformation("SaveEval → Survey {SurveyId} trouvé, LsId={LsId}, IsSaved={IsSaved}",
        dto.SurveyId, survey.LsId, survey.IsSaved);

    var now = DateTime.Now;

    // ══════════════════════════════════════════════════════
    // 2. Charger les items en base
    // ══════════════════════════════════════════════════════
    var surveyItemsToUpdate = await _db.LsSurveyItems
        .Where(i => i.SurveyId == dto.SurveyId)
        .ToListAsync();

    _log.LogDebug("SaveEval → ItemIds EN BASE    : [{Ids}]",
        string.Join(", ", surveyItemsToUpdate.Select(i => i.ItemId)));

    _log.LogDebug("SaveEval → ItemIds REÇUS (DTO): [{Ids}]",
        string.Join(", ", dto.Items.Select(i => i.ItemId)));

    _log.LogInformation("SaveEval → Nb items en base={CountDb}, Nb items reçus={CountDto}",
        surveyItemsToUpdate.Count, dto.Items.Count);

    // ══════════════════════════════════════════════════════
    // 3. Mettre à jour les items
    // ══════════════════════════════════════════════════════
    int matched = 0, notFound = 0;

    foreach (var itemVal in dto.Items)
    {
        var item = surveyItemsToUpdate
            .FirstOrDefault(i => i.ItemId == itemVal.ItemId);

        if (item == null)
        {
            _log.LogWarning(
                "SaveEval → ❌ templateItemId={ItemId} INTROUVABLE dans survey {SurveyId}",
                itemVal.ItemId, dto.SurveyId);
            notFound++;
            continue;
        }

        _log.LogDebug(
            "SaveEval → ✅ itemId={ItemId} oldValue={OldValue} → newValue={NewValue}",
            item.ItemId, item.Value, itemVal.Value);

        item.Value      = itemVal.Value;
        item.Memo       = itemVal.Memo;
        item.UpdateTime = now;
        item.UpdateBy   = userId;
        matched++;
    }

    _log.LogInformation(
        "SaveEval → Matching terminé : {Matched} mis à jour, {NotFound} non trouvés sur {Total}",
        matched, notFound, dto.Items.Count);

    // ══════════════════════════════════════════════════════
    // 4. Mettre à jour le survey
    // ══════════════════════════════════════════════════════
    survey.Memo            = dto.Memo;
    survey.MemoActionTaken = dto.MemoAction;
    survey.IsSaved         = 1;
    survey.UpdateBy        = userId;
    survey.UpdateDate      = now;

    if (dto.CategoryId.HasValue)
    {
        survey.IdCategories = dto.CategoryId.Value;
        _log.LogDebug("SaveEval → CategoryId={CategoryId}", dto.CategoryId.Value);
    }

    if (dto.CallReasonId.HasValue)
    {
        survey.IdCallReason = dto.CallReasonId.Value;
        _log.LogDebug("SaveEval → CallReasonId={CallReasonId}", dto.CallReasonId.Value);
    }

    var record = await _db.RecordData
        .FirstOrDefaultAsync(r => r.Id == survey.RecordDataId);

    _log.LogDebug("SaveEval → RecordDataId={RecordDataId} record={RecordFound}",
        survey.RecordDataId, record != null ? "trouvé" : "NULL");

    // ══════════════════════════════════════════════════════
    // 5. Premier SaveChanges (items + survey)
    // ══════════════════════════════════════════════════════
    _log.LogDebug("SaveEval → Avant premier SaveChanges...");

    await _db.SaveChangesAsync();

    _log.LogInformation("SaveEval → ✅ Premier SaveChanges OK — surveyId={SurveyId}", dto.SurveyId);

    // ══════════════════════════════════════════════════════
    // 6. Résoudre le siteId
    // ══════════════════════════════════════════════════════
    int siteId = 0;

if (survey.Ls?.CalledCampaignId != null)
{
    var calledCampaign = await _db.LsCalledCampaigns
        .Include(c => c.IdLsTemplateNavigation)
        .FirstOrDefaultAsync(c => c.Id == survey.Ls.CalledCampaignId);

    siteId = calledCampaign?.IdLsTemplateNavigation?.Site ?? 0;

    _log.LogInformation("SaveEval → calledCampaignId={CampaignId} siteId={SiteId}",
        survey.Ls.CalledCampaignId, siteId);
}
else
{
// ✅ APRÈS
_log.LogWarning("SaveEval → ⚠️ Ls={LsNull} CalledCampaignId={CampId} → siteId=0",
    survey.Ls == null ? "NULL" : "OK",
    survey.Ls?.CalledCampaignId.ToString() ?? "NULL");  // ← un seul ?
}

    // ══════════════════════════════════════════════════════
    // 7. Calcul des scores
    // ══════════════════════════════════════════════════════
    double score   = 0;
double lsScore = 0;

try
{
    string surveyScoreSql, lsScoreSql;

    if (siteId == 4)
{
    surveyScoreSql = $"SELECT [dbo].[Fn_Ls_getSurveyScoreAADC]({dto.SurveyId})";
    lsScoreSql     = $"SELECT [dbo].[Fn_Ls_getLsScoreAADC]({survey.LsId})";
}
else
{
    surveyScoreSql = $"SELECT [dbo].[Fn_Ls_getSurveyScore]({dto.SurveyId})";
    lsScoreSql     = $"SELECT [dbo].[Fn_Ls_getLsScore]({survey.LsId}, 0)"; // ✅ ajout , 0
}

    _log.LogInformation("SaveEval → Executing: {Sql}", surveyScoreSql);

    score   = await ExecuteScalarFunctionAsync(surveyScoreSql);
    lsScore = await ExecuteScalarFunctionAsync(lsScoreSql);

    _log.LogInformation(
        "SaveEval → score={Score} lsScore={LsScore} siteId={SiteId}",
        score, lsScore, siteId);

    // ✅ Alerte si score inattendu à 0
    if (score == 0)
        _log.LogWarning("SaveEval → ⚠️ score=0 pour surveyId={SurveyId} — vérifier Fn_Ls_getSurveyScore", dto.SurveyId);
}
catch (Exception ex)
{
    _log.LogError(ex, "SaveEval → ❌ Calcul score ÉCHOUÉ surveyId={SurveyId}", dto.SurveyId);
}
    // ══════════════════════════════════════════════════════
    // 8. Persister les scores
    // ══════════════════════════════════════════════════════
    survey.Score = (float)score;

    if (survey.Ls != null)
    {
        survey.Ls.Score      = (float)lsScore;
        survey.Ls.UpdateBy   = userId;
        survey.Ls.UpdateDate = now;

        _log.LogDebug("SaveEval → Ls.Score={LsScore} mis à jour sur LsId={LsId}",
            lsScore, survey.LsId);
    }
    else
    {
        _log.LogWarning("SaveEval → ⚠️ survey.Ls est NULL — LsScore non persisté");
    }

    // ══════════════════════════════════════════════════════
    // 9. Scores par groupe / section
    // ══════════════════════════════════════════════════════
    var allSurveyItems = await _db.LsSurveyItems
        .Where(i => i.SurveyId == dto.SurveyId)
        .ToListAsync();

    var templateItems = await _db.LsSurveyItems
        .Where(i => i.SurveyId == dto.SurveyId)
        .Join(_db.LsTemplateItems.Include(t => t.Group),
              si => si.ItemId,
              ti => ti.Id,
              (si, ti) => ti)
        .Distinct()
        .ToListAsync();

    var groups = templateItems
        .Select(t => t.Group)
        .Where(g => g != null)
        .DistinctBy(g => g!.Id)
        .ToList();

    _log.LogDebug("SaveEval → {GroupCount} groupes à scorer", groups.Count);

    foreach (var grp in groups)
    {
        try
        {
            string groupScoreSql = $"SELECT [dbo].[Fn_Ls_getSurveyGroupScore]({dto.SurveyId},{grp!.Id})";

            _log.LogDebug("SaveEval → SQL group score : {Sql}", groupScoreSql);

            double groupScore = await ExecuteScalarFunctionAsync(groupScoreSql);

            _log.LogDebug("SaveEval → GroupId={GroupId} score={GroupScore}%",
                grp.Id, groupScore);

            _db.LsScoreSections.Add(new LsScoreSection
            {
                ScoreGroup            = (float)groupScore,
                IdLsSurvey            = dto.SurveyId,
                IdLsTemplateItemGroup = grp.Id
            });
        }
        catch (Exception ex)
        {
            _log.LogError(ex,
                "SaveEval → ❌ Score section ÉCHOUÉ pour groupId={GroupId} surveyId={SurveyId}",
                grp!.Id, dto.SurveyId);
        }
    }

    // ══════════════════════════════════════════════════════
    // 10. Deuxième SaveChanges (scores)
    // ══════════════════════════════════════════════════════
    _log.LogDebug("SaveEval → Avant deuxième SaveChanges (scores)...");

    await _db.SaveChangesAsync();

    _log.LogInformation("SaveEval → ✅ Deuxième SaveChanges OK — scores persistés");

    // ══════════════════════════════════════════════════════
    // 11. Envoi email
    // ══════════════════════════════════════════════════════
    if (record != null)
    {
        _log.LogDebug("SaveEval → Tentative envoi email pour recordId={RecordId}", record.Id);
        await TrySendEmailAsync(survey, allSurveyItems, templateItems, record, dto.CcEmail);
    }
    else
    {
        _log.LogWarning("SaveEval → ⚠️ record NULL — email non envoyé");
    }

    // ══════════════════════════════════════════════════════
    // 12. Retour
    // ══════════════════════════════════════════════════════
    _log.LogInformation(
        "SaveEval → ✅ TERMINÉ surveyId={SurveyId} score={Score}% matched={Matched}/{Total}",
        dto.SurveyId, Math.Round(score, 2), matched, dto.Items.Count);

    return new SaveEvaluationResultDto
    {
        Success = true,
        Score   = Math.Round(score, 2),
        Message = $"Score : {Math.Round(score, 2)}%"
    };
}
public async Task<RecordFileDto?> GetRecordFilePathAsync(int recordId)
{
    var record = await _db.RecordData
        .Where(r => r.Id == recordId)
        .Select(r => new RecordFileDto
        {
            Id       = r.Id,
            FilePath = r.RecFilename   // ← champ physique du fichier
        })
        .FirstOrDefaultAsync();

    return record;
}
        // ══════════════════════════════════════════════════════
        // RÉFÉRENTIELS
        // ══════════════════════════════════════════════════════
        public async Task<List<CategoryDto>> GetCategoriesAsync() =>
            await _db.LsCategories
                .Select(c => new CategoryDto { Id = c.Id, Libelle = c.DesCategories ?? "" })
                .ToListAsync();

        public async Task<List<CallReasonDto>> GetCallReasonsAsync() =>
            await _db.LsCallReasons
                .Select(c => new CallReasonDto { Id = c.Id, Libelle = c.DesCallReason ?? "" })
                .ToListAsync();

       public async Task<List<AgentDto>> GetAgentsAsync(
    int userId,
    string userRole,
    int userSite
)
{
    IQueryable<TListAgent> query = _db.TListAgents;

    // 🔐 Filtrage par site uniquement si pas SuperAdmin
    if (userRole != "SuperAdmin" && userSite > 0)
    {
        query = query.Where(a => a.CustomerId == userSite);
    }

    return await query
        .OrderBy(a => a.Prenom)
        .ThenBy(a => a.Nom)
        .Select(a => new AgentDto
        {
            Oid   = a.Oid ?? "",
            Label = ((a.Prenom ?? "") + " " + (a.Nom ?? "")).Trim()
        })
        .Distinct()
        .ToListAsync();
}

        public async Task<byte[]?> BuildZipAsync(List<int> recordIds)
        {
            var records = await _db.RecordData
                .Where(r => recordIds.Contains(r.Id))
                .ToListAsync();

            if (!records.Any()) return null;

            using var memStream = new MemoryStream();
            using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, true))
            {
                foreach (var record in records)
                {
                    var filePath = record.RecFilename;
                    if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                        continue;

                    var entryName = Path.GetFileName(filePath);
                    var entry     = archive.CreateEntry(entryName, CompressionLevel.Fastest);

                    using var entryStream = entry.Open();
                    using var fileStream  = System.IO.File.OpenRead(filePath);
                    await fileStream.CopyToAsync(entryStream);
                }
            }

            var bytes = memStream.ToArray();
            return bytes.Length == 0 ? null : bytes;
        }

        public async Task<List<CampaignQualityDto>> GetCampaignQualitiesAsync(
            string userId, int userSite, string userRole)
        {
            return await _db.LsCalledCampaigns
                .Where(c => c.Status == 1)
                .Select(c => new CampaignQualityDto
                {
                    Id          = c.Id,
                    Description = c.Description ?? ""
                })
                .ToListAsync();
        }

        public Task<List<CallStatusItemDto>> GetCallStatusItemsAsync(
            string customerId, string campaignId, int callType)
        {
            return Task.FromResult(new List<CallStatusItemDto>());
        }

        // ══════════════════════════════════════════════════════
        // REQUALIFICATION
        // ══════════════════════════════════════════════════════
        public async Task RequalifyRecordAsync(RequalificationDto dto, int userId)
        {
            var record = await _db.RecordData
                .FirstOrDefaultAsync(r => r.Id == dto.RecordId)
                ?? throw new KeyNotFoundException($"Record {dto.RecordId} introuvable.");

            int typeRequalif = int.TryParse(dto.TypeRequalif, out int parsed) ? parsed : -1;

            if (typeRequalif == 0)
            {
                record.StatusGroupeRequal = dto.StatusGroupeRequal;
                record.StatusNumRequal    = dto.StatusNumRequal;
                record.StatusDetailRequal = dto.StatusDetailRequal;
                record.StatusRequal       = dto.StatusRequal;
                record.TypeRequalif       = 0;
                record.TypeImport         = 1;
            }
            else
            {
                record.StatusGroupeRequal = 0;
                record.StatusNumRequal    = 0;
                record.StatusDetailRequal = 0;
                record.StatusRequal       = dto.StatusRequal;
                record.TypeRequalif       = 1;
                record.TypeImport         = 1;
            }

            await _db.SaveChangesAsync();
        }

        // ══════════════════════════════════════════════════════
        // ENVOI EMAIL
        // ══════════════════════════════════════════════════════
private async Task TrySendEmailAsync(
    LsSurvey survey,
    List<LsSurveyItem> items,
    List<LsTemplateItem> templateItems,
    RecordDatum record,
    string? cc)
{
    try
    {
        // ── 1. Email agent ────────────────────────────────────
        var agentEmail = await _db.TListAgentEmails
            .Where(e => e.Oidagent == record.AgentOid)
            .Select(e => e.Email)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(agentEmail))
        {
            _log.LogWarning("Email → ⚠️ Aucun email trouvé pour l'agent {AgentOid}", record.AgentOid);
            return;
        }

        // ── 2. Infos agent ────────────────────────────────────
        var agentName = $"{record.PrenomAgent} {record.NomAgent}".Trim();
        var safeName  = string.Join("_", agentName.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        // ── 3. Config SMTP ────────────────────────────────────
        var smtpHost  = _cfg["Email:SmtpHost"]  ?? "smtp.gmail.com";
        var smtpPort  = int.Parse(_cfg["Email:Port"] ?? "587");
        var username  = _cfg["Email:Username"]  ?? "";
        var password  = _cfg["Email:Password"]  ?? "";
        var fromAddr  = _cfg["Email:From"]      ?? username;
        var enableSsl = bool.Parse(_cfg["Email:EnableSsl"] ?? "true");

        // ── 4. Dictionnaire templates ─────────────────────────
        var templateDict = templateItems.ToDictionary(t => t.Id);

        // ── 5. Générer le PDF ─────────────────────────────────
        var pdfBytes = GeneratePdf(survey, items, templateDict, record, agentName);
        var pdfName  = $"evaluation_{safeName}_{DateTime.Now:yyyyMMdd}.pdf";

        // ── 6. Score couleur pour l'email ─────────────────────
        var scoreColor  = survey.Score >= 80 ? "#16a34a" : survey.Score >= 60 ? "#d97706" : "#dc2626";
        var scoreBg     = survey.Score >= 80 ? "#f0fdf4" : survey.Score >= 60 ? "#fffbeb" : "#fef2f2";
        var scoreBorder = survey.Score >= 80 ? "#86efac" : survey.Score >= 60 ? "#fcd34d" : "#fca5a5";
        var scoreLabel  = survey.Score >= 80 ? "✅ Excellent" : survey.Score >= 60 ? "⚠️ À améliorer" : "❌ Insuffisant";

        // ── 7. Corps email ────────────────────────────────────
        var html = $@"
<!DOCTYPE html>
<html lang='fr'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0;padding:0;background:#f1f5f9;font-family:Segoe UI,Arial,sans-serif;'>

  <!-- Wrapper -->
  <table width='100%' cellpadding='0' cellspacing='0' style='background:#f1f5f9;padding:32px 0;'>
    <tr><td align='center'>
      <table width='560' cellpadding='0' cellspacing='0' style='max-width:560px;width:100%;'>

        <!-- HEADER -->
        <tr>
          <td style='background:#dc2626;border-radius:12px 12px 0 0;padding:28px 32px;'>
            <div style='font-size:11px;color:#fca5a5;font-weight:600;
                        text-transform:uppercase;letter-spacing:1px;margin-bottom:6px;'>
              Système de scoring qualité
            </div>
            <div style='font-size:22px;font-weight:700;color:#ffffff;margin-bottom:4px;'>
              Rapport d'évaluation
            </div>
            <div style='font-size:13px;color:#fecaca;'>
              {DateTime.Now:dddd dd MMMM yyyy}
            </div>
          </td>
        </tr>

        <!-- BODY -->
        <tr>
          <td style='background:#ffffff;padding:32px;'>

            <!-- Salutation -->
            <div style='font-size:17px;font-weight:700;color:#111827;margin-bottom:8px;'>
              Bonjour, <span style='color:#dc2626;'>{agentName}</span> 👋
            </div>
            <div style='font-size:13px;color:#6b7280;line-height:1.8;margin-bottom:28px;'>
              Votre évaluation qualité du
              <strong style='color:#374151;'>{DateTime.Now:dd/MM/yyyy}</strong>
              est disponible. Veuillez consulter le rapport en pièce jointe.
            </div>

            <!-- Score card -->
            <div style='background:{scoreBg};border:1px solid {scoreBorder};
                        border-radius:10px;padding:20px 24px;margin-bottom:24px;
                        border-left:4px solid {scoreColor};'>
              <div style='font-size:11px;color:#9ca3af;font-weight:600;
                          text-transform:uppercase;letter-spacing:.5px;margin-bottom:6px;'>
                Score global
              </div>
              <div style='display:flex;align-items:center;gap:12px;'>
                <span style='font-size:36px;font-weight:800;color:{scoreColor};'>
                  {survey.Score:F1}%
                </span>
                <span style='font-size:13px;font-weight:600;color:{scoreColor};
                             background:{scoreBg};padding:4px 12px;
                             border-radius:20px;border:1px solid {scoreBorder};'>
                  {scoreLabel}
                </span>
              </div>
            </div>

            <!-- Infos -->
            <table width='100%' cellpadding='0' cellspacing='0'
                   style='background:#f9fafb;border:1px solid #e5e7eb;
                          border-radius:8px;margin-bottom:24px;'>
              <tr>
                <td style='padding:14px 20px;border-right:1px solid #e5e7eb;'>
                  <div style='font-size:10px;color:#9ca3af;font-weight:600;
                              text-transform:uppercase;margin-bottom:3px;'>
                    Date d'enregistrement
                  </div>
                  <div style='font-size:13px;font-weight:600;color:#374151;'>
                    {(record.RecordDate.HasValue ? record.RecordDate.Value.ToString("dd/MM/yyyy") : "—")}
                  </div>
                </td>
                <td style='padding:14px 20px;border-right:1px solid #e5e7eb;'>
                  <div style='font-size:10px;color:#9ca3af;font-weight:600;
                              text-transform:uppercase;margin-bottom:3px;'>
                    Date d'évaluation
                  </div>
                  <div style='font-size:13px;font-weight:600;color:#374151;'>
                    {DateTime.Now:dd/MM/yyyy}
                  </div>
                </td>
                <td style='padding:14px 20px;'>
                  <div style='font-size:10px;color:#9ca3af;font-weight:600;
                              text-transform:uppercase;margin-bottom:3px;'>
                    Indice
                  </div>
                  <div style='font-size:13px;font-weight:600;color:#374151;'>
                    {record.RecIdLink?.ToString() ?? "—"}
                  </div>
                </td>
              </tr>
            </table>

            <!-- Pièce jointe info -->
            <div style='background:#fff;border:1px dashed #d1d5db;border-radius:8px;
                        padding:14px 18px;margin-bottom:28px;
                        display:flex;align-items:center;gap:12px;'>
              
            </div>

            <!-- CTA -->
            <div style='text-align:center;margin-bottom:28px;'>
              <div style='display:inline-block;background:#dc2626;color:#fff;
                          padding:12px 32px;border-radius:8px;font-size:13px;
                          font-weight:700;letter-spacing:.3px;'>
                📎 Consultez votre rapport en pièce jointe
              </div>
            </div>

          </td>
        </tr>

        <!-- FOOTER -->
        <tr>
          <td style='background:#f9fafb;border-top:1px solid #e5e7eb;
                     border-radius:0 0 12px 12px;padding:18px 32px;
                     text-align:center;'>
            <div style='font-size:11px;color:#9ca3af;line-height:1.7;'>
              Ce message est généré automatiquement par le système de scoring qualité.<br>
              <strong style='color:#6b7280;'>Merci de ne pas répondre à cet email.</strong>
            </div>
          </td>
        </tr>

      </table>
    </td></tr>
  </table>

</body>
</html>";

        // ── 8. Construction du mail ───────────────────────────
        using var mail = new MailMessage();
        mail.From       = new MailAddress(fromAddr);
        mail.To.Add(agentEmail.Trim());
        mail.Subject    = $"{agentName} — Résultat de votre évaluation qualité du {DateTime.Now:dd/MM/yyyy}";
        mail.Body       = html;
        mail.IsBodyHtml = true;

        if (!string.IsNullOrWhiteSpace(cc))
            mail.CC.Add(cc.Trim());

        // ── 9. Attacher le PDF ────────────────────────────────
        var pdfStream  = new MemoryStream(pdfBytes);
        var attachment = new Attachment(pdfStream, pdfName, "application/pdf");
        mail.Attachments.Add(attachment);

        // ── 10. Envoi SMTP ────────────────────────────────────
        using var client = new SmtpClient(smtpHost)
        {
            Port                  = smtpPort,
            Credentials           = new System.Net.NetworkCredential(username, password),
            EnableSsl             = enableSsl,
            UseDefaultCredentials = false,
            DeliveryMethod        = SmtpDeliveryMethod.Network,
        };

        await client.SendMailAsync(mail);

        _log.LogInformation("Email → ✅ Envoyé à {To} avec PDF joint (CC: {Cc})",
            agentEmail, cc ?? "aucun");
    }
    catch (Exception ex)
    {
        _log.LogError(ex,
            "Email → ❌ Échec envoi pour agentOid={AgentOid} surveyId={SurveyId}",
            record.AgentOid, survey.Id);
    }
}

// ── Génération PDF ────────────────────────────────────────────────────────
private byte[] GeneratePdf(
    LsSurvey survey,
    List<LsSurveyItem> items,
    Dictionary<int, LsTemplateItem> templateDict,
    RecordDatum record,
    string agentName)
{
    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

    var initiales = string.Concat(
        agentName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                 .Take(2)
                 .Select(w => w[0].ToString().ToUpper()));

    var scoreColor = survey.Score >= 80 ? Colors.Green.Medium
                   : survey.Score >= 60 ? Colors.Orange.Medium
                   : Colors.Red.Medium;

    var scoreLabel = survey.Score >= 80 ? "Excellent"
                   : survey.Score >= 60 ? "À améliorer"
                   : "Insuffisant";

    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

            // ── HEADER ────────────────────────────────────────
            page.Header().Background("#dc2626").Padding(20).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("RAPPORT D'ÉVALUATION QUALITÉ")
                       .FontSize(8).FontColor("#fca5a5").Bold()
                       .LetterSpacing(0.08f);
                    col.Item().PaddingTop(4).Text(agentName)
                       .FontSize(18).FontColor("#ffffff").Bold();
                    col.Item().PaddingTop(2).Text($"Évaluation du {DateTime.Now:dd/MM/yyyy}")
                       .FontSize(9).FontColor("#fecaca");
                });

                row.ConstantItem(60).AlignMiddle().AlignRight()
                   .Width(52).Height(52)
                   .Background("#991b1b")
                   .AlignCenter().AlignMiddle()
                   .Text(initiales)
                   .FontSize(20).FontColor("#ffffff").Bold();
            });

            // ── CONTENU ───────────────────────────────────────
            page.Content().PaddingTop(20).Column(col =>
            {
                // ── Score + Infos ─────────────────────────────
                col.Item()
                   .Border(1).BorderColor("#e5e7eb")
                   .Background("#f9fafb")
                   .Padding(16).Row(row =>
                   {
                       // Score
                       row.RelativeItem(2).Column(c =>
                       {
                           c.Item().Text("SCORE GLOBAL")
                            .FontSize(8).FontColor("#9ca3af").Bold().LetterSpacing(0.05f);
                           c.Item().PaddingTop(4).Text($"{survey.Score:F1}%")
                            .FontSize(32).Bold().FontColor(scoreColor);
                           c.Item().Text(scoreLabel)
                            .FontSize(9).Bold().FontColor(scoreColor);
                       });

                       // Séparateur
                       row.ConstantItem(1).Background("#e5e7eb");

                       // Dates + Indice
                       row.RelativeItem(3).PaddingLeft(16).Column(c =>
                       {
                           c.Item().Row(r =>
                           {
                               r.RelativeItem().Column(inner =>
                               {
                                   inner.Item().Text("DATE ENREGISTREMENT")
                                        .FontSize(8).FontColor("#9ca3af").Bold();
                                   inner.Item().PaddingTop(2)
                                        .Text(record.RecordDate.HasValue
                                            ? record.RecordDate.Value.ToString("dd/MM/yyyy") : "—")
                                        .FontSize(11).Bold();
                               });
                               r.RelativeItem().Column(inner =>
                               {
                                   inner.Item().Text("DATE ÉVALUATION")
                                        .FontSize(8).FontColor("#9ca3af").Bold();
                                   inner.Item().PaddingTop(2)
                                        .Text(DateTime.Now.ToString("dd/MM/yyyy"))
                                        .FontSize(11).Bold();
                               });
                               r.RelativeItem().Column(inner =>
                               {
                                   inner.Item().Text("INDICE")
                                        .FontSize(8).FontColor("#9ca3af").Bold();
                                   inner.Item().PaddingTop(2)
                                        .Text(record.RecIdLink?.ToString() ?? "—")
                                        .FontSize(11).Bold();
                               });
                           });
                       });
                   });

                // ── Commentaire ───────────────────────────────
                if (!string.IsNullOrWhiteSpace(survey.Memo))
                {
                    col.Item().PaddingTop(14)
                       .Background("#fef2f2")
                       .BorderLeft(4).BorderColor("#dc2626")
                       .Padding(12).Column(c =>
                       {
                           c.Item().Text("💬 COMMENTAIRE")
                            .FontSize(8).Bold().FontColor("#dc2626").LetterSpacing(0.05f);
                           c.Item().PaddingTop(4).Text(survey.Memo)
                            .FontSize(10).FontColor("#374151").LineHeight(1.5f);
                       });
                }

                // ── Action prise ──────────────────────────────
                if (!string.IsNullOrWhiteSpace(survey.MemoActionTaken))
                {
                    col.Item().PaddingTop(10)
                       .Background("#f0fdf4")
                       .BorderLeft(4).BorderColor("#16a34a")
                       .Padding(12).Column(c =>
                       {
                           c.Item().Text("✅ ACTION PRISE AVEC L'AGENT")
                            .FontSize(8).Bold().FontColor("#16a34a").LetterSpacing(0.05f);
                           c.Item().PaddingTop(4).Text(survey.MemoActionTaken)
                            .FontSize(10).FontColor("#374151").LineHeight(1.5f);
                       });
                }

                // ── Titre tableau ─────────────────────────────
                col.Item().PaddingTop(18).PaddingBottom(8).Row(r =>
                {
                    r.RelativeItem().Text("Détail des critères évalués")
                     .FontSize(12).Bold().FontColor("#111827");
                    r.ConstantItem(80).AlignRight()
                     .Text($"{items.Count} critère(s)")
                     .FontSize(9).FontColor("#9ca3af");
                });

                // ── Tableau ───────────────────────────────────
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(4);
                        columns.RelativeColumn(4);
                        columns.RelativeColumn(2);
                    });

                    // En-tête
                    table.Header(h =>
                    {
                        foreach (var label in new[] { "CRITÈRE", "COMMENTAIRE", "NOTE" })
                        {
                            var cell = h.Cell().Background("#dc2626").Padding(9);
                            if (label == "NOTE")
                                cell.AlignCenter().Text(label).FontSize(9).Bold().FontColor("#ffffff");
                            else
                                cell.Text(label).FontSize(9).Bold().FontColor("#ffffff");
                        }
                    });

                    var rowIndex = 0;
                    foreach (var item in items)
                    {
                        if (!templateDict.TryGetValue(item.ItemId, out var ti)) continue;

                        var bg          = rowIndex % 2 == 0 ? "#ffffff" : "#f9fafb";
                        var description = ti.Description ?? item.ItemId.ToString();
                        var memo        = string.IsNullOrWhiteSpace(item.Memo) ? "—" : item.Memo;

                        var noteColor = item.Value >= 80 ? Colors.Green.Darken2
                                      : item.Value >= 60 ? Colors.Orange.Darken2
                                      : Colors.Red.Darken2;

                        table.Cell().Background(bg)
                             .BorderBottom(1).BorderColor("#f3f4f6")
                             .Padding(9).Text(description)
                             .FontSize(9).FontColor("#374151");

                        table.Cell().Background(bg)
                             .BorderBottom(1).BorderColor("#f3f4f6")
                             .Padding(9).Text(memo)
                             .FontSize(9).FontColor("#6b7280");

                        table.Cell().Background(bg)
                             .BorderBottom(1).BorderColor("#f3f4f6")
                             .Padding(9).AlignCenter()
                             .Text($"{item.Value}")
                             .FontSize(10).Bold().FontColor(noteColor);

                        rowIndex++;
                    }
                });
            });

            // ── FOOTER ────────────────────────────────────────
            page.Footer().PaddingTop(8)
                .BorderTop(1).BorderColor("#e5e7eb")
                .Row(row =>
                {
                    row.RelativeItem().AlignLeft()
                       .Text("Rapport généré automatiquement — Système de scoring qualité")
                       .FontSize(8).FontColor("#9ca3af");

                    row.ConstantItem(80).AlignRight().Text(text =>
                    {
                        text.Span("Page ").FontSize(8).FontColor("#9ca3af");
                        text.CurrentPageNumber().FontSize(8).FontColor("#9ca3af");
                        text.Span(" / ").FontSize(8).FontColor("#9ca3af");
                        text.TotalPages().FontSize(8).FontColor("#9ca3af");
                    });
                });
        });
    }).GeneratePdf();

    
}    // ══════════════════════════════════════════════════════
        // GET ALL SURVEYS FOR RECORD
        // ══════════════════════════════════════════════════════
        public async Task<MultiSurveyResponseDto> GetAllSurveysForRecordAsync(int recordId)
        {
            var surveys = await _db.LsSurveys
                .Where(s => s.RecordDataId == recordId && s.IsSaved == 1)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (!surveys.Any())
                return new MultiSurveyResponseDto { Surveys = new() };

            var surveyIds = surveys.Select(s => s.Id).ToList();

            var allSurveyItems = await _db.LsSurveyItems
                .Where(i => surveyIds.Contains(i.SurveyId))
                .ToListAsync();

            var templateItems = await _db.LsSurveyItems
                .Where(i => surveyIds.Contains(i.SurveyId))
                .Join(_db.LsTemplateItems.Include(t => t.Group),
                      si => si.ItemId,
                      ti => ti.Id,
                      (si, ti) => ti)
                .Distinct()
                .ToListAsync();

            var surveyColumns = surveys.Select((s, index) => new SurveyColumnDto
            {
                Label    = $"E{index + 1}",
                SurveyId = s.Id,
                Score    = s.Score ?? 0f
            }).ToList();

            var rows = templateItems
                .OrderBy(t => t.Group?.Order ?? 0)
                .ThenBy(t => t.Order)
                .Select(ti =>
                {
                    var group = ti.Group;
                    var values = surveys.Select(s =>
                    {
                        var item = allSurveyItems
                            .FirstOrDefault(i => i.SurveyId == s.Id && i.ItemId == ti.Id);
                        return item?.Value ?? 0f;
                    }).ToList();

                    return new MultiSurveyRowDto
                    {
                        TemplateItemId = ti.Id,
                        GroupId        = group?.Id          ?? 0,
                        GroupName      = group?.Description ?? "",
                        GroupOrder     = group?.Order       ?? 0,
                        Question       = ti.Question        ?? "",
                        Definition     = ti.Description     ?? "",
                        ScaleMax       = ti.Max,
                        ScaleMin       = ti.Min,
                        IsNA           = ti.IsNa == 1,
                        ItemOrder      = ti.Order,
                        Values         = values
                    };
                })
                .ToList();

            return new MultiSurveyResponseDto
            {
                Surveys = surveyColumns,
                Rows    = rows
            };
        }
public async Task<RecordScreenDto?> GetRecordScreenPathAsync(int recordId)
{
    var record = await _db.VwlistRecordGvs
        .Where(r => r.Id == recordId)
        .Select(r => new RecordScreenDto
        {
            Id           = r.Id,
            ScreenSource = r.ScreenSource
        })
        .FirstOrDefaultAsync();

    // LOG pour diagnostic
    Console.WriteLine($"🎬 GetRecordScreenPath → recordId={recordId}");
    Console.WriteLine($"🎬 ScreenSource={record?.ScreenSource ?? "NULL"}");
    Console.WriteLine($"🎬 File.Exists={System.IO.File.Exists(record?.ScreenSource ?? "")}");

    return record;
}

    }
}