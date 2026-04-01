// Services/DynamicQueryEngine.cs
/*
public class DynamicQueryEngine
{
        private readonly SqrScoringContext _db;

    // Catalogue des DataSources disponibles avec leurs champs exposables
    private static readonly Dictionary<string, DataSourceDescriptor> _catalog = new()
    {
        ["VWListLsSurveyGVNew"] = new(
            TableOrView: "VWListLsSurveyGVNew",
            AvailableFields: new[]
            {
                new FieldDescriptor("NomAgent",       "Agent Last Name",         "string"),
                new FieldDescriptor("PrenomAgent",    "Agent First Name",        "string"),
                new FieldDescriptor("LastAgent",      "Agent ID",                "string"),
                new FieldDescriptor("Score",          "Evaluation Score (%)",    "decimal"),
                new FieldDescriptor("CreateDate",     "Evaluation Date",         "datetime"),
                new FieldDescriptor("CampaignDescription", "Campaign",           "string"),
                new FieldDescriptor("userName",       "Auditor",                 "string"),
                new FieldDescriptor("CallLocalTime",  "Call Local Time",         "string"),
                new FieldDescriptor("ConvDuration",   "Conv Duration",           "string"),
                new FieldDescriptor("NumeroTel",      "Phone Number",            "string"),
                new FieldDescriptor("Memo",           "Comment",                 "string"),
                new FieldDescriptor("Des_Categories", "Category",               "string"),
                new FieldDescriptor("Des_CallReason", "Call Reason",             "string"),
                new FieldDescriptor("Is_saved_desc",  "Call Evaluated",          "string"),
            }
        ),
        ["Ls_survey"] = new(
            TableOrView: "Ls_survey",
            AvailableFields: new[]
            {
                new FieldDescriptor("CreateDate",     "Date",                    "datetime"),
                new FieldDescriptor("Ls.Agent",       "Agent",                   "string"),
                new FieldDescriptor("Score",          "Score (%)",               "decimal"),
                new FieldDescriptor("Id",             "Evaluation Count",        "int"),
            }
        ),
    };

    public DynamicQueryEngine(AppDbContext db) => _db = db;

    public async Task<DynamicQueryResult> ExecuteAsync(
        DynamicQueryRequest req, 
        UserContext user)
    {
        var descriptor = _catalog[req.DataSource];

        // Valide que les champs demandés sont dans le catalogue (sécurité)
        var safeFields = req.SelectedFields
            .Where(f => descriptor.AvailableFields.Any(af => af.Name == f))
            .ToList();

        if (!safeFields.Any())
            safeFields = descriptor.AvailableFields.Select(f => f.Name).Take(6).ToList();

        var sql = BuildSql(descriptor, safeFields, req, user);
        var rows = await _db.QueryRawAsync(sql);

        return new DynamicQueryResult(
            Rows: rows,
            TotalCount: rows.Count,
            AvailableFields: descriptor.AvailableFields.Select(f => f.Name).ToList()
        );
    }

    private string BuildSql(
        DataSourceDescriptor ds, 
        List<string> fields, 
        DynamicQueryRequest req, 
        UserContext user)
    {
        var selectParts = new List<string>();

        foreach (var f in fields)
        {
            var fd = ds.AvailableFields.First(x => x.Name == f);
            selectParts.Add(fd.SqlExpression ?? $"[{f}]");
        }

        // Agrégat si demandé
        if (req.AggregateType != null && req.GroupByField != null)
        {
            var aggField = fields.FirstOrDefault(f => f == "Score") ?? fields.Last();
            var agg = req.AggregateType switch
            {
                "Average" => $"AVG(CAST([{aggField}] AS FLOAT))",
                "Count"   => $"COUNT([Id])",
                "Sum"     => $"SUM(CAST([{aggField}] AS FLOAT))",
                _ => $"AVG(CAST([{aggField}] AS FLOAT))"
            };
            selectParts.Add($"{agg} AS AggregateValue");
        }

        var sb = new StringBuilder();
        sb.AppendLine($"SELECT {string.Join(", ", selectParts)}");
        sb.AppendLine($"FROM [{ds.TableOrView}]");
        sb.AppendLine("WHERE Is_saved = 1");
        sb.AppendLine($"  AND CreateDate >= '{req.DateFrom:yyyy-MM-dd} 00:00:00'");
        sb.AppendLine($"  AND CreateDate <= '{req.DateTo:yyyy-MM-dd} 23:59:59'");

        // Filtre utilisateur
        if (!string.IsNullOrEmpty(req.FilterExpression))
            sb.AppendLine($"  AND ({req.FilterExpression})");

        // Restriction par rôle (remplace la logique dispersée dans chaque page)
        AppendRoleFilter(sb, user);

        // Filtre équipe
        if (req.TeamId.HasValue)
            sb.AppendLine($"  AND ls_surveyAgentIdLs IN " +
                $"(SELECT tListAgents.Ident FROM tAgentTeams " +
                $"WHERE tListAgentTeam_Id = {req.TeamId})");

        if (req.AggregateType != null && req.GroupByField != null)
        {
            var safeGroup = req.GroupByField.Replace("'", "''");
            sb.AppendLine($"GROUP BY [{safeGroup}]");
            sb.AppendLine($"ORDER BY [{safeGroup}]");
        }

        sb.AppendLine($"ORDER BY CreateDate DESC");
        sb.AppendLine($"OFFSET {req.Page * req.PageSize} ROWS " +
                      $"FETCH NEXT {req.PageSize} ROWS ONLY");

        return sb.ToString();
    }

    private static void AppendRoleFilter(StringBuilder sb, UserContext user)
    {
        // Centralise la logique de rôle qui était dupliquée dans les 4 pages
        switch (user.Role)
        {
            case UserRole.SiteManager:
                sb.AppendLine($"  AND Ls_CSite = {user.SiteId}");
                break;
            case UserRole.Auditor:
                sb.AppendLine($"  AND Ls_CSite = {user.SiteId}");
                // + campagnes autorisées
                break;
            // Role 1 (admin) = pas de filtre
        }
    }

    public List<FieldDescriptor> GetAvailableFields(string dataSource) =>
        _catalog.TryGetValue(dataSource, out var ds) 
            ? ds.AvailableFields.ToList() 
            : new();
}

// Descripteurs (records C# 9+)
public record DataSourceDescriptor(string TableOrView, FieldDescriptor[] AvailableFields);
public record FieldDescriptor(string Name, string Caption, string Type, string? SqlExpression = null);*/