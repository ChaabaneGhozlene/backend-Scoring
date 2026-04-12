// DTO/Statistique/UserDashboardConfigDto.cs
namespace scoring_Backend.DTO.Statistique;

public class UserDashboardConfigDto
{
    public int                userId  { get; set; }
    public List<WidgetInstanceDto> widgets { get; set; } = new();
}

public class WidgetInstanceDto
{
    public string  id         { get; set; } = string.Empty;
    public string  widgetType { get; set; } = string.Empty;
    public string  chartType  { get; set; } = "Bar";
    public string  size       { get; set; } = "medium";
    public string? title      { get; set; }
    public WidgetFiltersDto  filters  { get; set; } = new();
    public WidgetPositionDto position { get; set; } = new();
}

public class WidgetFiltersDto
{
    public string  dateFrom       { get; set; } = string.Empty;
    public string  dateTo         { get; set; } = string.Empty;
    public int?    agentId        { get; set; }
    public bool    allSupervisors { get; set; } = true;
    public string  sortDirection  { get; set; } = "Descending";
}

public class WidgetPositionDto
{
    public int x { get; set; }
    public int y { get; set; }
    public int w { get; set; } = 6;
    public int h { get; set; } = 4;
}