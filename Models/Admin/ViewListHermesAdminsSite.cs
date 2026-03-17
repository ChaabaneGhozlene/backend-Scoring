using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesAdminsSite
{
    public string UserOid { get; set; } = null!;

    public string Login { get; set; } = null!;

    public int CustomerId { get; set; }

    public string CustomerOid { get; set; } = null!;

    public string Description { get; set; } = null!;
}
