using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Admin
{
    public int IdUser { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public string PasswordSalt { get; set; } = null!;
}
