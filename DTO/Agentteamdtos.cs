namespace scoring_Backend.DTO
{
    // ── Lecture ──────────────────────────────────────────────────────────────

    /// <summary>Groupe d'agents (tListAgentTeam)</summary>
    public record AgentTeamDto(
        int    Id,
        string Description,
        int    IdSite,
        string SiteDescription
    );

    /// <summary>Agent appartenant à un groupe (tAgentTeams + tListAgents)</summary>
    public record AgentTeamMemberDto(
        int    Id,          // tAgentTeams.Id
        string AgentOid,    // tAgentTeams.AgentOid
        int    AgentId,     // tListAgents.Ident
        string AgentName    // CONCAT(Prenom,' ',Nom)
    );

    /// <summary>Agent disponible (tListAgents)</summary>
    public record AvailableAgentDto(
        string Oid,
        string Name         // CONCAT(Prenom,' ',Nom)
    );

    /// <summary>Site / Customer (distinct depuis tListAgents)</summary>
    public record AgentSiteDto(
        int    Id,          // customerId
        string Description  // customer
    );

    // ── Création ─────────────────────────────────────────────────────────────

    public record CreateAgentTeamDto(
        string       Description,
        int          IdSite,
        List<string> AgentOids    // liste des Oid des agents sélectionnés
    );

    // ── Modification ─────────────────────────────────────────────────────────

    public record UpdateAgentTeamDto(
        string       Description,
        List<string> AgentOids    // nouvelle liste complète d'agents
    );
    /// <summary>
/// DTO pour la suppression de membres d'un groupe.
/// </summary>
public record RemoveMembersDto(List<string> AgentOids);
}