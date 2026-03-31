using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class L
{
    public int Id { get; set; }

    public string AgentOid { get; set; } = null!;

    public int Auditor { get; set; }

    public string? Memo { get; set; }

    public float? Score { get; set; }

    public int CalledCampaignId { get; set; }

    public DateTime? StartPeriode { get; set; }

    public DateTime? EndPeriode { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? AgentId { get; set; }

    public string? Agent { get; set; }

    public string? MemoActionTaken { get; set; }

    public int? IdCategories { get; set; }

    public int? IdCallReason { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<LsSurvey> LsSurveys { get; set; } = new List<LsSurvey>();
}
