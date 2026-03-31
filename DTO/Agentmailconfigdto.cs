namespace scoring_Backend.DTO.Configuration
{
    /// <summary>
    /// DTO représentant un agent avec son email — utilisé pour la grille principale.
    /// </summary>
    public class AgentMailConfigDto
    {
        public int     Id    { get; set; }
        public string  Oid   { get; set; } = string.Empty;
        public string  Agent { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    /// <summary>
    /// DTO pour la mise à jour de l'email d'un agent (body du PUT).
    /// </summary>
    public class UpdateAgentEmailDto
    {
        public string Oid     { get; set; } = string.Empty;
        public int    AgentId { get; set; }
        public string Email   { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO pour le détail d'un agent dans le popup d'édition.
    /// </summary>
    public class AgentMailEditDetailDto
    {
        public int     Ident    { get; set; }
        public string  Oid      { get; set; } = string.Empty;
        public string  FullName { get; set; } = string.Empty;
        public string? Email    { get; set; }
    }
}