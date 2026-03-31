using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Admin;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Configuration;

namespace scoring_Backend.Repositories.Implementations.Configuration
{
    public class ConfigurationCampagnesRepository : IConfigurationCampagnesRepository
    {
        private readonly SqrScoringContext _db;
        private readonly SqrAdminContext _adminDb;

        private SqlConnection OpenConnection()
        {
            var connectionString = _db.Database.GetConnectionString();
            return new SqlConnection(connectionString);
        }

        private SqlConnection OpenAdminConnection()
        {
            var connectionString = _adminDb.Database.GetConnectionString();
            return new SqlConnection(connectionString);
        }

        public ConfigurationCampagnesRepository(SqrScoringContext db, SqrAdminContext adminDb)
        {
            _db = db;
            _adminDb = adminDb;
        }

        // ════════════════════════════════════════════════════════════════════
        // TEMPLATES
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<LsTemplateDto>> GetAllTemplatesAsync()
        {
            const string sql = @"
                SELECT t.Id, t.Description, t.Min, t.Max, t.version, t.status,
                       t.related_template, t.StartDate, t.EndDate,
                       t.PeriodeId, p.Description AS PeriodeDescription
                FROM dbo.Ls_template t
                LEFT JOIN dbo.Ls_templatePeriode p ON p.Id = t.PeriodeId
                ORDER BY t.Description";

            var list = new List<LsTemplateDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapTemplate(reader));
            return list;
        }

        public async Task<LsTemplateDto?> GetTemplateByIdAsync(int id)
        {
            const string sql = @"
                SELECT t.Id, t.Description, t.Min, t.Max, t.version, t.status,
                       t.related_template, t.StartDate, t.EndDate,
                       t.PeriodeId, p.Description AS PeriodeDescription
                FROM dbo.Ls_template t
                LEFT JOIN dbo.Ls_templatePeriode p ON p.Id = t.PeriodeId
                WHERE t.Id = @Id";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapTemplate(reader) : null;
        }

        public async Task<bool> TemplateExistsByDescriptionAsync(string description)
        {
            const string sql = "SELECT COUNT(1) FROM dbo.Ls_template WHERE Description = @Description";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Description", description);
            return (int)(await cmd.ExecuteScalarAsync())! > 0;
        }

        public async Task<int> CreateTemplateAsync(CreateLsTemplateDto dto)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine("║        CreateTemplateAsync — START           ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.WriteLine($"  Description  : {dto.Description}");
                Console.WriteLine($"  ItemGroups   : {dto.ItemGroups?.Count() ?? 0} groupe(s)");
                Console.WriteLine($"  Campaigns    : {dto.SelectedCampaignParams?.Count() ?? 0} campagne(s)");

                const string insertTemplate = @"
                    INSERT INTO dbo.Ls_template
                        (Description, Min, Max, TypeId, ActiveMinMax, version, status,
                         StartDate, EndDate, PeriodeId)
                    VALUES
                        (@Description, @Min, @Max, 1, 1, 1, 1,
                         @StartDate, @EndDate, @PeriodeId);
                    SELECT SCOPE_IDENTITY();";

                var startDate = DateTime.TryParse(dto.StartDate, out var sd) ? sd : DateTime.Now;
                var endDate   = DateTime.TryParse(dto.EndDate,   out var ed) ? ed : DateTime.Now.AddYears(1);

                int templateId;
                await using (var cmd = new SqlCommand(insertTemplate, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Description", dto.Description);
                    cmd.Parameters.AddWithValue("@Min",         dto.Min);
                    cmd.Parameters.AddWithValue("@Max",         dto.Max);
                    cmd.Parameters.AddWithValue("@StartDate",   startDate);
                    cmd.Parameters.AddWithValue("@EndDate",     endDate);
                    cmd.Parameters.AddWithValue("@PeriodeId",   dto.LsTemplatePeriodeId);
                    templateId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                Console.WriteLine($"  ✅ Ls_template inséré → Id = {templateId}");

                foreach (var group in dto.ItemGroups ?? Enumerable.Empty<ItemGroupDto>())
                {
                    const string insertGroup = @"
                        INSERT INTO dbo.Ls_templateItemGroup
                            (Description, Coef, [Order], TemplateId)
                        VALUES (@Desc, @Coef, @Order, @TemplateId);
                        SELECT SCOPE_IDENTITY();";

                    int groupId;
                    await using (var cmd = new SqlCommand(insertGroup, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@Desc",       group.Description);
                        cmd.Parameters.AddWithValue("@Coef",       group.Coef);
                        cmd.Parameters.AddWithValue("@Order",      group.Order);
                        cmd.Parameters.AddWithValue("@TemplateId", templateId);
                        groupId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }

                    foreach (var item in group.Items ?? Enumerable.Empty<TemplateItemDto>())
                        await InsertTemplateItemAsync(conn, tran, item, groupId);
                }

                foreach (var param in dto.SelectedCampaignParams ?? Enumerable.Empty<string>())
                    await InsertCalledCampaignAsync(conn, tran, templateId, param, DateTime.Now, DateTime.Now.AddYears(1));

                await tran.CommitAsync();
                Console.WriteLine($"  ✅✅ COMMIT réussi — templateId = {templateId}");
                return templateId;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                Console.WriteLine($"  ❌ ROLLBACK : {ex.Message}");
                throw;
            }
        }

        public async Task UpdateTemplateAsync(int id, UpdateLsTemplateDto dto)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                const string sql = @"
                    UPDATE dbo.Ls_template
                    SET Description = @Description, Min = @Min, Max = @Max,
                        StartDate = @StartDate, EndDate = @EndDate,
                        PeriodeId = @PeriodeId
                    WHERE Id = @Id";

                var startDate = DateTime.TryParse(dto.StartDate, out var sd) ? sd : DateTime.Now;
                var endDate   = DateTime.TryParse(dto.EndDate,   out var ed) ? ed : DateTime.Now.AddYears(1);

                await using (var cmd = new SqlCommand(sql, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id",          id);
                    cmd.Parameters.AddWithValue("@Description", dto.Description);
                    cmd.Parameters.AddWithValue("@Min",         dto.Min);
                    cmd.Parameters.AddWithValue("@Max",         dto.Max);
                    cmd.Parameters.AddWithValue("@StartDate",   startDate);
                    cmd.Parameters.AddWithValue("@EndDate",     endDate);
                    cmd.Parameters.AddWithValue("@PeriodeId",   dto.LsTemplatePeriodeId);
                    await cmd.ExecuteNonQueryAsync();
                }

                if (dto.ItemGroups != null && dto.ItemGroups.Any())
                {
                    const string deleteItems = @"
                        DELETE i FROM dbo.Ls_templateItem i
                        INNER JOIN dbo.Ls_templateItemGroup g ON g.Id = i.GroupId
                        WHERE g.TemplateId = @TemplateId";
                    await using (var cmd = new SqlCommand(deleteItems, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@TemplateId", id);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    const string deleteGroups = "DELETE FROM dbo.Ls_templateItemGroup WHERE TemplateId = @TemplateId";
                    await using (var cmd = new SqlCommand(deleteGroups, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@TemplateId", id);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    foreach (var group in dto.ItemGroups)
                    {
                        const string insertGroup = @"
                            INSERT INTO dbo.Ls_templateItemGroup
                                (Description, Coef, [Order], TemplateId)
                            VALUES (@Desc, @Coef, @Order, @TemplateId);
                            SELECT SCOPE_IDENTITY();";

                        int groupId;
                        await using (var cmd = new SqlCommand(insertGroup, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Desc",       group.Description);
                            cmd.Parameters.AddWithValue("@Coef",       group.Coef);
                            cmd.Parameters.AddWithValue("@Order",      group.Order);
                            cmd.Parameters.AddWithValue("@TemplateId", id);
                            groupId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        }

                        foreach (var item in group.Items ?? Enumerable.Empty<TemplateItemDto>())
                            await InsertTemplateItemAsync(conn, tran, item, groupId);
                    }
                }

                foreach (var param in dto.SelectedCampaignParams ?? Enumerable.Empty<string>())
                    await InsertCalledCampaignAsync(conn, tran, id, param, null, null);

                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CanDeleteTemplateAsync(int id)
        {
            const string sql = @"
                SELECT COUNT(1) FROM dbo.Ls l
                INNER JOIN dbo.Ls_CalledCampaign c ON c.Id = l.CalledCampaignId
                WHERE c.IdLsTemplate = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            return (int)(await cmd.ExecuteScalarAsync())! == 0;
        }

        public async Task DeleteTemplateAsync(int id)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                const string checkVersion = "SELECT version, related_template FROM dbo.Ls_template WHERE Id = @Id";
                await using (var cmd = new SqlCommand(checkVersion, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int version  = reader.GetInt32(0);
                        int? related = reader.IsDBNull(1) ? null : reader.GetInt32(1);
                        await reader.CloseAsync();
                        if (version > 1 && related.HasValue)
                        {
                            const string restore = "UPDATE dbo.Ls_template SET status = 1 WHERE Id = @RelId";
                            await using var restoreCmd = new SqlCommand(restore, conn, tran);
                            restoreCmd.Parameters.AddWithValue("@RelId", related.Value);
                            await restoreCmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                const string deleteSql = "DELETE FROM dbo.Ls_template WHERE Id = @Id";
                await using (var cmd = new SqlCommand(deleteSql, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
                await tran.CommitAsync();
            }
            catch { await tran.RollbackAsync(); throw; }
        }

        // ════════════════════════════════════════════════════════════════════
        // CHANGE MODEL
        // ════════════════════════════════════════════════════════════════════

        public async Task<bool> HasPendingVersionAsync(int templateId)
        {
            const string sql = "SELECT COUNT(1) FROM dbo.Ls_template WHERE related_template = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", templateId);
            return (int)(await cmd.ExecuteScalarAsync())! > 0;
        }

        public async Task<int> ChangeModelAsync(int originalTemplateId, ChangeModelDto dto)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                int originalVersion;
                const string getOrig = "SELECT version FROM dbo.Ls_template WHERE Id = @Id";
                await using (var cmd = new SqlCommand(getOrig, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", originalTemplateId);
                    originalVersion = (int)(await cmd.ExecuteScalarAsync())!;
                }

                const string insertTmpl = @"
                    INSERT INTO dbo.Ls_template
                        (Description, Min, Max, TypeId, ActiveMinMax, version, status,
                         related_template, StartDate, EndDate, PeriodeId)
                    VALUES
                        (@Description, @Min, @Max, 1, 1, @Version, 1,
                         @Related, @StartDate, @EndDate, @PeriodeId);
                    SELECT SCOPE_IDENTITY();";

                int newId;
                var startDate = DateTime.TryParse(dto.StartDate, out var sd) ? sd : DateTime.Now;
                var endDate   = DateTime.TryParse(dto.EndDate,   out var ed) ? ed : DateTime.Now.AddYears(1);

                await using (var cmd = new SqlCommand(insertTmpl, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Description", dto.Description);
                    cmd.Parameters.AddWithValue("@Min",         dto.Min);
                    cmd.Parameters.AddWithValue("@Max",         dto.Max);
                    cmd.Parameters.AddWithValue("@Version",     originalVersion + 1);
                    cmd.Parameters.AddWithValue("@Related",     originalTemplateId);
                    cmd.Parameters.AddWithValue("@StartDate",   startDate);
                    cmd.Parameters.AddWithValue("@EndDate",     endDate);
                    cmd.Parameters.AddWithValue("@PeriodeId",   dto.LsTemplatePeriodeId);
                    newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                const string deactivate = "UPDATE dbo.Ls_template SET status = 0 WHERE Id = @Id";
                await using (var cmd = new SqlCommand(deactivate, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", originalTemplateId);
                    await cmd.ExecuteNonQueryAsync();
                }

                foreach (var group in dto.ItemGroups ?? Enumerable.Empty<ItemGroupDto>())
                {
                    const string insertGroup = @"
                        INSERT INTO dbo.Ls_templateItemGroup
                            (Description, Coef, [Order], TemplateId)
                        VALUES (@Desc, @Coef, @Order, @TemplateId);
                        SELECT SCOPE_IDENTITY();";

                    int groupId;
                    await using (var cmd = new SqlCommand(insertGroup, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@Desc",       group.Description);
                        cmd.Parameters.AddWithValue("@Coef",       group.Coef);
                        cmd.Parameters.AddWithValue("@Order",      group.Order);
                        cmd.Parameters.AddWithValue("@TemplateId", newId);
                        groupId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }

                    foreach (var item in group.Items ?? Enumerable.Empty<TemplateItemDto>())
                        await InsertTemplateItemAsync(conn, tran, item, groupId);
                }

                var campaigns = new List<(int Site, string Did, string CampDesc, string Desc)>();
                const string getCampaigns = @"
                    SELECT Site, CampagneDID, CampagneDescription, Description
                    FROM dbo.Ls_CalledCampaign WHERE IdLsTemplate = @OrigId";
                await using (var cmd = new SqlCommand(getCampaigns, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@OrigId", originalTemplateId);
                    await using var r = await cmd.ExecuteReaderAsync();
                    while (await r.ReadAsync())
                        campaigns.Add((
                            r.GetInt32(0),
                            r.IsDBNull(1) ? string.Empty : r.GetString(1),
                            r.IsDBNull(2) ? string.Empty : r.GetString(2),
                            (r.IsDBNull(3) ? string.Empty : r.GetString(3)) + " Version " + (originalVersion + 1)
                        ));
                }

                foreach (var (site, did, campDesc, desc) in campaigns)
                {
                    const string insertCamp = @"
                        INSERT INTO dbo.Ls_CalledCampaign
                            (Description, Site, CampagneDID, CampagneDescription,
                             StartDate, EndDate, Status, IdLsTemplate)
                        VALUES (@Desc, @Site, @Did, @CampDesc, @Start, @End, 1, @TemplateId)";
                    await using var cmd = new SqlCommand(insertCamp, conn, tran);
                    cmd.Parameters.AddWithValue("@Desc",       desc);
                    cmd.Parameters.AddWithValue("@Site",       site);
                    cmd.Parameters.AddWithValue("@Did",        did);
                    cmd.Parameters.AddWithValue("@CampDesc",   campDesc);
                    cmd.Parameters.AddWithValue("@Start",      DateTime.Now);
                    cmd.Parameters.AddWithValue("@End",        DateTime.Now.AddYears(1));
                    cmd.Parameters.AddWithValue("@TemplateId", newId);
                    await cmd.ExecuteNonQueryAsync();
                }

                foreach (var param in dto.SelectedCampaignParams ?? Enumerable.Empty<string>())
                    await InsertCalledCampaignAsync(conn, tran, newId, param, DateTime.Now, DateTime.Now.AddYears(1));

                await tran.CommitAsync();
                return newId;
            }
            catch { await tran.RollbackAsync(); throw; }
        }

        // ════════════════════════════════════════════════════════════════════
        // ITEM GROUPS
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<ItemGroupDto>> GetItemGroupsByTemplateAsync(int templateId)
        {
            // ✅ Question ajouté dans le SELECT
            const string sql = @"
                SELECT g.Id, g.Description, g.Coef, g.[Order],
                       i.Id AS ItemId, i.Description AS ItemDesc,
                       i.Question,
                       i.Min, i.Max, i.Coef AS ItemCoef, i.[Order] AS ItemOrder,
                       i.is_NA, i.is_killer_question, i.is_killer_section
                FROM dbo.Ls_templateItemGroup g
                LEFT JOIN dbo.Ls_templateItem i ON i.GroupId = g.Id
                WHERE g.TemplateId = @TemplateId
                ORDER BY g.[Order], i.[Order]";

            var groups = new Dictionary<int, (ItemGroupDto group, List<TemplateItemDto> items)>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TemplateId", templateId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int gId = reader.GetInt32(0);
                if (!groups.ContainsKey(gId))
                {
                    var items = new List<TemplateItemDto>();
                    var g = new ItemGroupDto(gId, reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), items);
                    groups[gId] = (g, items);
                }

                if (!reader.IsDBNull(4))
                {
                    // ✅ Index décalés de +1 après Question (colonne 6)
                    groups[gId].items.Add(new TemplateItemDto(
                        reader.GetInt32(4),                                       // Id
                        reader.IsDBNull(5) ? string.Empty : reader.GetString(5), // Description
                        reader.IsDBNull(6) ? null : reader.GetString(6),          // Question ✅
                        reader.GetInt32(7),                                       // Min
                        reader.GetInt32(8),                                       // Max
                        reader.GetInt32(9),                                       // Coef
                        reader.GetInt32(10),                                      // Order
                        reader.GetInt32(11),                                      // IsNa
                        reader.GetInt32(12),                                      // IsKillerQuestion
                        reader.GetInt32(13)));                                    // IsKillerSection
                }
            }
            return groups.Values.Select(v => v.group);
        }

        public async Task<int> CreateItemGroupAsync(int templateId, CreateItemGroupDto dto)
        {
            const string sql = @"
                INSERT INTO dbo.Ls_templateItemGroup (Description, Coef, [Order], TemplateId)
                VALUES (@Desc, @Coef, @Order, @TemplateId);
                SELECT SCOPE_IDENTITY();";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Desc",       dto.Description);
            cmd.Parameters.AddWithValue("@Coef",       dto.Coef);
            cmd.Parameters.AddWithValue("@Order",      dto.Order);
            cmd.Parameters.AddWithValue("@TemplateId", templateId);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateItemGroupAsync(int id, UpdateItemGroupDto dto)
        {
            const string sql = @"
                UPDATE dbo.Ls_templateItemGroup
                SET Description = @Desc, Coef = @Coef, [Order] = @Order
                WHERE Id = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id",    id);
            cmd.Parameters.AddWithValue("@Desc",  dto.Description);
            cmd.Parameters.AddWithValue("@Coef",  dto.Coef);
            cmd.Parameters.AddWithValue("@Order", dto.Order);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteItemGroupAsync(int id)
        {
            const string sql = "DELETE FROM dbo.Ls_templateItemGroup WHERE Id = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ════════════════════════════════════════════════════════════════════
        // TEMPLATE ITEMS
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<TemplateItemDto>> GetItemsByGroupAsync(int groupId)
        {
            // ✅ Question ajouté dans le SELECT
            const string sql = @"
                SELECT Id, Description, Question, Min, Max, Coef, [Order],
                       is_NA, is_killer_question, is_killer_section
                FROM dbo.Ls_templateItem
                WHERE GroupId = @GroupId
                ORDER BY [Order]";

            var list = new List<TemplateItemDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new TemplateItemDto(
                    reader.GetInt32(0),                                       // Id
                    reader.IsDBNull(1) ? string.Empty : reader.GetString(1), // Description
                    reader.IsDBNull(2) ? null : reader.GetString(2),          // Question ✅
                    reader.GetInt32(3),                                       // Min
                    reader.GetInt32(4),                                       // Max
                    reader.GetInt32(5),                                       // Coef
                    reader.GetInt32(6),                                       // Order
                    reader.GetInt32(7),                                       // IsNa
                    reader.GetInt32(8),                                       // IsKillerQuestion
                    reader.GetInt32(9)));                                     // IsKillerSection
            return list;
        }

        public async Task<int> CreateTemplateItemAsync(CreateTemplateItemDto dto)
        {
            // ✅ Question ajouté dans INSERT
            const string sql = @"
                INSERT INTO dbo.Ls_templateItem
                    (Description, Question, Min, Max, Coef, [Order],
                     is_NA, is_killer_question, is_killer_section, GroupId)
                VALUES
                    (@Desc, @Question, @Min, @Max, @Coef, @Order,
                     @IsNa, @IsKillerQ, @IsKillerS, @GroupId);
                SELECT SCOPE_IDENTITY();";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Desc",      dto.Description);
            cmd.Parameters.AddWithValue("@Question",  (object?)dto.Question ?? DBNull.Value); // ✅
            cmd.Parameters.AddWithValue("@Min",       dto.Min);
            cmd.Parameters.AddWithValue("@Max",       dto.Max);
            cmd.Parameters.AddWithValue("@Coef",      dto.Coef);
            cmd.Parameters.AddWithValue("@Order",     dto.Order);
            cmd.Parameters.AddWithValue("@IsNa",      dto.IsNa);
            cmd.Parameters.AddWithValue("@IsKillerQ", dto.IsKillerQuestion);
            cmd.Parameters.AddWithValue("@IsKillerS", dto.IsKillerSection);
            cmd.Parameters.AddWithValue("@GroupId",   dto.LsTemplateItemGroupId);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateTemplateItemAsync(int id, UpdateTemplateItemDto dto)
        {
            // ✅ Question ajouté dans UPDATE
            const string sql = @"
                UPDATE dbo.Ls_templateItem
                SET Description        = @Desc,
                    Question           = @Question,
                    Min                = @Min,
                    Max                = @Max,
                    Coef               = @Coef,
                    [Order]            = @Order,
                    is_NA              = @IsNa,
                    is_killer_question = @IsKillerQ,
                    is_killer_section  = @IsKillerS,
                    GroupId            = @GroupId
                WHERE Id = @Id";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id",        id);
            cmd.Parameters.AddWithValue("@Desc",      dto.Description);
            cmd.Parameters.AddWithValue("@Question",  (object?)dto.Question ?? DBNull.Value); // ✅
            cmd.Parameters.AddWithValue("@Min",       dto.Min);
            cmd.Parameters.AddWithValue("@Max",       dto.Max);
            cmd.Parameters.AddWithValue("@Coef",      dto.Coef);
            cmd.Parameters.AddWithValue("@Order",     dto.Order);
            cmd.Parameters.AddWithValue("@IsNa",      dto.IsNa);
            cmd.Parameters.AddWithValue("@IsKillerQ", dto.IsKillerQuestion);
            cmd.Parameters.AddWithValue("@IsKillerS", dto.IsKillerSection);
            cmd.Parameters.AddWithValue("@GroupId",   dto.LsTemplateItemGroupId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteTemplateItemAsync(int id)
        {
            const string sql = "DELETE FROM dbo.Ls_templateItem WHERE Id = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ════════════════════════════════════════════════════════════════════
        // CALLED CAMPAIGNS
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<LsCalledCampaignDto>> GetCalledCampaignsByTemplateAsync(int templateId)
        {
            const string sql = @"
                SELECT Id, Description, Site, CampagneDID, CampagneDescription,
                       Status, StartDate, EndDate, IdLsTemplate
                FROM dbo.Ls_CalledCampaign
                WHERE IdLsTemplate = @TemplateId";

            var list = new List<LsCalledCampaignDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TemplateId", templateId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var startDate = reader.IsDBNull(6) ? DateTime.Now             : reader.GetDateTime(6);
                var endDate   = reader.IsDBNull(7) ? DateTime.Now.AddYears(1) : reader.GetDateTime(7);
                list.Add(new LsCalledCampaignDto(
                    reader.GetInt32(0),
                    reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    reader.GetInt32(2),
                    reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    reader.GetInt32(5),
                    startDate,
                    endDate,
                    reader.GetInt32(8)));
            }
            return list;
        }

        public async Task<int> CreateCalledCampaignAsync(CreateCalledCampaignDto dto)
        {
            var parts = dto.CampagneParam?.Split(',') ?? Array.Empty<string>();
            bool hasValidParts = parts.Length >= 3;
            int site = 0;
            if (hasValidParts) int.TryParse(parts[0].Trim(), out site);
            var did      = hasValidParts ? parts[1].Trim() : string.Empty;
            var campDesc = hasValidParts ? parts[2].Trim() : (dto.CampagneParam ?? string.Empty);

            const string sql = @"
                INSERT INTO dbo.Ls_CalledCampaign
                    (Description, Site, CampagneDID, CampagneDescription,
                     StartDate, EndDate, Status, IdLsTemplate)
                VALUES
                    (@Desc, @Site, @Did, @CampDesc,
                     GETDATE(), DATEADD(YEAR,1,GETDATE()), @Status, @TemplateId);
                SELECT SCOPE_IDENTITY();";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Desc",       campDesc);
            cmd.Parameters.AddWithValue("@Site",       site);
            cmd.Parameters.AddWithValue("@Did",        did);
            cmd.Parameters.AddWithValue("@CampDesc",   campDesc);
            cmd.Parameters.AddWithValue("@Status",     dto.Status);
            cmd.Parameters.AddWithValue("@TemplateId", dto.LsTemplateId);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateCalledCampaignAsync(int id, UpdateCalledCampaignDto dto)
        {
            var parts = dto.CampagneParam?.Split(',') ?? Array.Empty<string>();
            bool hasValidParts = parts.Length >= 3;
            int site = 0;
            if (hasValidParts) int.TryParse(parts[0].Trim(), out site);
            var did      = hasValidParts ? parts[1].Trim() : string.Empty;
            var campDesc = hasValidParts ? parts[2].Trim() : (dto.CampagneParam ?? string.Empty);

            const string sql = @"
                UPDATE dbo.Ls_CalledCampaign
                SET Description         = @Desc,
                    Site                = @Site,
                    CampagneDID         = @Did,
                    CampagneDescription = @CampDesc,
                    Status              = @Status,
                    IdLsTemplate        = @TemplateId
                WHERE Id = @Id";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id",         id);
            cmd.Parameters.AddWithValue("@Desc",       campDesc);
            cmd.Parameters.AddWithValue("@Site",       site);
            cmd.Parameters.AddWithValue("@Did",        did);
            cmd.Parameters.AddWithValue("@CampDesc",   campDesc);
            cmd.Parameters.AddWithValue("@Status",     dto.Status);
            cmd.Parameters.AddWithValue("@TemplateId", dto.LsTemplateId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> CanDeleteCalledCampaignAsync(int id)
        {
            const string sql = "SELECT COUNT(1) FROM dbo.Ls WHERE CalledCampaignId = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            return (int)(await cmd.ExecuteScalarAsync())! == 0;
        }

        public async Task DeleteCalledCampaignAsync(int id)
        {
            const string sql = "DELETE FROM dbo.Ls_CalledCampaign WHERE Id = @Id";
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ════════════════════════════════════════════════════════════════════
        // LOOKUPS
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<AvailableCampaignDto>> GetAvailableCampaignsBySiteAsync(int customerId, int? templateId = null)
        {
            const string sql = @"
                SELECT Description AS DESCRIP,
                       (CONVERT(VARCHAR, customerId) + ',' + DID + ',' + Description) AS Param
                FROM dbo.tListCampaigns
                WHERE Description IS NOT NULL
                  AND customerId = @CustomerId
                  AND DID NOT IN (
                      SELECT CampagneDID FROM dbo.Ls_CalledCampaign
                      WHERE CampagneDID IS NOT NULL
                        AND (@TemplateId IS NULL OR IdLsTemplate <> @TemplateId))
                ORDER BY DESCRIP";

            var list = new List<AvailableCampaignDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);
            cmd.Parameters.AddWithValue("@TemplateId", (object?)templateId ?? DBNull.Value);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new AvailableCampaignDto(reader.GetString(0), reader.GetString(1)));
            return list;
        }

        public async Task<IEnumerable<AvailableCampaignDto>> GetAvailableCampaignsAsync(int? excludeTemplateId = null)
        {
            const string sql = @"
                SELECT (customer + ' - ' + Description) AS DESCRIP,
                       (CONVERT(VARCHAR, customerId) + ',' + DID + ',' + Description) AS Param
                FROM dbo.tListCampaigns
                WHERE Description IS NOT NULL
                  AND DID NOT IN (SELECT CampagneDID FROM dbo.Ls_CalledCampaign WHERE CampagneDID IS NOT NULL)
                ORDER BY DESCRIP";

            var list = new List<AvailableCampaignDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new AvailableCampaignDto(reader.GetString(0), reader.GetString(1)));
            return list;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            const string sql = @"
                SELECT CustomerID, Description
                FROM dbo.ListCustomers
                WHERE CustomerID <> 0
                ORDER BY Description";

            var list = new List<CustomerDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new CustomerDto(reader.GetInt32(0), reader.GetString(1)));
            return list;
        }

        public async Task<IEnumerable<LsTemplatePeriodeDto>> GetPeriodesAsync()
        {
            const string sql = "SELECT Id, Description FROM dbo.Ls_templatePeriode ORDER BY Description";
            var list = new List<LsTemplatePeriodeDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new LsTemplatePeriodeDto(reader.GetInt32(0), reader.GetString(1)));
            return list;
        }

        // ════════════════════════════════════════════════════════════════════
        // HELPERS PRIVÉS
        // ════════════════════════════════════════════════════════════════════

        private static LsTemplateDto MapTemplate(SqlDataReader r) => new(
            r.GetInt32(0),
            r.GetString(1),
            r.GetInt32(2),
            r.GetInt32(3),
            r.GetInt32(4),
            r.GetInt32(5),
            r.IsDBNull(6)  ? null : r.GetInt32(6),
            r.IsDBNull(7)  ? DateTime.Now             : r.GetDateTime(7),
            r.IsDBNull(8)  ? DateTime.Now.AddYears(1) : r.GetDateTime(8),
            r.IsDBNull(9)  ? null : r.GetInt32(9),
            r.IsDBNull(10) ? null : r.GetString(10)
        );

        // ✅ Question ajouté dans INSERT
        private static async Task InsertTemplateItemAsync(
            SqlConnection conn, SqlTransaction tran,
            TemplateItemDto item, int groupId)
        {
            const string sql = @"
                INSERT INTO dbo.Ls_templateItem
                    (Description, Question, Min, Max, Coef, [Order],
                     is_NA, is_killer_question, is_killer_section, GroupId)
                VALUES
                    (@Desc, @Question, @Min, @Max, @Coef, @Order,
                     @IsNa, @IsKillerQ, @IsKillerS, @GroupId)";

            await using var cmd = new SqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@Desc",      item.Description);
            cmd.Parameters.AddWithValue("@Question",  (object?)item.Question ?? DBNull.Value); // ✅
            cmd.Parameters.AddWithValue("@Min",       item.Min);
            cmd.Parameters.AddWithValue("@Max",       item.Max);
            cmd.Parameters.AddWithValue("@Coef",      item.Coef);
            cmd.Parameters.AddWithValue("@Order",     item.Order);
            cmd.Parameters.AddWithValue("@IsNa",      item.IsNa);
            cmd.Parameters.AddWithValue("@IsKillerQ", item.IsKillerQuestion);
            cmd.Parameters.AddWithValue("@IsKillerS", item.IsKillerSection);
            cmd.Parameters.AddWithValue("@GroupId",   groupId);
            await cmd.ExecuteNonQueryAsync();
        }

        private static async Task InsertCalledCampaignAsync(
            SqlConnection conn, SqlTransaction tran,
            int templateId, string param,
            DateTime? startDate, DateTime? endDate)
        {
            var parts = param?.Split(',') ?? Array.Empty<string>();
            if (parts.Length < 3) return;
            if (!int.TryParse(parts[0].Trim(), out int site)) return;
            var did      = parts[1].Trim();
            var campDesc = parts[2].Trim();

            const string sql = @"
                INSERT INTO dbo.Ls_CalledCampaign
                    (Description, Site, CampagneDID, CampagneDescription,
                     StartDate, EndDate, Status, IdLsTemplate)
                VALUES (@Desc, @Site, @Did, @CampDesc, @Start, @End, 1, @TemplateId)";

            await using var cmd = new SqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@Desc",       campDesc);
            cmd.Parameters.AddWithValue("@Site",       site);
            cmd.Parameters.AddWithValue("@Did",        did);
            cmd.Parameters.AddWithValue("@CampDesc",   campDesc);
            cmd.Parameters.AddWithValue("@Start",      (object?)(startDate ?? DateTime.Now));
            cmd.Parameters.AddWithValue("@End",        (object?)(endDate   ?? DateTime.Now.AddYears(1)));
            cmd.Parameters.AddWithValue("@TemplateId", templateId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}