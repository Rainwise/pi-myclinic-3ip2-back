using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Admin
{
    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;
}
