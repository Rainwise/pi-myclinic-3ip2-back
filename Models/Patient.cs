using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Patient
{
    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
