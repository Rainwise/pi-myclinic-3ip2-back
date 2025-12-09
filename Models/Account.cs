using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Account
{
    public int IdAccount { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? RoleId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Role? Role { get; set; }
}
