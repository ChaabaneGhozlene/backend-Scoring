using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Models.Admin;
using scoring_Backend.Repositories.Interfaces.Statistique;

namespace scoring_Backend.Repositories.Implementations.Statistique
{
    public class StatistiqueRepository : IStatistiqueRepository
    {
        private readonly SqrScoringContext _db;
        private readonly SqrAdminContext   _adminDb;

        public StatistiqueRepository(SqrScoringContext db, SqrAdminContext adminDb)
        {
            _db      = db;
            _adminDb = adminDb;
        }

        // ─── Section Stats ─────────────────────────────────────────────────────
        public async Task<IEnumerable<SectionStatsDto>> GetSectionStatsAsync(
            StatFilterDto filter, int userId, int userRole, int siteId)
        {
            var dateFrom = filter.DateFrom.Date;
            var dateTo   = filter.DateTo.Date.AddDays(1).AddSeconds(-1);

            var sql = @"
                SELECT
                    grp.Id          AS GroupId,
                    grp.[Order]     AS SectionOrder,
                    grp.Description AS Section,
                    l.Agent         AS Agent,
                    ISNULL(l.AgentId, 0) AS AgentId,
                    cc.Description  AS Campaign,
                    ss.ScoreGroup   AS ScoreGroup
                FROM LsScoreSection ss
                INNER JOIN LsSurvey sv       ON ss.IdLsSurvey            = sv.Id
                INNER JOIN Ls l              ON sv.LsId                  = l.Id
                INNER JOIN LsCalledCampaign cc ON l.CalledCampaignId     = cc.Id
                INNER JOIN LsTemplateItemGroup grp ON ss.IdLsTemplateItemGroup = grp.Id
                WHERE sv.CreateDate >= {0}
                  AND sv.CreateDate <= {1}
                  AND sv.IsSaved    = {2}";

            var raw = await _db.Database
                .SqlQueryRaw<SectionRawRow>(sql, dateFrom, dateTo, 1)
                .ToListAsync();

            if (!raw.Any())
                return Enumerable.Empty<SectionStatsDto>();

            var groupIds = raw.Select(r => r.GroupId).Distinct().ToList();

            var questions = await _db.LsTemplateItems
                .Where(q => groupIds.Contains(q.GroupId))
                .Select(q => new { q.Id, q.Order, q.Description, q.GroupId })
                .ToListAsync();

            var questionsByGroup = questions
                .GroupBy(q => q.GroupId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(q => q.Order)
                          .Select(q => new SectionQuestionDto
                          {
                              QuestionId    = q.Id,
                              QuestionOrder = q.Order,
                              Description   = q.Description ?? string.Empty,
                              GroupId       = q.GroupId,
                          }).ToList()
                );

            return raw
                .GroupBy(r => new
                {
                    r.Agent, r.AgentId, r.GroupId,
                    r.SectionOrder, r.Section, r.Campaign,
                })
                .Select(g => new SectionStatsDto
                {
                    SectionId    = g.Key.GroupId,
                    SectionOrder = g.Key.SectionOrder,
                    Section      = g.Key.Section   ?? string.Empty,
                    Agent        = g.Key.Agent      ?? string.Empty,
                    AgentId      = g.Key.AgentId,
                    Campaign     = g.Key.Campaign   ?? string.Empty,
                    Reference    = g.Count(),
                    ScoreGroup   = g.Sum(x => x.ScoreGroup),
                    Questions    = questionsByGroup.TryGetValue(g.Key.GroupId, out var qs)
                                       ? qs
                                       : new List<SectionQuestionDto>(),
                })
                .OrderBy(r => r.Agent)
                .ThenBy(r => r.SectionOrder)
                .ToList();
        }

        // ─── Agent Scores ──────────────────────────────────────────────────────
        public async Task<IEnumerable<AgentScoreDto>> GetAgentScoresAsync(
            StatFilterDto filter, int userId, int userRole, int siteId, string sortDirection)
        {
            var dateFrom = filter.DateFrom.Date;
            var dateTo   = filter.DateTo.Date.AddDays(1).AddSeconds(-1);

            var allowedCampagnes = await GetAllowedCampagnes(userId, userRole, siteId);

            var query =
                from sv in _db.LsSurveys
                join l  in _db.Ls                on sv.LsId            equals l.Id
                join cc in _db.LsCalledCampaigns on l.CalledCampaignId equals cc.Id
                where sv.CreateDate >= dateFrom
                   && sv.CreateDate <= dateTo
                   && sv.IsSaved == 1
                select new { sv, l, cc };

            query = userRole switch
            {
                2 => query.Where(x =>
                        allowedCampagnes.Contains(x.cc.CampagneDid!) &&
                        x.cc.Site == siteId &&
                        (siteId == 4 ? x.l.AgentId > 6000 : x.l.AgentId < 6000)),

                3 => filter.AllSupervisors
    ? query.Where(x =>
        siteId == 4 ? x.l.AgentId > 6000 : x.l.AgentId < 6000)
    : query.Where(x =>
        x.l.Auditor == (filter.SupervisorId ?? userId) &&  // ← SupervisorId ou fallback userId
        (siteId == 4 ? x.l.AgentId > 6000 : x.l.AgentId < 6000)),
                _ => query
            };

            var grouped = await query
                .GroupBy(x => x.l.Agent)
                .Select(g => new AgentScoreDto
                {
                    Agent = g.Key ?? string.Empty,
                    Score = g.Average(x => (double?)x.sv.Score) ?? 0
                })
                .ToListAsync();

            return sortDirection == "Ascending"
                ? grouped.OrderBy(x => x.Score)
                : grouped.OrderByDescending(x => x.Score);
        }

        // ─── Program Level ─────────────────────────────────────────────────────
        public async Task<IEnumerable<ProgramLevelDto>> GetProgramLevelAsync(
            StatFilterDto filter, int userId, int userRole, int siteId, bool allSupervisors)
        {
            var dateFrom = filter.DateFrom.Date;
            var dateTo   = filter.DateTo.Date.AddDays(1).AddSeconds(-1);

            var allowedCampagnes = await GetAllowedCampagnes(userId, userRole, siteId);

            var query =
                from sv in _db.LsSurveys
                join l  in _db.Ls                on sv.LsId            equals l.Id
                join cc in _db.LsCalledCampaigns on l.CalledCampaignId equals cc.Id
                where sv.CreateDate >= dateFrom
                   && sv.CreateDate <= dateTo
                   && sv.IsSaved == 1
                select new { sv, l, cc };

            if (userRole == 2)
                query = query.Where(x =>
                    allowedCampagnes.Contains(x.cc.CampagneDid!) &&
                    x.cc.Site == siteId);
            else if (userRole == 3 && !allSupervisors)
                query = query.Where(x => x.l.Auditor == userId);

            return await query.Select(x => new ProgramLevelDto
            {
                Agent      = x.l.Agent      ?? string.Empty,
                CreateDate = x.sv.CreateDate,
                Score      = x.sv.Score     ?? 0
            }).ToListAsync();
        }

        // ─── Coaching Sheet ────────────────────────────────────────────────────
        public async Task<IEnumerable<CoachingSheetDto>> GetCoachingSheetAsync(
            StatFilterDto filter, int agentId, int supervisorId, int userRole, bool allSupervisors)
        {
            var dateFrom = filter.DateFrom.Date;
            var dateTo   = filter.DateTo.Date.AddDays(1).AddSeconds(-1);

            var query =
                from si in _db.LsSurveyItems
                join sv in _db.LsSurveys        on si.SurveyId    equals sv.Id
                join l  in _db.Ls               on sv.LsId        equals l.Id
                join ti in _db.LsTemplateItems  on si.ItemId      equals ti.Id into tiGroup
                from ti in tiGroup.DefaultIfEmpty()
                join rd in _db.RecordData       on sv.RecordDataId equals rd.Id into rdGroup
                from rd in rdGroup.DefaultIfEmpty()
                where sv.CreateDate >= dateFrom
                   && sv.CreateDate <= dateTo
                   && sv.IsSaved == 1
                   && l.AgentId  == agentId
                select new { si, sv, l, ti, rd };

            if (!allSupervisors && userRole == 3)
                query = query.Where(x => x.l.Auditor == supervisorId);

            return await query.Select(x => new CoachingSheetDto
            {
                Id              = x.si.Id,
                CallIndex       = x.rd != null ? x.rd.RecIdLink.ToString() : string.Empty,
                EvaluationScore = x.sv.Score ?? 0,
                Question        = x.ti != null ? x.ti.Description ?? string.Empty : string.Empty,
                ItemScore       = x.si.Value ?? 0,
                Comment         = x.si.Memo  ?? string.Empty
            }).ToListAsync();
        }

        // ─── Coaching Analysis ─────────────────────────────────────────────────
        public async Task<IEnumerable<CoachingAnalysisDto>> GetCoachingAnalysisAsync(
            StatFilterDto filter, int agentId, int userId, int userRole, bool allSupervisors)
        {
            var dateFrom = filter.DateFrom.Date;
            var dateTo   = filter.DateTo.Date.AddDays(1).AddSeconds(-1);

            var query =
                from si  in _db.LsSurveyItems
                join sv  in _db.LsSurveys            on si.SurveyId equals sv.Id
                join l   in _db.Ls                   on sv.LsId     equals l.Id
                join ti  in _db.LsTemplateItems      on si.ItemId   equals ti.Id into tiGroup
                from ti in tiGroup.DefaultIfEmpty()
                join grp in _db.LsTemplateItemGroups on ti!.GroupId equals grp.Id into grpGroup
                from grp in grpGroup.DefaultIfEmpty()
                where sv.CreateDate >= dateFrom
                   && sv.CreateDate <= dateTo
                   && sv.IsSaved == 1
                   && l.AgentId  == agentId
                   && si.Value   >= 0
                select new { si, sv, l, ti, grp };

            if (!allSupervisors && userRole == 3)
                query = query.Where(x => x.l.Auditor == userId);

            var items = await query.ToListAsync();

            return items
                .GroupBy(x => new
                {
                    SectionId = x.grp?.Id          ?? 0,
                    Section   = x.grp?.Description ?? string.Empty,
                    ErrorType = x.ti?.Description  ?? string.Empty
                })
                .Select(g => new CoachingAnalysisDto
                {
                    SectionId       = g.Key.SectionId,
                    Section         = g.Key.Section,
                    ErrorType       = g.Key.ErrorType,
                    Occurrence      = g.Count(),
                    PositiveAnswers = g.Count(x => x.si.Value > 0),
                    LoseRate        = g.Count() > 0
                        ? Math.Round(100 - ((double)g.Count(x => x.si.Value > 0) / g.Count() * 100), 2)
                        : 0
                });
        }

        // ─── Coaching Summary ──────────────────────────────────────────────────
        public async Task<IEnumerable<CoachingSummaryDto>> GetCoachingSummaryAsync(
            StatFilterDto filter, int agentId, int userId, int userRole, bool allSupervisors)
        {
            var dateFrom = filter.DateFrom.Date;
            var dateTo   = filter.DateTo.Date.AddDays(1).AddSeconds(-1);

            var query =
                from sv in _db.LsSurveys
                join l  in _db.Ls         on sv.LsId         equals l.Id
                join rd in _db.RecordData on sv.RecordDataId equals rd.Id into rdGroup
                from rd in rdGroup.DefaultIfEmpty()
                where sv.CreateDate >= dateFrom
                   && sv.CreateDate <= dateTo
                   && sv.IsSaved == 1
                   && l.AgentId  == agentId
                select new { sv, l, rd };

            if (!allSupervisors && userRole == 3)
                query = query.Where(x => x.l.Auditor == userId);

            return await query.Select(x => new CoachingSummaryDto
            {
                Id        = x.sv.Id,
                CallIndex = x.rd != null ? x.rd.RecIdLink.ToString() : string.Empty,
                Score     = x.sv.Score ?? 0,
                Comment   = x.sv.Memo  ?? string.Empty
            }).ToListAsync();
        }

        // ─── Agent List ────────────────────────────────────────────────────────
        public async Task<IEnumerable<AgentListDto>> GetAgentListAsync(
            int userId, int userRole, int siteId, bool allSupervisors)
        {
            if (userRole == 1)
            {
                return await _db.Ls
                    .Where(l => l.AgentId != null && l.Agent != null)
                    .Select(l => new AgentListDto { Id = l.AgentId!.Value, Agent = l.Agent! })
                    .Distinct()
                    .ToListAsync();
            }

            List<int> allowedAgentIds = userRole switch
            {
                2 => await _adminDb.UsersAgents
                        .Where(ua => ua.UserId == userId)
                        .Select(ua => ua.AgentId)
                        .Distinct()
                        .ToListAsync(),

                3 when allSupervisors => await _adminDb.UsersAgents
                        .Where(ua => ua.SiteId == siteId)
                        .Select(ua => ua.AgentId)
                        .Distinct()
                        .ToListAsync(),

                3 => await _adminDb.UsersAgents
                        .Where(ua => ua.UserId == userId)
                        .Select(ua => ua.AgentId)
                        .Distinct()
                        .ToListAsync(),

                _ => new List<int>()
            };

            if (!allowedAgentIds.Any())
                return Enumerable.Empty<AgentListDto>();

            return await _db.Ls
                .Where(l => l.AgentId != null
                         && l.Agent   != null
                         && allowedAgentIds.Contains(l.AgentId!.Value))
                .Select(l => new AgentListDto { Id = l.AgentId!.Value, Agent = l.Agent! })
                .Distinct()
                .ToListAsync();
        }

        // ─── Supervisors ───────────────────────────────────────────────────────
        public async Task<IEnumerable<SupervisorDto>> GetSupervisorsAsync(  // ✅ ajouté
            int userId, int userRole, int siteId)
        {
            var query = _adminDb.Users.Where(u => u.Role <= 3);

            query = userRole switch
            {
                1 => query,
                2 => query.Where(u => u.SiteId == siteId),
                3 => query.Where(u => u.Id == userId),
                _ => query
            };

            return await query
                .Select(u => new SupervisorDto
                {
                    Id   = u.Id,
                    Name = (u.FirstName ?? string.Empty) + " " + u.LastName,
                })
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // ─── Helper : campagnes autorisées ─────────────────────────────────────
        private async Task<List<string>> GetAllowedCampagnes(
            int userId, int userRole, int siteId)
        {
            if (userRole != 2)
                return new List<string>();

            return await _adminDb.UsersCampagnes
                .Where(uc => uc.UserId == userId && uc.SiteId == siteId)
                .Select(uc => uc.CampagneId)
                .ToListAsync();
        }
    }
}