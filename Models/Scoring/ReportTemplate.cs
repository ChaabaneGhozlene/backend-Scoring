// Models/ReportTemplate.cs
namespace scoring_Backend.Models;
public class ReportTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }          // ex: "Statistics", "Grids Monitoring"
    public string DataSource { get; set; }    // ex: "VWListLsSurveyGVNew", "Ls_survey"
    public string UserLogin { get; set; }
    public int Group { get; set; }            // scope utilisateur
    public bool IsShared { get; set; }        // partageable entre users

    public List<ColumnDefinition> Columns { get; set; } = new();
    public List<FilterDefinition> Filters { get; set; } = new();
    public ChartConfiguration? Chart { get; set; }
}

public class ColumnDefinition
{
    public int Id { get; set; }
    public int ReportTemplateId { get; set; }
    public string FieldName { get; set; }     // ex: "NomAgent", "Score"
    public string Caption { get; set; }       // libellé affiché
    public string Area { get; set; }          // "Row", "Column", "Data", "Filter"
    public int AreaIndex { get; set; }
    public string? AggregateType { get; set; } // "Average", "Count", "Sum", null
    public string? FormatString { get; set; } // ex: "{0:n2}"
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }
}

public class FilterDefinition
{
    public int Id { get; set; }
    public int ReportTemplateId { get; set; }
    public string Name { get; set; }
    public string Expression { get; set; }    // expression stockée (DevExpress criteria)
    public int FilterGroup { get; set; }      // 1=Statistics, 2=Collaborators, 3=Grids...
}

public class ChartConfiguration
{
    public int Id { get; set; }
    public int ReportTemplateId { get; set; }
    public string ChartType { get; set; }     // "Bar", "Line", "Pie", etc.
    public string? XAxisField { get; set; }
    public string? YAxisField { get; set; }
    public string? SeriesField { get; set; }
    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 500;
}