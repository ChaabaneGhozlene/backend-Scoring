public class UserDashboardConfig
{
    public int      Id         { get; set; }
    public int      UserId     { get; set; }
    public string   ConfigJson { get; set; } = "[]";
    public DateTime UpdatedAt  { get; set; }
}