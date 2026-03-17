namespace scoring_Backend.DTO
{
    /// <summary>
    /// Envoyé par le frontend pour requalifier un enregistrement.
    /// Les champs int? correspondent aux types dans RecordDatum.cs
    /// </summary>
    public class RequalificationDto
    {
        public int RecordId { get; set; }

        /// <summary>
        /// Type de requalification.
        /// RecordDatum.TypeRequalif est int? — la valeur sera parsée côté repo.
        /// </summary>
        public string? TypeRequalif { get; set; }

        /// <summary>RecordDatum.StatusRequal (string?)</summary>
        public string? StatusRequal { get; set; }

        /// <summary>RecordDatum.StatusGroupeRequal (int?)</summary>
        public int? StatusGroupeRequal { get; set; }

        /// <summary>RecordDatum.StatusNumRequal (int?)</summary>
        public int? StatusNumRequal { get; set; }

        /// <summary>RecordDatum.StatusDetailRequal (int?)</summary>
        public int? StatusDetailRequal { get; set; }
    }
}