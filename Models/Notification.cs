using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Notification
{
    public int IdNotification { get; set; }

    public int? ReservationId { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
