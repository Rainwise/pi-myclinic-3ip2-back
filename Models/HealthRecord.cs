using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class HealthRecord
{
    public int IdHealthRecord { get; set; }

    public int? PatientId { get; set; }

    public virtual Patient? Patient { get; set; }
}
