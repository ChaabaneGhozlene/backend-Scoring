using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Models.Admin;
using scoring_Backend.Repositories.Interfaces.Statistique;

namespace scoring_Backend.Repositories.Implementations.Statistique
{
    public class SectionStatRepository : ISectionStatRepository
    {
        private readonly SqrScoringContext _db;
        private readonly SqrAdminContext   _adminDb;

        public SectionStatRepository(SqrScoringContext db, SqrAdminContext adminDb)
        {
            _db      = db;
            _adminDb = adminDb;
        }

        public async Task<SectionStatResponseDto> GetSectionStatsAsync(
            SectionStatFilterDto filter,
            int? userId   = null,
            int? userRole = null,
            int? siteId   = null)
        {
            var dateStart = filter.DateDebut.Date;
            var dateEnd   = filter.DateFin.Date.AddDays(1).AddSeconds(-1);

            // --- Jointures explicites sur les vraies entités du DbContext ---
            // LsScoreSection → LsSurvey → L → LsTemplateItemGroup
            var query =
                from section in _db.LsScoreSections
                join survey in _db.LsSurveys
                    on section.IdLsSurvey equals survey.Id
                join ls in _db.Ls
                    on survey.LsId equals ls.Id
                join grp in _db.LsTemplateItemGroups
                    on section.IdLsTemplateItemGroup equals grp.Id
                // Jointure vers LsCalledCampaign pour récupérer la description campagne
                join campaign in _db.LsCalledCampaigns
                    on ls.Id equals campaign.IdLsTemplate into campaignJoin
                from campaign in campaignJoin.DefaultIfEmpty()   // LEFT JOIN
                where survey.IsSaved == 1
                   && ls.CreateDate  >= dateStart
                   && ls.CreateDate  <= dateEnd
                select new
                {
                    Section        = grp,
                    Survey         = survey,
                    Ls             = ls,
                    Campaign       = campaign,
                    ScoreGroup     = section.ScoreGroup
                };

            // ----------------------------------------------------------------
            // Role 2 : filtrage par campagnes autorisées (SqrAdminContext)
            // UsersCampagne : UserId, SiteId, CampagneId (string, max 64)
            // ----------------------------------------------------------------
            if (userRole == 2 && userId.HasValue && siteId.HasValue)
            {
                var allowedCampaignIds = await _adminDb.UsersCampagnes
                    .Where(uc => uc.UserId == userId.Value
                              && uc.SiteId == siteId.Value)
                    .Select(uc => uc.CampagneId)   // string (max 64)
                    .ToListAsync();

                query = query.Where(q =>
                    allowedCampaignIds.Contains(q.Campaign.CampagneDid));
            }
            // ----------------------------------------------------------------
            // Role 3 : filtrage par auditeur
            // L.UserName est un string — on compare avec userId converti
            // ----------------------------------------------------------------
            else if (userRole == 3 && userId.HasValue)
            {
                var userIdStr = userId.Value.ToString();
                query = query.Where(q => q.Ls.UserName == userIdStr);
            }

            // ----------------------------------------------------------------
            // Projection + groupement
            // ----------------------------------------------------------------
            var rows = await query
                .GroupBy(q => new
                {
                    SectionId    = q.Section.Id,
                    SectionName  = q.Section.Description,
                    Agent        = q.Ls.Agent,
                    AgentOid     = q.Ls.AgentOid,
                    CampaignDesc = q.Campaign != null
                                       ? q.Campaign.Description
                                       : string.Empty,
                })
                .Select(g => new SectionStatRowDto
                {
                    SectionId    = g.Key.SectionId,
                    Section      = g.Key.SectionName  ?? string.Empty,
                    Agent        = g.Key.Agent         ?? string.Empty,
                    AgentId      = g.Key.AgentOid      ?? string.Empty,
                    Campaign     = g.Key.CampaignDesc  ?? string.Empty,
                    ScorePercent = Math.Round(
                        g.Average(x => (double?)x.ScoreGroup) ?? 0.0, 2),
                })
                .OrderBy(r => r.Section)
                .ThenBy(r => r.Agent)
                .ToListAsync();

            return new SectionStatResponseDto
            {
                Rows  = rows,
                Total = rows.Count,
            };
        }
    }
}