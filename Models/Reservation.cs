using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Reservation
{
    public int IdReservation { get; set; }

    public int? AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Patient? Patient { get; set; }
}
