namespace scoring_Backend.DTO.Statistique
{
    // Filtre envoyé par le front
    public class SectionStatFilterDto
    {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin   { get; set; }
    }

    // Ligne brute renvoyée par le back (équiv. Ls_ScoreSection)
    public class SectionStatRowDto
    {
        public int    SectionId          { get; set; }
        public string Section            { get; set; } = string.Empty;
        public string Agent              { get; set; } = string.Empty;
        public string AgentId            { get; set; } = string.Empty;
        public string Campaign           { get; set; } = string.Empty;
        public double ScorePercent       { get; set; }   // moyenne ScoreGroup
    }

    // Réponse paginée (optionnel)
    public class SectionStatResponseDto
    {
        public List<SectionStatRowDto> Rows  { get; set; } = new();
        public int                     Total { get; set; }
    }
}