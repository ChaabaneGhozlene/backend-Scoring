// DTOs/ReportDTOs.cs

public record ReportTemplateDto(
    int Id,
    string Name,
    string DataSource,
    List<ColumnDefinitionDto> Columns,
    List<FilterDefinitionDto> Filters,
    ChartConfigDto? Chart
);

public record ColumnDefinitionDto(
    int Id,
    string FieldName,
    string Caption,
    string Area,
    int AreaIndex,
    string? AggregateType,
    string? FormatString,
    bool IsVisible,
    int SortOrder
);

public record FilterDefinitionDto(
    int Id,
    string Name,
    string Expression
);

public record ChartConfigDto(
    string ChartType,
    string? XAxisField,
    string? YAxisField,
    int Width,
    int Height
);

// Requête de données dynamique envoyée par le frontend
public record DynamicQueryRequest(
    string DataSource,
    List<string> SelectedFields,       // colonnes choisies par l'user
    DateTime DateFrom,
    DateTime DateTo,
    string? FilterExpression,          // critère libre
    string? AggregateType,            // "Average", "Count", "Sum"
    string? GroupByField,
    int? TeamId,
    int PageSize = 500,
    int Page = 1
);

public record DynamicQueryResult(
    List<Dictionary<string, object?>> Rows,
    int TotalCount,
    List<string> AvailableFields       // tous les champs dispo selon la DataSource
);