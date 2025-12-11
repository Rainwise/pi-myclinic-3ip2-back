using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Doctor
{
    public int IdDoctor { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public int SpecializationId { get; set; }

    public string LicenseNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Specialization Specialization { get; set; } = null!;
}
