using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Specialization
{
    public int IdSpecialization { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
