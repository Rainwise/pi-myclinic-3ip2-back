using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Doctor
{
    public int AccountId { get; set; }

    public int? SpecializationId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Specialization? Specialization { get; set; }
}
