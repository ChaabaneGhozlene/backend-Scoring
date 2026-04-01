namespace scoring_Backend.DTO
{
    // ── Lecture ───────────────────────────────────────────────────────────────
    public class UserDto
    {
        public int     Id        { get; set; }
        public string  Login     { get; set; } = string.Empty;
        public string  FirstName { get; set; } = string.Empty;
        public string  LastName  { get; set; } = string.Empty;
        public bool    IsActive  { get; set; }
        public string  SiteName  { get; set; } = string.Empty;
        public int     SiteId    { get; set; }
    }

    // ── Création ──────────────────────────────────────────────────────────────
    public class CreateUserDto
    {
        public string Login     { get; set; } = string.Empty;
        public string Password  { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public bool   IsActive  { get; set; } = true;
        public int    SiteId    { get; set; }
        public string SiteName  { get; set; } = string.Empty;
    }

    // ── Modification ──────────────────────────────────────────────────────────
    public class UpdateUserDto
    {
        public string  Login     { get; set; } = string.Empty;
        public string? Password  { get; set; }
        public string  FirstName { get; set; } = string.Empty;
        public string  LastName  { get; set; } = string.Empty;
        public bool    IsActive  { get; set; }
        public int     SiteId    { get; set; }
        public string  SiteName  { get; set; } = string.Empty;
    }

    // ── Liste paginée ─────────────────────────────────────────────────────────
    public class PaginatedUsersDto
    {
        public int           TotalCount { get; set; }
        public int           Page       { get; set; }
        public int           PageSize   { get; set; }
        public List<UserDto> Items      { get; set; } = [];
    }

    // ── Site ──────────────────────────────────────────────────────────────────
    public class SiteDto
    {
        public int    Id   { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    
}