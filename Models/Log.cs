using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Log
{
    public int IdLog { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Timestamp { get; set; }
}
