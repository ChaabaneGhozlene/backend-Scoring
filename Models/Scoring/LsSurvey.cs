using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsSurvey
{
    public int Id { get; set; }

    public int? RecordDataId { get; set; }

    public int LsId { get; set; }

    public float? Score { get; set; }

    public string? Memo { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? IsSaved { get; set; }

    public string? MemoActionTaken { get; set; }

    public int? IdCategories { get; set; }

    public int? IdCallReason { get; set; }

    public int? AgentIdLs { get; set; }

    public virtual LsCallReason? IdCallReasonNavigation { get; set; }

    public virtual LsCategory? IdCategoriesNavigation { get; set; }

    public virtual L Ls { get; set; } = null!;

    public virtual ICollection<LsSurveyItem> LsSurveyItems { get; set; } = new List<LsSurveyItem>();
}
