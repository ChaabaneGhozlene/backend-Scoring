using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class UsersApplication
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ApplicationId { get; set; }

    public int ProfilId { get; set; }
}
