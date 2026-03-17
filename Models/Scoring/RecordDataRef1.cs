using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class RecordDataRef1
{
    public string Id { get; set; } = null!;

    public decimal? Indice { get; set; }

    public string? VCampaignDescription { get; set; }

    public int? VCustomerId { get; set; }

    public string? VCustomerDescription { get; set; }

    public string? VCallTypeDescription { get; set; }

    public string? VNomAgent { get; set; }

    public string? VPrenomAgent { get; set; }

    public string? VAgentOid { get; set; }

    public string? VNumeroTel { get; set; }

    public string? VCallLocalTimeString { get; set; }

    public int? VDuration { get; set; }

    public int? VCallDuration { get; set; }

    public int? VConvDuration { get; set; }

    public int? VWaitDuration { get; set; }

    public int? VTotalWaitDuration { get; set; }

    public string? VAni { get; set; }

    public string? VDnis { get; set; }

    public string? VMemo { get; set; }

    public DateTime? VCallLocalTime { get; set; }

    public int? VCallStatusGroup { get; set; }

    public int? VCallStatusNum { get; set; }

    public int? VCallStatusDetail { get; set; }

    public string? VStatusText { get; set; }

    public string? VCallStatusGroupDescription { get; set; }

    public string? VCallStatusNumDescription { get; set; }

    public string? VCallStatusDetailDescription { get; set; }
}
