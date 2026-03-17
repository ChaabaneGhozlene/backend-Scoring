using scoring_Backend.DTO.Configuration;

namespace scoring_Backend.Repositories.Interfaces.Configuration
{
    /// <summary>
    /// Interface du repository pour la configuration email des agents.
    /// </summary>
    public interface IAgentMailConfigRepository
    {
        /// <summary>
        /// Retourne la liste des agents avec leur email.
        /// Role == 1 (Admin) => tous les agents.
        /// Sinon             => agents liés à l'utilisateur connecté.
        /// </summary>
        Task<IEnumerable<AgentMailConfigDto>> GetAgentsWithEmailAsync(int userId, string userRole);

        /// <summary>
        /// Retourne le détail d'un agent pour le popup d'édition (Ident, Nom complet, Email).
        /// </summary>
        Task<AgentMailEditDetailDto?> GetAgentEditDetailAsync(string oid);

        /// <summary>
        /// Crée ou met à jour l'email de notification d'un agent.
        /// </summary>
        Task UpsertAgentEmailAsync(UpdateAgentEmailDto dto);
    }
}